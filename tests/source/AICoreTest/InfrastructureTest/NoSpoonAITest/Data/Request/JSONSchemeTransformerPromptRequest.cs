﻿using System;
using AICore.Infrastructure.NoSpoonAI.Types.Data;
using AICore.Infrastructure.NoSpoonAI.Types.Request;

namespace Tests.AICoreTest.InfrastructureTest.NoSpoonAITest.Data.Request
{
    public class JSONSchemeTransformerPromptRequest : NoSpoonAITransformerPromptRequest
    {
        #region Initialization

        public JSONSchemeTransformerPromptRequest(string userPrompt, string systemPrompt, NoSpoonAIExampleData exampleData) : 
            base(userPrompt, systemPrompt, exampleData){}
        public override void OnBeginInitialize()
        {
            MaxTokens(256);
            Temperature(0.2f);
            TopP(0.1f);
            RetryDelay(150);
            RetryLimit(10);
            SetStructuredOutputFormat<NoSpoonAIListData<string>>(true);
        }

        protected override T OnSuccessMessage<T>(string value)
        {
            try
            {
                var result = base.OnSuccessMessage<T>(value);
                if(result == null) throw new Exception("Result is null");
                return result;
            }
            catch (Exception e)
            {
                return new NoSpoonAIListData<string>() as T;
            }
        }
        
        #endregion
    }
}