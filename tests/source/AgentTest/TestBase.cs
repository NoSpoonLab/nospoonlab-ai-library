using AICore.Infrastructure.NoSpoonAI;
using AICore.Infrastructure.OpenAI;
using AICore.Services.Interfaces;
using AICore.Services.Types.Data;
using DependencyInjectionCore;

namespace Tests.AgentTest
{
    
    public class TestBase
    {
    
        #region Initialization
        
        public TestBase()
        {
            //Inject Dependencies
            if(!DIContainer.Exists<IAIGPTService>()) DIContainer.Register<IAIGPTService, OpenAIClient>();
            if(!DIContainer.Exists<NoSpoonAIClient>()) DIContainer.Register<NoSpoonAIClient, NoSpoonAIClient>();
            DIContainer.Get<IAIGPTService>().SetSettings(new AIServiceInitData
            {
                APIKey = "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",
                Model = AIModel.gpt_35_16k,
                Type = AIServiceType.OpenAI
            });
            DIContainer.Get<NoSpoonAIClient>().Initialize();
        }
        #endregion
    }
}