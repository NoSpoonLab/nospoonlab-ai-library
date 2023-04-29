using System;
using AICore.Infrastructure.Embedding;
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
            if(!DIContainer.Exists<IEmbeddingService>()) DIContainer.Register<IEmbeddingService, EmbeddingClient>();
            if(!DIContainer.Exists<NoSpoonAIClient>()) DIContainer.Register<NoSpoonAIClient, NoSpoonAIClient>();
            DIContainer.Get<IAIGPTService>().SetSettings(new AIServiceInitData
            {
                APIKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY"),
                Type = AIServiceType.OpenAI
            });
            DIContainer.Get<NoSpoonAIClient>().Initialize();
        }
        #endregion
    }
}