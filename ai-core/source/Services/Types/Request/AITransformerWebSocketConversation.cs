using System.Collections.Generic;
using AICore.Infrastructure.OpenAI;
using AICore.Infrastructure.OpenAI.Data;
using Newtonsoft.Json;

namespace AICore.Services.Types.Request
{
    public class AITransformerWebSocketConversationItemCreate : WebsocketBase
    {
        public AITransformerWebSocketConversationItemCreate()
        {
            Type = OpenAIClient.WebSocketClientEventType.ConversationItemCreate.ConvertToString();
        }
        
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("previous_item_id")]
        public string PreviousItemId { get; set; }

        [JsonProperty("item")]
        public AITransformerWebSocketConversationItem Item { get; set; }
    }
    
    public class AITransformerWebSocketConversationItemTruncate : WebsocketBase
    {
        public AITransformerWebSocketConversationItemTruncate()
        {
            Type = OpenAIClient.WebSocketClientEventType.ConversationItemTruncate.ConvertToString();
        }
        
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }

        [JsonProperty("audio_end_ms")]
        public int AudioEndMs { get; set; }
    }
    
    public class AITransformerWebSocketConversationItemDelete : WebsocketBase
    {
        public AITransformerWebSocketConversationItemDelete()
        {
            Type = OpenAIClient.WebSocketClientEventType.ConversationItemDelete.ConvertToString();
        }
        
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }
    }

    public class AITransformerWebSocketConversationItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } = "message";

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public List<AITransformerWebSocketContentItem> Content { get; set; }
    }

    public class AITransformerWebSocketContentItem
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "input_text";

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
        
        [JsonProperty("audio", NullValueHandling = NullValueHandling.Ignore)]
        public string Audio { get; set; }
    }
}