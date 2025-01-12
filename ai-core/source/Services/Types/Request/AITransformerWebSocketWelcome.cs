using Newtonsoft.Json;

namespace AICore.Services.Types.Request
{

    public class AITransformerWebSocketWelcome : WebsocketBase
    {
        [JsonProperty("response")]
        public AITransformerWebSocketResponse Response { get; set; }
    }
}