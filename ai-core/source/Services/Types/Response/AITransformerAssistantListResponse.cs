using System.Collections.Generic;
using AICore.Services.Types.Response;
using Newtonsoft.Json;

namespace CleverAI.Models.Types.MM
{
    public class AITransformerAssistantListResponse
    {
        [JsonProperty("object")]
        public string Object { get; set; }
        
        [JsonProperty("data")]
        public List<AITransformerAssistantResponse> Data { get; set; }
        
        [JsonProperty("first_id")]
        public string FirstId { get; set; }
        
        [JsonProperty("last_id")]
        public string LastId { get; set; }
        
        [JsonProperty("has_more")]  
        public bool HasMore { get; set; }
    }
}