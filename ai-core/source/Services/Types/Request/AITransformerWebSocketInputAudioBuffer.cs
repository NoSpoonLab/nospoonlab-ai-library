using AICore.Infrastructure.OpenAI;
using AICore.Infrastructure.OpenAI.Data;
using Newtonsoft.Json;

namespace AICore.Services.Types.Request
{
    public class AITransformerWebSocketInputAudioBufferAppend : WebsocketBase
    {
        public AITransformerWebSocketInputAudioBufferAppend()
        {
            Type = OpenAIClient.WebSocketClientEventType.InputAudioBufferAppend.ConvertToString();
        }

        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("audio")]
        public string Audio { get; set; }
    }

    public class AITransformerWebSocketInputAudioBufferCommit : WebsocketBase
    {
        public AITransformerWebSocketInputAudioBufferCommit()
        {
            Type = OpenAIClient.WebSocketClientEventType.InputAudioBufferCommit.ConvertToString();
        }

        [JsonProperty("event_id")]
        public string EventId { get; set; }
    }

    public class AITransformerWebSocketInputAudioBufferClear : WebsocketBase
    {
        public AITransformerWebSocketInputAudioBufferClear()
        {
            Type = OpenAIClient.WebSocketClientEventType.InputAudioBufferClear.ConvertToString();
        }

        [JsonProperty("event_id")]
        public string EventId { get; set; }
    }
}