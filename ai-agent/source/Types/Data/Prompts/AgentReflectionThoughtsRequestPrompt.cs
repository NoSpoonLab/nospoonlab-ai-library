using AICore.Infrastructure.NoSpoonAI.Types.Data;
using AICore.Infrastructure.NoSpoonAI.Types.Request;

namespace AIAgent.Types.Data.Prompts
{
    public class AgentReflectionThoughtsRequestPrompt : NoSpoonAITransformerPromptRequest
    {
        #region Initialization

        public AgentReflectionThoughtsRequestPrompt(string userPrompt, string systemPrompt, NoSpoonAIExampleData exampleData) : 
            base(userPrompt, systemPrompt, exampleData){}
        public override void OnBeginInitialize()
        {
            MaxTokens(256);
            Temperature(0.2f);
            TopP(0.1f);
            RetryDelay(150);
            RetryLimit(10);
        }
        
        #endregion
    }
}