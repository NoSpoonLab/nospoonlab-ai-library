using System;

namespace AICore.Infrastructure.GeminiAI.Data
{
    [Serializable]
    public struct GoogleAIAuthTokenRequest
    {
        public string access_token;
        public int expires_in;
        public string scope;
        public string token_type;
    }
}