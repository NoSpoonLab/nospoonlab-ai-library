using System.Threading.Tasks;
using AICore.Services.Types.Data;
using AICore.Services.Types.Request;
using AICore.Services.Types.Response;

namespace AICore.Services.Interfaces
{
    /// <summary>
    /// Defines the requirements for a service that processes AI requests including embeddings and transformers with generative pre-trained transformers.
    /// </summary>
    /// 
    /// <seealso cref="AICore.Services.Interfaces.IService"/>
    public interface IAIGPTService : IService
    {
        /// <summary>
        /// Asynchronously sends an embedding request, transform a string into a embedding vector of floats.
        /// </summary>
        /// <param name="value">The AIEmbeddingRequest carrying the data required for the embedding request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an AIEmbeddingResponse with the response details.</returns>
        Task<AIEmbeddingResponse> SendEmbeddingRequest(AIEmbeddingRequest value);
      
        /// <summary>
        /// Asynchronously sends a transformer request to be executed by a generative pre-trained transformer model.
        /// </summary>
        /// <param name="value">The AITransformerRequest carrying the information required for the transformer request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an AITransformerResponse with the response details.</returns>
        Task<AITransformerResponse> SendTransformerRequest(AITransformerRequest value);

        /// <summary>
        /// Sets the initial data settings for the AI Service.
        /// </summary>
        /// <param name="value">The AIServiceInitData carrying the initial settings data.</param>
        void SetSettings(AIServiceInitData value);
    }
}