using System.Collections.Generic;
using AICore.Infrastructure.NoSpoonAI.Types.Request;
using AICore.Services.Types.Request;

namespace AIAgent.Types.Data.Prompts
{
    public class AgentPlanningRequestPrompt : NoSpoonAITransformerPromptRequest
    {
        
        #region Properties

        private string _agentName = string.Empty;

        #endregion
        
        public AgentPlanningRequestPrompt(List<DataMessage> messages, string agentName)
        {
            Messages(messages);
            _agentName = agentName;
        }
    }
}