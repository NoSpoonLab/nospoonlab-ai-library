using System.Collections.Generic;
using Newtonsoft.Json;

namespace AICore.Services.Types.Response
{
    public class AITransformerWebSocketResponseCreated : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("response")]
        public AITransformerWebSocketResponseR Response { get; set; }
    }
    
    public class AITransformerWebSocketResponseDone : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("response")]
        public AITransformerWebSocketResponseR Response { get; set; }
    }
    
    public class AITransformerWebSocketResponseOutputItemAdded : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("response_id")]
        public string ResponseId { get; set; }

        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }

        [JsonProperty("item")]
        public AITransformerWebSocketItem Item { get; set; }
    }
    
    public class AITransformerWebSocketResponseOutputItemDone : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("response_id")]
        public string ResponseId { get; set; }

        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }

        [JsonProperty("item")]
        public AITransformerWebSocketItem Item { get; set; }
    }
    
    public class AITransformerWebSocketResponseContentPartDone : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("response_id")]
        public string ResponseId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }

        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }

        [JsonProperty("part")]
        public AITransformerWebSocketPart Part { get; set; }
    }
    
    public class AITransformerWebSocketResponseContentPartAdded : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("response_id")]
        public string ResponseId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }

        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }

        [JsonProperty("part")]
        public AITransformerWebSocketPart Part { get; set; }
    }
    
    public class AITransformerWebSocketResponseTextDelta : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("response_id")]
        public string ResponseId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }

        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }

        [JsonProperty("delta")]
        public string Delta { get; set; }
    }
    
    public class AITransformerWebSocketResponseTextDone : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("response_id")]
        public string ResponseId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }

        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
    
    public class AITransformerWebSocketResponseAudioTranscriptDelta : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("response_id")]
        public string ResponseId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }

        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }

        [JsonProperty("delta")]
        public string Delta { get; set; }
    }
    
    public class AITransformerWebSocketResponseAudioTranscriptDone : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("response_id")]
        public string ResponseId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }

        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }

        [JsonProperty("transcript")]
        public string Transcript { get; set; }
    }
    
    public class AITransformerWebSocketResponseAudioDelta : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("response_id")]
        public string ResponseId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }

        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }

        [JsonProperty("delta")]
        public string Delta { get; set; }
    }
    
    public class AITransformerWebSocketResponseAudioDone : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("response_id")]
        public string ResponseId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }

        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }
    }
    
    public class AITransformerWebSocketResponseFunctionCallArgumentsDelta : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("response_id")]
        public string ResponseId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }

        [JsonProperty("call_id")]
        public string CallId { get; set; }

        [JsonProperty("delta")]
        public string Delta { get; set; }
    }
    
    public class AITransformerWebSocketResponseFunctionCallArgumentsDone : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("response_id")]
        public string ResponseId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }

        [JsonProperty("call_id")]
        public string CallId { get; set; }

        [JsonProperty("arguments")]
        public string Arguments { get; set; }
    }

    public class AITransformerWebSocketRateLimitsUpdated : WebsocketBase
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("rate_limits")]
        public List<AITransformerWebSocketRateLimit> RateLimits { get; set; }
    }
    
    public class AITransformerWebSocketRateLimit
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("remaining")]
        public int Remaining { get; set; }

        [JsonProperty("reset_seconds")]
        public int ResetSeconds { get; set; }
    }

    public class AITransformerWebSocketPart
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class AITransformerWebSocketResponseR
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("status_details")]
        public object StatusDetails { get; set; }

        [JsonProperty("output")]
        public List<object> Output { get; set; }

        [JsonProperty("usage")]
        public object Usage { get; set; }
    }

    public class AITransformerWebSocketUsage
    {
        [JsonProperty("total_tokens")]
        public int TotalTokens { get; set; }

        [JsonProperty("input_tokens")]
        public int InputTokens { get; set; }

        [JsonProperty("output_tokens")]
        public int OutputTokens { get; set; }
    }
}