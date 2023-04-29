#pragma warning disable CS0649
using System;

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
        public FunctionContentInternal function_call;
    }


    [Serializable]
    internal class FunctionContentInternal
    {
        public string name;
        public string arguments;
    }
}

#pragma warning restore CS0649