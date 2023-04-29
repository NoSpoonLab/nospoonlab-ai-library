using System;

namespace AICore.Services.Types.Data
{
    /// <summary>
    /// Represents the essential configuration required for initializing an AI service.
    /// This information includes the type of AI service, the API key, and the default AI model to be used.
    /// </summary>
    [Serializable]
    public class AIServiceInitData
    {
        /// <summary>
        /// The type of the AI service provider to be used. 
        /// It determines the AI service through which requests will be sent and responses received.
        /// </summary>
        public AIServiceType Type;

        /// <summary>
        /// The API key for authenticating requests to the selected AI service.
        /// It is a secret key used to authorize and track the requests made to the AI service.
        /// </summary>
        public string APIKey;
    }
}