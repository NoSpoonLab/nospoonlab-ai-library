using AICore.Infrastructure.NoSpoonAI.Types.Exceptions;
using AICore.Services.Types.Data;
using AICore.Services.Types.Request;

namespace AICore.Infrastructure.NoSpoonAI.Types.Request
{
    /// <summary>
    /// Defines the structure and functionality of a request to the NoSpoonAI embedding component.
    /// </summary>
    public class NoSpoonAIEmbeddingRequest : NoSpoonAIRequest
    {
        #region Properties

        /// <summary>
        /// The AIEmbeddingRequest instance holding the specifics of an embedding request.
        /// </summary>
        protected AIEmbeddingRequest _request = new AIEmbeddingRequest();
        protected AIModel _model;

        #endregion

        #region Internal Methods

        /// <summary>
        /// Retrieves the current AIEmbeddingRequest instance.
        /// </summary>
        /// <returns>The current AIEmbeddingRequest instance.</returns>
        internal AIEmbeddingRequest _Request() => _request;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpoonAIEmbeddingRequest"/> class with the input for the AI.
        /// </summary>
        /// <param name="input">The input string for the AIEmbeddingRequest.</param>
        public NoSpoonAIEmbeddingRequest(string input) => Set(input);
 
        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpoonAIEmbeddingRequest"/> class with the input and model for the AI.
        /// </summary>
        /// <param name="input">The input string for the AIEmbeddingRequest.</param>
        /// <param name="model">The AI model to be used for the AIEmbeddingRequest.</param>
        public NoSpoonAIEmbeddingRequest(string input, AIModel model) : this(input) => _model = model;

        #endregion

        #region Setters

        /// <summary>
        /// Sets the input for the current AIEmbeddingRequest instance.
        /// </summary>
        /// <param name="input">The input string for the AIEmbeddingRequest.</param>
        /// <exception cref="NoSpoonAIEmptyStringException">Thrown when the input string is null or empty.</exception>
        public void Set(string input)
        {
            if (string.IsNullOrEmpty(input)) throw new NoSpoonAIEmptyStringException();
            _request.input = input;
        }

        #endregion
    }
}