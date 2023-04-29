using System;
using System.Collections.Generic;

namespace AICore.Services.Types.Response
{
    /// <summary>
    /// Represents the response from the AI Image service.
    /// This response includes timestamps and a list of successfully processed ImageUrls.
    /// </summary>
    [Serializable]
    public class AIImageResponse
    {
        /// <summary>
        /// The time at which the AI Image service completed processing.
        /// This is represented as a UNIX timestamp.
        /// </summary>
        public long created;
        
        /// <summary>
        /// The list of ImageUrls that have been processed by the AI Image service.
        /// </summary>
        public List<ImageUrl> data;
    }

    /// <summary>
    /// Represents a single image as a URL.
    /// This is used in the list of processed images within the AIImageResponse.
    /// </summary>
    [Serializable]
    public class ImageUrl
    {
        /// <summary>
        /// The URL of the processed image.
        /// </summary>
        public string url;
    }
}