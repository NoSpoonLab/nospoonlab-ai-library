using System;
using System.Threading.Tasks;
using AICore.Infrastructure.Azure;
using AICore.Services.Interfaces;
using AICore.Services.Types.Data;
using NUnit.Framework;

namespace Tests.ServicesTest
{
    public class CloudServiceTest
    {
        #region Properties

        private ICloudService _cloudService;

        #endregion

        #region Initialization
        
        public CloudServiceTest()
        {
            //Inject Dependencies
            _cloudService = new AzureClient();
            _cloudService.SetSettings(new VoiceServiceInitData
            {
                APIKey = Environment.GetEnvironmentVariable("AZURE_API_KEY"),
                Region = "francecentral"
            });
        }
        #endregion
        
        [Test]
        public async Task TextToSpeechTest()
        {
            //Test Embedding Request
            Console.WriteLine("Starting test of text to speech request\n");
            /*
            try
            {
                var request = await _cloudService.SendTextToSpeechRequest("Hola Humano, espero que estes muy bien.", "es-ES-VeraNeural", "es-ES");
                Assert.IsNotNull(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }*/
        }
        
        [Test]
        public async Task TextToSpeechStyleTest()
        {
            //Test Embedding Request
            Console.WriteLine("Starting test of text to speech request\n");
            /*
            try
            {
                var request = await _cloudService.SendTextToSpeechRequest("Hi, i am sad i did not get pay today.", "en-US-JennyNeural", "en-US", "sad");
                Assert.IsNotNull(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }*/
        }
    }
}