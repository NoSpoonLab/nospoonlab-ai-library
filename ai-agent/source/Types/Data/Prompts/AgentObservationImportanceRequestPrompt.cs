using System;
using System.Linq;
using System.Text.RegularExpressions;
using AIAgent.Types.Data.AIAgent.Types.Data;
using AICore.Infrastructure.NoSpoonAI.Types.Data;
using AICore.Infrastructure.NoSpoonAI.Types.Request;

namespace AIAgent.Types.Data.Prompts
{
    public class AgentObservationImportanceRequestPrompt : NoSpoonAITransformerPromptRequest
    {
        #region Initialization

        public AgentObservationImportanceRequestPrompt(string userPrompt, string systemPrompt, NoSpoonAIExampleData exampleData) : 
            base(userPrompt, systemPrompt, exampleData){}

        public override void OnBeginInitialize()
        {
            MaxTokens(12);
            Temperature(0.2f);
            TopP(0.1f);
            RetryDelay(150);
            RetryLimit(6);
            SetJsonModeFormat(true);
        }
        
        #endregion
        
        #region Methods
        
        protected override T OnSuccessMessage<T>(string value)
        {
            try
            {
                return base.OnSuccessMessage<T>(value);
            }
            catch
            {
                Console.WriteLine("Conversion Failed, with the data: " + value);
                var regex = new Regex(@"\d+");
                var matches = regex.Matches(value);
                var numbers = (from Match match in matches select int.Parse(match.Value)).ToList();
                if(numbers.Count == 2 || numbers.Count > 3) throw new Exception("Invalid number of numbers found, after trying to convert the data to AgentMemoryImportance");
                if(numbers.Count == 1) return new AgentMemoryImportance { score = numbers.First() } as T;
                try
                {
                    return new AgentMemoryImportance { score = numbers.First(it => it != 1 && it != 9 && it != 10 && it != 0) } as T;
                }
                catch
                {
                    return new AgentMemoryImportance { score = 0 } as T;
                }
            }
        }

        #endregion
    }
}