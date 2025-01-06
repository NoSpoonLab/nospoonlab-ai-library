namespace AICore.Infrastructure.OpenAI
{
    internal static class OpenAIConstants
    {
        #region Properties

        internal const string API_URL = "https://api.openai.com/v1/";
        internal const string API_CHAT_REQUEST = "chat/completions";
        internal const string API_IMAGE_REQUEST = "images/generations";
        internal const string API_EMBENDDING_REQUEST = "embeddings";
        
        internal const string API_THREAD_REQUEST = "threads";
        internal const string API_ASSISTANTS_REQUEST = "assistants";

        #endregion
    }
}

