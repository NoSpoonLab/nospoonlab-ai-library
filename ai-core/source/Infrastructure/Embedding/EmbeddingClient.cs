using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AICore.Services.Interfaces;
using AICore.Services.Types.Data;
using AICore.Services.Types.Request;
using AICore.Services.Types.Response;
using AICore.Utils;

namespace AICore.Infrastructure.Embedding
{
    public class EmbeddingClient : IEmbeddingService
    {
        /*
        public class ModelInput
        {
            public int[] InputIds { get; set; }
            public int[] AttentionMask { get; set; }
            public int[] TokenTypeIds { get; set; }
        }
        
        #region Variables
        
        private Model _runtimeModel = null;
        
        #endregion
        
        #region Methods

        public Task<AIEmbeddingResponse> SendEmbeddingRequest(AIEmbeddingRequest value, AIModel model)
        {
            if (model == AIModel.embedding_ada_002)
            {
                var embedding = (IEmbedding)DIContainer.Get<IAIGPTService>();
                return embedding.SendEmbeddingRequest(value);
            }
            return SendLocalEmbeddingRequest(value, model);
        }
        
        public void LoadModel(AIModel type, ModelAsset  model)
        {
            if(_runtimeModel != null) throw new Exception("Model already loaded");
            if(type == AIModel.embedding_ada_002) throw new Exception("Model not supported for local load");
            if(model == null) throw new Exception("Model asset not provided"); 
            _runtimeModel = ModelLoader.Load(model);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task<AIEmbeddingResponse> SendLocalEmbeddingRequest(AIEmbeddingRequest value, AIModel model)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            //Debug.Log("Request Start at: " + DateTime.Now.ToString("HH:mm:ss.fff"));
            var runtimeWorker =  WorkerFactory.CreateWorker(BackendType.GPUCompute, _runtimeModel);
            var encodedText = TokenizerUtils.GetTokenizer(model).Encode(value.input.Length, value.input);
            encodedText = encodedText.FindAll(it => it.AttentionMask == 1);
            
            var embeddingInput = new ModelInput()
            {
                InputIds = encodedText.Select(it => (int) it.InputIds ).ToArray(),
                AttentionMask = encodedText.Select(it => (int) it.AttentionMask ).ToArray(),
                TokenTypeIds = encodedText.Select(it => (int) it.TokenTypeIds ).ToArray()
            };
            
            TensorShape shape = new TensorShape(1, encodedText.Count);
            Tensor inputIds = new TensorInt(shape, embeddingInput.InputIds);
            Tensor attentionMask = new TensorInt(shape, embeddingInput.AttentionMask);
            Tensor tokenTypeIds = new TensorInt(shape, embeddingInput.TokenTypeIds);
            
            var inputTensorsToGetEmbedding = new Dictionary<string, Tensor>() {
                { "input_ids", inputIds },
                { "attention_mask", attentionMask },
                { "token_type_ids", tokenTypeIds }
            };
            
            runtimeWorker.Execute(inputTensorsToGetEmbedding);
            
            var outputTensorFromEmbedding = runtimeWorker.PeekOutput() as TensorFloat;
            outputTensorFromEmbedding?.MakeReadable();
            var results = outputTensorFromEmbedding?.ToReadOnlyArray();
            var maxSample = outputTensorFromEmbedding.shape.ToArray()[2];
            /*var inputTensorToGetGlobalAverage = new TensorFloat(new TensorShape(1, 150, encodedText.Count), results);
            var tensorCachingAllocator = new TensorCachingAllocator();

            // Create an IOps object. The object uses Sentis compute shaders to do operations on the GPU.
            var globalAverageOps = WorkerFactory.CreateOps(BackendType.GPUCompute, tensorCachingAllocator);

            // Run the ArgMax operator on the input tensor.
            var outputTensorFromGlobalAverage = globalAverageOps.GlobalAveragePool(inputTensorToGetGlobalAverage);
            outputTensorFromGlobalAverage.MakeReadable();
            results = outputTensorFromGlobalAverage.ToReadOnlyArray();
            
            tensorCachingAllocator.Dispose();
            globalAverageOps.Dispose();
            inputTensorToGetGlobalAverage.Dispose();
            outputTensorFromGlobalAverage.Dispose();*/
            /*
            inputIds.Dispose();
            attentionMask.Dispose();
            tokenTypeIds.Dispose();
            
            inputTensorsToGetEmbedding["input_ids"].Dispose();
            inputTensorsToGetEmbedding["attention_mask"].Dispose();
            inputTensorsToGetEmbedding["token_type_ids"].Dispose();
            outputTensorFromEmbedding.Dispose();
            runtimeWorker.Dispose();
            //Debug.Log("Request End at: " + DateTime.Now.ToString("HH:mm:ss.fff"));
            return new AIEmbeddingResponse { data = new []{ new AIEmbeddingResponseData { embedding = results.Take(maxSample).ToArray() } } };
        }

        #endregion
            */
            public async Task<AIEmbeddingResponse> SendEmbeddingRequest(AIEmbeddingRequest value, AIModel model)
            {
                await Task.Delay(4);
                return new AIEmbeddingResponse();
            }
    }
}