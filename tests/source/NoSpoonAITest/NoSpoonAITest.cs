using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AICore.Infrastructure.NoSpoonAI;
using AICore.Infrastructure.NoSpoonAI.Types.Data;
using AICore.Infrastructure.NoSpoonAI.Types.Request;
using AICore.Services.Types.Data;
using Newtonsoft.Json;
using NUnit.Framework;
using Tests.NoSpoonAITest.Data.Request;
using Tests.NoSpoonAITest.Data.Response;

namespace Tests.NoSpoonAITest
{
    public class NoSpoonAITest
    {
        #region Properties

        private NoSpoonAIClient _noSpoonAIClient;

        #endregion

        #region Initialization
        
        public NoSpoonAITest()
        {
            _noSpoonAIClient = new NoSpoonAIClient();
            _noSpoonAIClient.Initialize(); // This already initializes the client with the default settings with the OpenAI API key that is set in the environment variable
        }

        [SetUp]
        public void Setup()
        {
            
        }

        #endregion


        [Test]
        public async Task SimpleUsageOfTransformerRequest()
        {
            var noSpoonClient = new NoSpoonAIClient(); // Create a new instance of the NoSpoonAIClient
            var credentials = new AIServiceInitData // Create a new instance of the AIServiceInitData
            {
                APIKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY"), // here is where you set the API key
                Type = AIServiceType.OpenAI // here is where you set the type of service
            };
            noSpoonClient.Initialize(credentials); // Initialize the client with the credentials
            
            //Test Embedding Request
            Console.WriteLine("Starting test of simple transformer request\n");
            
            //Create the request object
            var request = new SimpleTransformerPromptRequest("Hello, how are you? and how are you today?");

            //Send the request
            var response = await noSpoonClient.SendTransformerRequest(request);
            
            //Print the response
            Console.WriteLine("Response: " + response);
            
            //Get the response
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task JsonSchema()
        {
            var request = new JSONSchemeTransformerPromptRequest(
                "Ordenador",
                "Crea una lista de las partes que componen un objeto",
                new NoSpoonAIExampleData
                {
                    Input = "Casa",
                    Output = JsonConvert.SerializeObject(new NoSpoonAIListData<string>()
                    {
                        data = new List<string>()
                        {
                            "Puerta",
                            "Ventana",
                            "Pared",
                            "Techo",
                            "Suelo"
                        }
                    })
                });
            
            // Send the reflection request to the AI API and return the result
            var remoteRequest = await _noSpoonAIClient.SendTransformerRequest<NoSpoonAIListData<string>, JSONSchemeTransformerPromptRequest>(request);
        }
        
        [Test]
        public async Task TestEmbeddingRequest()
        {
            //Test Embedding Request
            Console.WriteLine("Starting test of embedding request\n");
            
            //Create the request object
            var request = new NoSpoonAIEmbeddingRequest("Hello, my name is John. I am a human.");

            //Send the request
            var response = await _noSpoonAIClient.SendEmbeddingRequest<NoSpoonAIEmbeddingRequest>(request);
            
            //Get the response
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.data.First().embedding);
        }
        
        [Test]
        public async Task JSONTransformerRequestTest()
        {
            //Test Embedding Request
            Console.WriteLine("Starting test of JSON transformer request\n");
            
            //Create the request object
            var request = new JsonTransformerPromptRequest("Walk along the boardwalk");

            //Send the request
            var response = await _noSpoonAIClient.SendTransformerRequest<JSONTransformerResult, JsonTransformerPromptRequest>(request);
            
            //Print the response
            Console.WriteLine("Score: " + response.score);
            
            //Get the response
            Assert.IsTrue(response.score > -1.0f);
        }
        
        [Test]
        public async Task FunctionTransformerRequestTest()
        {
            //Test Embedding Request
            Console.WriteLine("Starting test of Function transformer request\n");
            
            //Create the request object
            var request = new FunctionTransformerPromptRequest("It looks like it's raining cats");

            //Send the request
            var response = await _noSpoonAIClient.SendTransformerRequest<string, FunctionTransformerPromptRequest>(request);

            //Print the response
            Console.WriteLine("Response: " + response);
            
            //new request
            request = new FunctionTransformerPromptRequest("My favorite car is my pink 1997 chevy, it still runs perfectly and gives me the best adventures on the road.");
            
            //Send a new the request
            response = await _noSpoonAIClient.SendTransformerRequest<string, FunctionTransformerPromptRequest>(request);
            
            //Print the response
            Console.WriteLine("Response: " + response);
            
            //Now create request with JSON transformation
            
            //Create the request object
            var weatherRequest = new FunctionTransformerPromptRequest("It seems that it is an excellent day, it does not rain a bit and there is a lot of sun.");

            //Send the request
            var weatherResponse = await _noSpoonAIClient.SendTransformerRequest<FunctionTransformerPromptRequest.WeatherResponse, FunctionTransformerPromptRequest>(weatherRequest);
            
            //Asset the result
            Assert.IsTrue(weatherResponse.message == "Sunny");

            //Create the request object
            var colorRequest = new FunctionTransformerPromptRequest("My dress is blue, it's my favorite color!.");

            //Send the request
            var colorResponse = await _noSpoonAIClient.SendTransformerRequest<FunctionTransformerPromptRequest.ClassifyResponse, FunctionTransformerPromptRequest>(colorRequest);
            
            //Asset the result
            Assert.IsTrue(colorResponse.color == "Blue");
        }
    }
}