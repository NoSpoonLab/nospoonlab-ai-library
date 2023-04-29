using System.Threading.Tasks;
using AICore.Services.Types.Request;
using AICore.Services.Types.Response;

namespace AICore.Services.Interfaces
{
    /// <summary>
    /// Defines the requirements for a service that processes AI Image requests utilizing deep learning algorithms and a large dataset of images and text,
    /// with an intent to generate high-quality images from textual descriptions.
    /// This interface is used to implement services that can be applied to various domains such as web design, artwork production, 
    /// virtual environment design, architectural and interior design.
    /// </summary>
    /// 
    /// <seealso cref="AICore.Services.Interfaces.IService"/>
    public interface IAIImageService : IService
    {
        /// <summary>
        /// Asynchronously sends an image request. The method takes detailed textual descriptions as input and produces realistic, 
        /// original images corresponding to the provided description.
        /// </summary>
        /// <param name="value">The AIImageRequest carrying the data required for the image generation request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an AIImageResponse with the generated image details.</returns>
        Task<AIImageResponse> SendImageRequest(AIImageRequest value);
    }
}