using Newtonsoft.Json;

namespace AICore.Services.Types.Response
{
    public class AITransformerWebSocketConversationItemInputAudioTranscriptionCompleted : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }

        [JsonProperty("transcript")]
        public string Transcript { get; set; }
    }
    
    public class AITransformerWebSocketConversationItemInputAudioTranscriptionFailed : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }

        [JsonProperty("error")]
        public AITransformerWebSocketError Error { get; set; }
    }
    
    public class AITransformerWebSocketInputAudioBufferCommitted : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("previous_item_id")]
        public string PreviousItemId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }
    }
    
    public class AITransformerWebSocketInputAudioBufferCleared : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }
    }
    
    public class AITransformerWebSocketInputAudioBufferSpeechStarted : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("audio_start_ms")]
        public int AudioStartMs { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }
    }
    
    public class AITransformerWebSocketInputAudioBufferSpeechStopped : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("audio_end_ms")]
        public int AudioEndMs { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }
    }
}