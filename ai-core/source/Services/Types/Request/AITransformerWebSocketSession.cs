using System.Collections.Generic;
using AICore.Infrastructure.OpenAI;
using AICore.Infrastructure.OpenAI.Data;
using AICore.Services.Types.Response;
using Newtonsoft.Json;

namespace AICore.Services.Types.Request
{
    public class AITransformerWebSocketSessionUpdate : WebsocketBase
    {
        public AITransformerWebSocketSessionUpdate()
        {
            Type = OpenAIClient.WebSocketClientEventType.SessionUpdate.ConvertToString();
        }
        
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("session")]
        public AITransformerWebSocketSession Session { get; set; }
    }
    public class AITransformerWebSocketTool
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("parameters")]
        public AITransformerWebSocketToolParameters Parameters { get; set; }
    }

    public class AITransformerWebSocketToolParameters
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]
        public Dictionary<string, AITransformerWebSocketToolParameterProperty> Properties { get; set; }

        [JsonProperty("required")]
        public List<string> Required { get; set; }
    }

    public class AITransformerWebSocketToolParameterProperty
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}