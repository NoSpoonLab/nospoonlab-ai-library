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
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public List<FunctionInternal> functions  { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public object function_call;
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
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public FunctionCallInternal function_call { get; set; }
    }
    
    
    internal class FunctionCallInternal
    {
        public string name { get; set; }
        public object arguments { get; set; }
    }

    internal class FunctionInternal
    {
        public string name{ get; set; }
        public string description{ get; set; }
        public FunctionParameterInternal parameters{ get; set; }
    }

    internal class FunctionParameterInternal
    {
        public string type{ get; set; }
        public List<string> required { get; set; }
        public object properties{ get; set; }
    }
}
#pragma warning restore CS0649