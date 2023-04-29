using System;
using System.Collections.Generic;
using AIAgent.Language;
using AICore.Infrastructure.NoSpoonAI.Types.Request;
using AICore.Services.Types.Request;
using Newtonsoft.Json;

namespace AIAgent.Types.Data.Prompts
{
    public class AgentFunctionRequestPrompt : NoSpoonAITransformerPromptRequest
    {
        #region Properties

        private string _agentName = string.Empty;
        private List<DataMessage> _messages;
        private IAgentLanguage _language;

        #endregion
        
        #region Initialization

        public AgentFunctionRequestPrompt(List<DataMessage> messages, string agentName, IAgentLanguage language) : base()
        {
            _agentName = agentName;
            _messages = messages;
            _language = language;
        }

        public override void OnBeginInitialize()
        {
            RetryDelay(125);
            RetryLimit(10);
            MaxTokens(AgentConstants.MAX_TOKENS_ON_COMPLETION);
            
            //Setup the function mode
            FunctionMode(new FunctionDetail{ name = "send_message_to_user"});
            Functions(new List<Function>
            {
                new Function
                {
                    name = _language.AgentFunction().name,
                    description = _language.AgentFunction().description,
                    parameters = new FunctionParameter
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
            });
            
            //Setup the messages
            Messages(_messages);
        }

        protected override T OnSuccessFunction<T>(string value)
        {
            try
            {
                return base.OnSuccessFunction<T>(value);
            }
            catch
            {
                if (value.Contains("\"message\": \"")) return new AgentResponse{ message = string.Empty, emotion = string.Empty} as T;
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