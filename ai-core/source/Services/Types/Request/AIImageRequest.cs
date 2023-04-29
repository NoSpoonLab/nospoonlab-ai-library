namespace AICore.Services.Types.Request
{
    /// <summary>
    /// Represents a request for generating AI images based on a given prompt.
    /// Users can specify the number of images and the size of the output images.
    /// </summary>
    public class AIImageRequest
    {
        /// <summary>
        /// The textual prompt that will be used as the basis for generating the image.
        /// This can be any form of descriptive text or keywords.
        /// </summary>
        public string prompt;

        /// <summary>
        /// The number of images to be returned in the response.
        /// This determines how many different images will be generated based on the input prompt.
        /// </summary>
        public int n = 1;

        /// <summary>
        /// The size of the output images in the format "width x height" (e.g., "256x256").
        /// The resulting images will conform to this size specification.
        /// </summary>
        public string size = "256x256";

        /// <summary>
        /// Constructs a new instance of <see cref="AIImageRequest"/> with a specified prompt, number of images, and size of images.
        /// </summary>
        /// <param name="prompt">The textual prompt for generating images.</param>
        /// <param name="n">The number of images to be generated.</param>
        /// <param name="size">The size of the output images.</param>
        public AIImageRequest(string prompt, int n, string size)
        {
            this.prompt = prompt;
            this.n = n;
            this.size = size;
        }
    
        /// <summary>
        /// Constructs a new instance of <see cref="AIImageRequest"/> with a specified prompt.
        /// </summary>
        /// <param name="prompt">The textual prompt for generating images.</param>
        public AIImageRequest(string prompt)
        {
            this.prompt = prompt;
        }
    }
}