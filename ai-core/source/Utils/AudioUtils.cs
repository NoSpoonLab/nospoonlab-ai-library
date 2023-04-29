using UnityEngine;

namespace AICore.Utils
{
    /// <summary>
    /// Contains utility methods for processing audio data in AI core.
    /// </summary>
    public class AudioUtils
    {
        /// <summary>
        /// Converts a byte array to an AudioClip.
        /// </summary>
        /// <param name="data">The byte array containing the audio data, this includes the hz, channels, samples etc.</param>
        /// <returns>An AudioClip created from the given byte array. The AudioClip is named 'temp', and is created with the parameters given in the byte array.</returns>
        /// <exception cref="System.ArgumentException">Thrown when any of the BitConverter calls fail meaning the data array does not contain valid audio data.</exception>
        public static AudioClip BytesToAudioClip(byte[] data)
        {
            // Extract frequency(Hz).
            var hz = System.BitConverter.ToInt32(data, 24);
            
            // Extract number of audio channels.
            var channels = System.BitConverter.ToInt16(data, 22);
            
            // Extract number of samples.
            var samples = System.BitConverter.ToInt32(data, 40) / 2;
            
            // Initialize a float array to hold the audio data.
            var audioData = new float[samples];

            // Convert byte data to float and populate audioData array.
            for (var i = 0; i < samples; i++)
            {
                audioData[i] = System.BitConverter.ToInt16(data, i * 2 + 44) / 32768f;
            }

            // Create AudioClip object.
            var clip = AudioClip.Create("temp", samples, channels, hz, false);
            
            // Set AudioClip data.
            clip.SetData(audioData, 0);

            // Return AudioClip.
            return clip;
        }
    }
}