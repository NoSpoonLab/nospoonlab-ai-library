using System.Collections.Generic;
using AICore.Infrastructure.NoSpoonAI.Types.Request;

namespace Tests.NoSpoonAITest.Data.Request
{
    public class SimpleTransformerPromptRequest : NoSpoonAITransformerPromptRequest
    {

        #region Initialization

        public SimpleTransformerPromptRequest(List<string> userPrompts) : base(userPrompts){}
        public SimpleTransformerPromptRequest(string userPrompt) : base(userPrompt){}
        public override void OnBeginInitialize() {}

        #endregion
    }
}