﻿using System;
using System.Threading.Tasks;
using AICore.Infrastructure.GoogleAI;
using NUnit.Framework;

namespace Tests.AICoreTest.InfrastructureTest.GoogleAITest
{
    public class GoogleAITest
    {
        
        [Test]
        public async Task TextToSpeechTest()
        {
            //Test Embedding Request
            Console.WriteLine("Starting test of text to speech request\n");
            var client = new GoogleAIClient();
            /*
            try
            {
                var request = await client.SendTransformerRequest(new AITransformerRequest{ messages = new List<DataMessage>{ new DataMessage{ content = "Hola Maquina, espero que estes muy bien.", role = AIRole.User}}});
                Assert.IsNotNull(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            */
        }
    }
}