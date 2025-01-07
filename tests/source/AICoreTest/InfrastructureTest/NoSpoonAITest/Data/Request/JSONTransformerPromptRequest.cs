using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AICore.Infrastructure.NoSpoonAI.Types.Data;
using AICore.Infrastructure.NoSpoonAI.Types.Request;
using Tests.AICoreTest.InfrastructureTest.NoSpoonAITest.Data.Response;

namespace Tests.AICoreTest.InfrastructureTest.NoSpoonAITest.Data.Request
{
    public class JsonTransformerPromptRequest : NoSpoonAITransformerPromptRequest
    {
        
        #region Initialization
      
        public JsonTransformerPromptRequest(List<string> userPrompts) : base(userPrompts){}
        public JsonTransformerPromptRequest(string userPrompt) : base(userPrompt){}
        public override void OnBeginInitialize()
        {
            //System prompts
            var prompt = "Your job as an assistant is to score sentences and answer " +
                         "only in RFC8259 compliant JSON format without any additional data.\n\n" +
                         "On the rating scale it goes from 1 to 9, where 1 is " +
                         "purely mundane (e.g., brushing teeth, making the bed) and " +
                         "9 is extremely touching (e.g., a breakup, college " +
                         "acceptance, a dismissal from work).\n";
            prompt += "Output format:\n" + "{\n" + "  \"score\":9 \n" + "}\n";
            prompt += "Rate the poignant probability of the following sentence:";
            SystemPrompts(new List<string> { prompt });
            
            //Example prompts
            ExamplePrompt(new NoSpoonAIExampleData
            {
                Input = "Being fired from work",
                Output = "{\n" + "  \"score\":9 \n" + "}"
            });
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
                if(numbers.Count == 1) return new JSONTransformerResult { score = numbers.First() } as T;
                try
                {
                    return new JSONTransformerResult { score = numbers.First(it => it != 1 && it != 9 && it != 10 && it != 0) } as T;
                }
                catch
                {
                    return new JSONTransformerResult { score = 0 } as T;
                }
            }
        }

        #endregion
    }
}