using System.Threading.Tasks;
using AICore.Services.Types.Data;
using AICore.Services.Types.Request;
using AICore.Services.Types.Response;

namespace AICore.Services.Interfaces
{
    public interface IEmbeddingService : IService
    {
        Task<AIEmbeddingResponse> SendEmbeddingRequest(AIEmbeddingRequest value, AIModel model);
    }
}