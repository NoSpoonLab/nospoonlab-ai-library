using System.Threading.Tasks;
using AICore.Infrastructure.NoSpoonAI.Types.Request;
using AICore.Services.Interfaces;
using AICore.Services.Types.Response;
using DependencyInjectionCore;

namespace AICore.Infrastructure.NoSpoonAI
{
    /// <summary>
    /// This class is a wrapper for IAIGPTService calls. It is a framework that allows interaction with IAIGPTService calls
    /// with the added benefit that we can build prompt objects. This allows us a high level of customization in handling prompts.
    /// </summary>
    public class NoSpoonAIClient
    {
        #region Properties

        // Contains instances of all services that have been initialized
        private Services.Services _services;

        #endregion

        #region Initialization

        // Constructor for the class. Initialize the services container.
        public NoSpoonAIClient()
        {
            _services = new Services.Services();
        }

        /// <summary>
        /// Adds a new service to the service collection.
        /// </summary>
        /// <typeparam name="TInterface">Interface type of the service to be added.</typeparam>
        /// <param name="service">Instance of the service to be added.</param>
        public void InitializeService<TInterface>(TInterface service) where TInterface : IService
        {
            _services.Add(service);
        }

        /// <summary>
        /// Verifies if the instance of IAIGPTService is available, and if not already added to the service collection, adds it.
        /// </summary>
        public void Initialize()
        {
            var aiGptService = DIContainer.Get<IAIGPTService>();
            if(aiGptService != null && !_services.Exists<IAIGPTService>()) _services.Add(aiGptService);
        }
        #endregion
        
        #region Methods

        #region Embedding

        /// <summary>
        /// Asynchronously sends an embedding request to the model.
        /// </summary>
        /// <param name="value">The NoSpoonAIEmbeddingRequest carrying the required data for the embedding request.</param>
        /// <returns>A task that represents the asynchronous operation. The result of the task contains an AIEmbeddingResponse with the response details.</returns>
        public async Task<AIEmbeddingResponse> SendEmbeddingRequest<TRequestType>(TRequestType value) where TRequestType : NoSpoonAIEmbeddingRequest
        {
            var gptService = _services.Get<IAIGPTService>();
            var response = await value.Execute<AIEmbeddingResponse>(async () => await gptService.SendEmbeddingRequest(value._Request()));
            return response;
        }

        #endregion

        #region Transformer

        /// <summary>
        /// Asynchronously sends a transformer request and returns an instance of the specified response type.
        /// </summary>
        /// <typeparam name="TResponseType">The response type to be returned upon the execution of the generative pre-trained transformer request.</typeparam>
        /// <param name="value">The NoSpoonAITransformerPromptRequest carrying the required data for the generative pre-trained transformer request.</param>
        /// <returns>A task that represents the asynchronous operation. The result of the task contains the expected response from the generative pre-trained transformer request.</returns>
        public async Task<TResponseType> SendTransformerRequest<TResponseType, TRequestType>(TRequestType value) 
            where TResponseType : class where TRequestType : NoSpoonAITransformerPromptRequest
        {
            var gptService = _services.Get<IAIGPTService>();
            if (!value.IsInitialized())
            {
                value.OnBeginInitialize();
                value.Initialize();
            }
            var response = await value.Execute<TResponseType>(async () => await gptService.SendTransformerRequest(value.Request()));
            return response;
        }
        
        /// <summary>
        /// Asynchronously sends a transformer request and returns a string representation of the result.
        /// </summary>
        /// <typeparam name="TRequestType">Type of the request to be sent to the generative pre-trained transformer.</typeparam>
        /// <param name="value">The NoSpoonAITransformerPromptRequest carrying the required data for the generative pre-trained transformer request.</param>
        /// <returns>A task that represents the asynchronous operation. The result of the task is the string representation of the response from the generative pre-trained transformer request.</returns>
        public async Task<string> SendTransformerRequest<TRequestType>(TRequestType value) where TRequestType : NoSpoonAITransformerPromptRequest
        {
            var gptService = _services.Get<IAIGPTService>();
            
            if (!value.IsInitialized())
            {
                value.OnBeginInitialize();
                value.Initialize();
            }
            var response = await value.Execute<string>(async () => await gptService.SendTransformerRequest(value.Request()));
            return response;
        }

        #endregion

        #endregion
    }
}