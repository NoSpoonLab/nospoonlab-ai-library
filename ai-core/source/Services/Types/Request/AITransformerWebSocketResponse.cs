using System.Collections.Generic;
using AICore.Infrastructure.OpenAI;
using AICore.Infrastructure.OpenAI.Data;
using Newtonsoft.Json;

namespace AICore.Services.Types.Request
{
    public class AITransformerWebSocketResponseCreate : WebsocketBase
    {
        public AITransformerWebSocketResponseCreate()
        {
            Type = OpenAIClient.WebSocketClientEventType.ResponseCreate.ConvertToString();
        }
        
        [JsonProperty("event_id", NullValueHandling = NullValueHandling.Ignore)]
        public string EventId { get; set; }

        [JsonProperty("response", NullValueHandling = NullValueHandling.Ignore)]
        public AITransformerWebSocketResponse Response { get; set; }
    }
    
    public class AITransformerWebSocketResponseCancel : WebsocketBase
    {
        public AITransformerWebSocketResponseCancel()
        {
            Type = OpenAIClient.WebSocketClientEventType.ResponseCancel.ConvertToString();
        }
        
        [JsonProperty("event_id", NullValueHandling = NullValueHandling.Ignore)]
        public string EventId { get; set; }
    }

    public class AITransformerWebSocketResponse
    {
        [JsonProperty("modalities", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Modalities { get; set; }

        [JsonProperty("instructions", NullValueHandling = NullValueHandling.Ignore)]
        public string Instructions { get; set; }

        [JsonProperty("voice", NullValueHandling = NullValueHandling.Ignore)]
        public string Voice { get; set; }

        [JsonProperty("output_audio_format", NullValueHandling = NullValueHandling.Ignore)]
        public string OutputAudioFormat { get; set; }

        [JsonProperty("tools", NullValueHandling = NullValueHandling.Ignore)]
        public List<AITransformerWebSocketTool> Tools { get; set; }

        [JsonProperty("tool_choice", NullValueHandling = NullValueHandling.Ignore)] 
        public string ToolChoice { get; set; } = "auto";

        [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public double Temperature { get; set; } = 0.8;

        [JsonProperty("max_output_tokens", NullValueHandling = NullValueHandling.Ignore)]
        public int MaxOutputTokens { get; set; } = 4096;
    }

    public class AITransformerWebSocketProperty
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}