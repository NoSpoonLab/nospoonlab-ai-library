using System;
using System.Collections.Generic;
using System.Linq;
using AIAgent.Language;
using AICore.Infrastructure.NoSpoonAI.Types.Request;
using AICore.Services.Types.Data;
using AICore.Services.Types.Request;
using AICore.Services.Types.Response;
using Newtonsoft.Json;
using UnityEngine;

namespace AIAgent.Types.Data.Prompts
{
    public class AgentFunctionRequestPrompt : NoSpoonAITransformerPromptRequest
    {
        #region Properties

        private string _agentName = string.Empty;
        private string _userName = string.Empty;
        private List<DataMessage> _messages;
        private IAgentLanguage _language;
        private int _agentVerbosity = 1;
        private int _agentCreativity = 1;

        #endregion
        
        #region Initialization

        public AgentFunctionRequestPrompt(List<DataMessage> messages, string agentName, IAgentLanguage language, int agentVerbosity, int agentCreativity) : base()
        {
            _agentName = agentName;
            _messages = messages;
            _language = language;
            _agentVerbosity = agentVerbosity;
            _agentCreativity = agentCreativity;
        }

        public override void OnBeginInitialize()
        {
            RetryDelay(125);
            RetryLimit(10);
            Model(AIModel.gpt);

            var maxTokens = (((_agentVerbosity / 10) * AgentConstants.NORMAL_TOKENS_ON_COMPLETION) + AgentConstants.NORMAL_TOKENS_ON_COMPLETION);
            maxTokens = maxTokens > AgentConstants.MAX_TOKENS_ON_COMPLETION ? AgentConstants.MAX_TOKENS_ON_COMPLETION : maxTokens;
            MaxTokens(maxTokens);
            
            //Request().temperature = 0.7f + (_agentCreativity / 20);
            Request().temperature = 1.0f;
            Request().top_p = 0.8f;
            
            //Setup the function mode
            ToolsMode(new ToolChoice{ function = new FunctionToolChoice{name = "send_message_to_user" }});
            Tools(new List<Tool>
            {
                new Tool
                {
                    function = new FunctionTool
                    {
                        name = _language.AgentFunction().name,
                        description = _language.AgentFunction().description,
                        parameters = new FunctionToolParameter
                        {
                            type = _language.AgentFunction().parameter.type,
                            required = new List<string>{"message", "emotion"},
                            properties = new AgentFunctionProperties
                            {
                                message = new MessageProperty
                                {
                                    type = _language.AgentFunction().parameter.properties.message.type,
                                    description = _language.AgentFunction().parameter.properties.message.description
                                },
                                emotion = new EmotionProperty
                                {
                                    type = _language.AgentFunction().parameter.properties.emotion.type,
                                    description = $"{_language.AgentFunction().parameter.properties.emotion.description} {_agentName}",
                                    enumerator =  _language.AgentFunction().parameter.properties.emotion.enumerator
                                }
                            }
                        }
                    }
                }
            });
            
            //Setup the messages
            Messages(_messages);
        }

        protected override List<T> OnSuccessFunctions<T>(List<NoSpoonToolContent> value) 
        {
            //Debug.Log("response: " + value);
            try
            {
                return base.OnSuccessFunctions<T>(value);
            }
            catch
            {
                var messages = value.Select(it => it.arguments).ToList();
                if (messages.Count == 0) throw new Exception("Failed to parse the response from the AI.");
                var anyContains = messages.FindAll(it => it.Contains("\"message\": \"") || it.Contains("message\":\"")).Count != 0;
                if (anyContains) return new List<T>{ new AgentResponse{ message = string.Empty, emotion = string.Empty} as T };
                throw new Exception("Failed to parse the response from the AI.");
            }
        }

        #endregion

        #region Functions Properties

        public class AgentFunctionProperties
        {
            public MessageProperty message{ get; set; }
            public EmotionProperty emotion{ get; set; }
        }
        
        public class MessageProperty
        {
            public string type{ get; set; }
            public string description{ get; set; }
        }
    
        public class EmotionProperty
        {
            public string type{ get; set; }
            public string description{ get; set; }
        
            [JsonProperty("enum")]
            public List<string> enumerator { get; set; }
        }

        #endregion
    }
}