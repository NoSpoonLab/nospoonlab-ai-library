using Newtonsoft.Json;

namespace AICore.Services.Types.Response
{
    public class AITransformerWebSocketError : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("error")]
        public AITransformerWebSocketErrorDetail Error { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("param")]
        public object Param { get; set; }
    }

    public class AITransformerWebSocketErrorDetail
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("param")]
        public string Param { get; set; }

        [JsonProperty("event_id")]
        public string EventId { get; set; }
    }
}