using System;

namespace AICore.Utils
{
    /// <summary>
    /// TokenUtils class provides functionalities to perform tokenization operations such as calculating costs based on a string's characters and words.
    /// </summary>
    public class TokenUtils
    {
        /// <summary>
        /// Tokenizer method calculates the cost of a string value using a predetermined cost for characters and words.
        /// </summary>
        /// <param name="value">The string for which the cost needs to be calculated.</param>
        /// <returns>Returns an integer representing the cost of the string which is the greater of the two costs calculated based on characters and words in the input string.</returns>
        public static int Tokenizer(string value)
        {
            // Cost per character in a string
            var charCost = 0.25f;
            
            // Cost per word in a string
            var wordCost = 0.75f;
            
            // Count how many characters the input string has
            var charCount = value.Length;
            
            // Count how many words the input string has
            var wordCount = value.Split(' ').Length;
            
            // Calculate the cost of the input string based on characters and round up to the nearest integer
            var costByChar = (int) Math.Ceiling(charCount * charCost);
            
            // Calculate the cost of the input string based on words and round up to the nearest integer
            var costByWord = (int) Math.Ceiling(wordCount * wordCost);
            
            // Return the greater cost of the two calculated costs
            return Math.Max(costByChar, costByWord);
        }
    }
}