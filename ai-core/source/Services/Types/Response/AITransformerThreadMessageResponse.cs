using System.Collections.Generic;
using Newtonsoft.Json;

namespace AICore.Services.Types.Response
{
    public class AITransformerThreadMessageResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        [JsonProperty("assistant_id")]
        public string AssistantId { get; set; }

        [JsonProperty("thread_id")]
        public string ThreadId { get; set; }

        [JsonProperty("run_id")]
        public string RunId { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public List<AITransformerThreadContent> Content { get; set; } = new List<AITransformerThreadContent>();

        [JsonProperty("attachments")]
        public List<object> Attachments { get; set; } = new List<object>();

        [JsonProperty("metadata")]
        public Dictionary<string,string> Metadata { get; set; } = new Dictionary<string,string>();
        
        [JsonProperty("deleted")]
        public bool Deleted { get; set; }
    }
    
    public class AITransformerThreadTextContent
    {
        [JsonProperty("value")]
        public string value { get; set; }
        
        [JsonProperty("annotations")]
        public List<object> Annotations { get; set; } = new List<object>();
    }

    public class AITransformerThreadContent
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("text")]
        public AITransformerThreadTextContent Text { get; set; }
    }
}