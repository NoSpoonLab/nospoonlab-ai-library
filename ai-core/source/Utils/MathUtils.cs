using System;
using System.Linq;

namespace AICore.Utils
{
    /// <summary>
    /// Contains a collection of mathematical utility functions aimed at AI calculations.
    /// </summary>
    public class MathUtils
    {
        /// <summary>
        /// Calculates Cosine Similarity between two vectors.
        /// </summary>
        /// <param name="vector1">The first input vector.</param>
        /// <param name="vector2">The second input vector.</param>
        /// <returns>The Cosine Similarity between vector1 and vector2.</returns>
        /// <exception cref="ArgumentException">Thrown when vectors have different lengths.</exception>
        public static float CalculateCosineSimilarity(float[] vector1, float[] vector2)
        {
            if (vector1.Length != vector2.Length)
            {
                throw new ArgumentException("Vectors must have same length. " + "Vector 1: " + vector1.Length + " Vector 2: " + vector2.Length);
            }
            float dotProduct = vector1.Zip(vector2, (x, y) => x * y).Sum();
            float magnitude1 = (float)Math.Sqrt(vector1.Select(x => (double)x * x).Sum());
            float magnitude2 = (float)Math.Sqrt(vector2.Select(x => (double)x * x).Sum());
            float cosineSimilarity = dotProduct / (magnitude1 * magnitude2);
            return Math.Max(0, cosineSimilarity);
        }

        /// <summary>
        /// Calculates the Decay Rate of a value with a maxMatchTime of 1 hour.
        /// </summary>
        /// <returns>The decay rate value of the logarithm of the minimum threshold (0.01) divided by the maxMatchTime.</returns>
        public static double DecayRate()
        {
            var maxMatchTime = 1.0; // 1 hour
            return Math.Pow(10, Math.Log10(0.01) / maxMatchTime);
        }

        /// <summary>
        /// Normalizes the given value using a maximum range.
        /// </summary>
        /// <param name="x">The value to be normalized.</param>
        /// <param name="max">The maximum range of the normalization calculation</param>
        /// <returns>The result of normalizing the value against the maximum range.</returns>
        public static float Normalize(float x, float max)
        {
            return (x) / max;
        }
    }
}