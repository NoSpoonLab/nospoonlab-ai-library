using System.Collections.Generic;
using Newtonsoft.Json;

namespace AICore.Services.Types.Response
{
    public class AITransformerAssistantResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("object")]
        public string Object { get; set; }
        
        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("model")]
        public string Model { get; set; }
        
        [JsonProperty("instructions")]
        public string Instructions { get; set; }
        
        [JsonProperty("tools")]
        public List<AITransformerTool> Tools { get; set; }
        
        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; set; }
        
        [JsonProperty("top_p")]
        public double TopP { get; set; }
        
        [JsonProperty("temperature")]
        public double Temperature { get; set; }
        
        [JsonProperty("response_format")]
        public AITransformerResponseFormat ResponseFormat { get; set; }
    }
    
    public class AITransformerTool
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class AITransformerToolOutput
    {
        [JsonProperty("tool_call_id")]
        public string ToolCallId { get; set; }
        
        [JsonProperty("output")]
        public string Output { get; set; }
    }
    
    public class AITransformerResponseFormat
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}