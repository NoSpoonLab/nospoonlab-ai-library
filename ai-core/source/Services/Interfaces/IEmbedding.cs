using System.Threading.Tasks;
using AICore.Services.Types.Request;
using AICore.Services.Types.Response;

namespace AICore.Services.Interfaces
{
    public interface IEmbedding
    {
        /// <summary>
        /// Asynchronously sends an embedding request, transform a string into a embedding vector of floats.
        /// </summary>
        /// <param name="value">The AIEmbeddingRequest carrying the data required for the embedding request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an AIEmbeddingResponse with the response details.</returns>
        Task<AIEmbeddingResponse> SendEmbeddingRequest(AIEmbeddingRequest value);
    }
}