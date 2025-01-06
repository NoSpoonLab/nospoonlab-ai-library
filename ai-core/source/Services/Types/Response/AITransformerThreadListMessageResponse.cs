using System.Collections.Generic;
using Newtonsoft.Json;

namespace AICore.Services.Types.Response
{
    public class AITransformerThreadListMessageResponse
    {
      [JsonProperty("object")]
      public string Object { get; set; }
      
      [JsonProperty("data")]
      public List<AITransformerThreadMessageResponse> Data { get; set; } = new List<AITransformerThreadMessageResponse>();
      
      [JsonProperty("first_id")]
      public string FirstId { get; set; }
      
      [JsonProperty("last_id")]
      public string LastId { get; set; }
      
      [JsonProperty("has_more")]
      public bool HasMore { get; set; }
    }
}