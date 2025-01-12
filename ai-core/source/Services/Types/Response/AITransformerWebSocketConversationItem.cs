using System.Collections.Generic;
using Newtonsoft.Json;

namespace AICore.Services.Types.Response
{
    public class AITransformerWebSocketConversationItemCreated : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("previous_item_id")]
        public string PreviousItemId { get; set; }

        [JsonProperty("item")]
        public AITransformerWebSocketItem Item { get; set; }
    }
    
    public class AITransformerWebSocketConversationItemTruncated : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }

        [JsonProperty("audio_end_ms")]
        public int AudioEndMs { get; set; }
    }
    
    public class AITransformerWebSocketConversationItemDeleted : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }
    }

    public class AITransformerWebSocketItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public List<AITransformerWebSocketContent> Content { get; set; }
    }
    

    public class AITransformerWebSocketContent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("transcript", NullValueHandling = NullValueHandling.Ignore)]
        public string Transcript { get; set; }
        
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
    }
}