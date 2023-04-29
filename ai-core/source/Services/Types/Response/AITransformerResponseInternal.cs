#pragma warning disable CS0649
using System;
using System.Collections.Generic;

namespace AICore.Services.Types.Response
{
    [Serializable]
    internal class AITransformerResponseInternal
    {
        public string id;
        public long created;
        public string model;
        public UsageDataInternal usage;
        public GeneratedDataInternal[] choices;
    }


    [Serializable]
    internal class UsageDataInternal
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }

    [Serializable]
    internal class GeneratedDataInternal
    {
        public DataContentInternal message;
        public string finish_reason;
    }

    [Serializable]
    internal class DataContentInternal
    {
        public string role;
        public string content;
        public List<ToolContentInternal> tool_calls;
    }
    
    [Serializable]
    internal class ToolContentInternal
    {
        public string id;
        public string type;
        public FunctionToolContentInternal function;
    }


    [Serializable]
    internal class FunctionToolContentInternal
    {
        public string name;
        public string arguments;
    }
}

#pragma warning restore CS0649