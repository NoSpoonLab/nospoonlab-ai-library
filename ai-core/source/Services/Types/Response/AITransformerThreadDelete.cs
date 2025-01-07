using Newtonsoft.Json;

namespace AICore.Services.Types.Response
{
    public class AITransformerThreadDelete
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("object")]
        public string Object { get; set; }
        
        [JsonProperty("deleted")]
        public bool Deleted { get; set; }
    }
}