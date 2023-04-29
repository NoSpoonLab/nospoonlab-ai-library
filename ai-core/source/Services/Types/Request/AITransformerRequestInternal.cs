using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#pragma warning disable CS0649

namespace AICore.Services.Types.Request
{
    internal class AITransformerRequestInternal
    {
        public string model { get; set; }
        public List<AITransformerMessageInternal> messages { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public List<ToolInternal> tools  { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public object tool_choice;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ResponseFormatInternal response_format;
        public int max_tokens = Int32.MaxValue;
        public float temperature = 1.0f;
        public float top_p = 1.0f;
        public int n = 1;
        public bool stream = false;
        public float presence_penalty = 0.0f;
        public float frequency_penalty = 0.0f;
        public string user;
    }
  
    internal class AITransformerMessageInternal
    {
        public string role { get; set; }
        public string content { get; set; }
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)] public string name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public List<ToolChoiceInternal> tool_calls { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string tool_call_id = null;
    }
    
    
    internal class ToolChoiceInternal
    {
        public string id { get; set; }
        public string type { get; set; }
        public FunctionToolChoiceInternal function { get; set; }
    }
    
    internal class FunctionToolChoiceInternal
    {
        public string name { get; set; }
        public string arguments { get; set; }
    }
    internal class ToolInternal
    {
        public string type { get; set; }
        public FunctionToolInternal function { get; set; }
    }

    internal class FunctionToolInternal
    {
        public string name{ get; set; }
        public string description{ get; set; }
        public ToolParameterInternal parameters{ get; set; }
    }

    internal class ToolParameterInternal
    {
        public string type{ get; set; }
        public List<string> required { get; set; }
        public object properties{ get; set; }
    }
    
    internal class ResponseFormatInternal
    {
        public string type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public object json_schema { get; set; }
    }
}
#pragma warning restore CS0649