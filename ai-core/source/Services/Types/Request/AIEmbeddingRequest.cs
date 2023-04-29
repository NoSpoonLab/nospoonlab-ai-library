using System;
using AICore.Services.Types.Data;

namespace AICore.Services.Types.Request
{
    /// <summary>
    /// Holds the necessary data required to send an embedding request to an AI model.
    /// It comes with a default choice of AI model.
    /// </summary>
    [Serializable]
    public class AIEmbeddingRequest
    {
        /// <summary>
        /// The input string sent to the AI model for processing.
        /// </summary>
        public string input;
    }
    
    /// <summary>
    /// Maps the JSON response when making an internal embedding request to an AI model.
    /// This class is primarily used for internal operations and parsing.
    /// </summary>
    internal class AIEmbeddingRequestInternal
    {
        /// <summary>
        /// The input string sent to the AI model for processing.
        /// </summary>
        public string input;

        /// <summary>
        /// The string representative of the AI model to be used for this request.
        /// </summary>
        public string model;
    }
}