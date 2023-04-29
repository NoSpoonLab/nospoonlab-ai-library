using System;
using System.Collections.Generic;

namespace AICore.Infrastructure.GeminiAI.Data
{
    [Serializable]
    public struct GoogleAIDataTransformRequest
    {
        public GoogleAIDataContent[] contents;
        public GoogleAIDataSafetySettings safety_settings;
        public GoogleAIDataGenerationConfig generation_config;
    }
    
    
    [Serializable]
    public struct GoogleAIDataTransformResponse
    {
        public List<GoogleAIDataCandidate> candidates;
        public GoogleAIDataUsageMetadata usageMetadata;
    }
    
    [Serializable]
    public struct GoogleAIDataSafetySettings
    {
        public string category;
        public string threshold;
    }

    public struct GoogleAIDataGenerationConfig
    {
        public float temperature;
        public float topP;
        public float topK;
        public int maxOutputTokens;
        public string[] stopSequences;
    }

    [Serializable]
    public struct GoogleAIDataContent
    {
        public string role;
        public GoogleAIDataParts[] parts;
    }

    [Serializable]
    public struct GoogleAIDataParts
    {
        public string text;
    }

    [Serializable]
    public struct GoogleAIDataCandidate
    {
        public GoogleAIDataContent content;
        public string finishReason;
        public GoogleAIDataSafetyRating[] safetyRatings;
    }

    [Serializable]
    public struct GoogleAIDataSafetyRating
    {
        public string category;
        public string probability;
    }

    [Serializable]
    public struct GoogleAIDataUsageMetadata
    {
        public int promptTokenCount;
        public int candidatesTokenCount;
        public int totalTokenCount;
    }
}