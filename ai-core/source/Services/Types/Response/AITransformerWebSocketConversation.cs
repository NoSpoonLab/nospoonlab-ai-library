using Newtonsoft.Json;

namespace AICore.Services.Types.Response
{
    public class AITransformerWebSocketConversationCreated : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("conversation")]
        public AITransformerWebSocketConversation Conversation { get; set; }
    }
    

    public class AITransformerWebSocketConversation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }
    }
}