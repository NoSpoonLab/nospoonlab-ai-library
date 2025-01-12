using Newtonsoft.Json;

namespace AICore.Services.Types
{
    public class WebsocketBase
    {
        [JsonProperty("type")]
        public string Type { get; protected set; }
    }
}