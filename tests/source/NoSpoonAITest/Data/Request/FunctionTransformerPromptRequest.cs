using System.Collections.Generic;
using AICore.Infrastructure.NoSpoonAI.Types.Request;
using AICore.Services.Types.Request;
using Newtonsoft.Json;

namespace Tests.NoSpoonAITest.Data.Request
{
    public class FunctionTransformerPromptRequest : NoSpoonAITransformerPromptRequest
    {
        #region Initialization
        
        public FunctionTransformerPromptRequest(string userPrompt) : base(userPrompt) {}

        public override void OnBeginInitialize()
        {
            RetryDelay(125);
            RetryLimit(10);
            
            //Setup the function mode
            FunctionMode("auto");
            FunctionMode(new FunctionDetail{ name = "extract_current_weather"});
            Functions(new List<Function>
            {
                new Function
                {
                    name = "extract_current_weather",
                    description = "extract current weather from message",
                    parameters = new FunctionParameter
                    {
                        type = "object",
                        required = new List<string> { "message" },
                        properties = new WeatherFunctionProperties
                        {
                            message = new FunctionProperty
                            {
                                type = "string",
                                description = "Current weather information extracted from the user message",
                                enumerator = new List<string>
                                {
                                    "Sunny", "Cloudy", "Rainy", "Snowy", "Windy", "Foggy", "Stormy", " Hailstorm",
                                    "Tornado", "Hot"
                                }
                            }
                        }
                    }
                },
                new Function
                {
                    name = "extract_importance",
                    description = "extract the importance from message",
                    parameters = new FunctionParameter
                    {
                        type = "object",
                        required = new List<string> { "color" },
                        properties = new ClassifyFunctionProperties
                        {
                            color = new FunctionProperty
                            {
                                type = "string",
                                description = "Current color of the car extracted from the user message",
                                enumerator = new List<string>
                                {
                                    "Red", "Blue", "Yellow", "Orange", "Purple", "Pink"
                                }
                            }
                        }
                    }
                }
            });
        }

        #endregion

        #region Functions Properties

        public class WeatherFunctionProperties
        {
            public FunctionProperty message{ get; set; }
        
        }
        
        public class WeatherResponse
        {
            public string message{ get; set; }
        }
    
        public class FunctionProperty
        {
            public string type{ get; set; }
            public string description{ get; set; }
        
            [JsonProperty("enum")]
            public List<string> enumerator { get; set; }
        }
        
        public class ClassifyFunctionProperties
        {
            public FunctionProperty color{ get; set; }
        }

        public class ClassifyResponse
        {
            public string color{ get; set; }
        }

        #endregion
    }
}