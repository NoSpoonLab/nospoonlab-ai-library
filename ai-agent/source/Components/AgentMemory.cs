using System;
using System.Collections.Generic;
using System.Linq;
using AICore.Utils;
using UnityEngine;

namespace AIAgent
{
    /// <summary>
    /// This class represents the memory module of an AI agent.
    /// The memory holds information on data, its importance and other relevant characteristics linked to the usage and relevance of the data.
    /// As a memory we can have events, observation, conversation, thoughts or any other data that the AI agent needs to store for a later use.
    /// </summary>
    public class AgentMemory
    {
        #region Properties
        
        /// <summary>
        /// Unique identifier for a specific part of memory.
        /// </summary>
        public int Id;

        /// <summary>
        /// Data stored in the memory.
        /// Events, observation, conversation, thoughts or any other data that the AI agent needs to store.
        /// </summary>
        public string Data;

        /// <summary>
        /// Token count will return the number of tokens in the data property string using a tokenizer utility.
        /// </summary>
        public int TokenCount => TokenUtils.Tokenizer(Data);

        /// <summary>
        /// Measure of the importance of the data in memory.
        /// Calculated based on the importance of sentence, the recency of the memory and last visited date.
        /// </summary>
        public float Importance;

        /// <summary>
        /// An array representing the data's location in a multi-dimensional space.
        /// </summary>
        public float[] Embedding;

        /// <summary>
        /// Date/time when the stored data was last accessed.
        /// </summary>
        public DateTime LastVisitedDate;

        /// <summary>
        /// Measure of how recent the data has been used or visited.
        /// </summary>
        public float RecencyScore;

        /// <summary>
        /// Normalized measure of the importance of the data.
        /// </summary>
        public float ImportanceScore;

        /// <summary>
        /// Measure of the relevance of the data to other current observations.
        /// </summary>
        public float RelevanceScore;

        /// <summary>
        /// Cumulative total score representing a measure of the overall utility of the stored data based on its recency, importance and relevance scores.
        /// </summary>
        public float TotalScore;
        public float KeywordEmbeddingsScore;
        
        public Dictionary<string,float[]> keywordEmbeddings;

        #endregion

        #region Methods
        
        /// <summary>
        /// Calculates the recency score of the stored memory based on the hours that have passed since the memory was last accessed.
        /// </summary>
        private void CalculateRecencyScore()
        {
            var timeSinceLastVisit = (DateTime.Now - LastVisitedDate).TotalHours;
            var recencyScore =  Math.Pow(MathUtils.DecayRate(), timeSinceLastVisit);
            RecencyScore = (float)(recencyScore);
        }

        /// <summary>
        /// Calculates and sets the importance score of the stored memory, normalizing the importance to a scale of 0 to 10.
        /// </summary>
        private void CalculateImportanceScore()
        {
            ImportanceScore = MathUtils.Normalize(Importance, 10);
        }

        /// <summary>
        /// Calculates the relevance score of stored memory based on similarity of the stored memory's embedding and provided observation embedding.
        /// </summary>
        /// <param name="observationEmbedding"></param>   
        private void CalculateRelevanceScore(float[] observationEmbedding)
        {
            RelevanceScore = MathUtils.CalculateCosineSimilarity(observationEmbedding, Embedding);
        }
        
        private void CalculateKeywordEmbeddingsScore(float[] observationKeywordEmbedding)
        {
            //Compare the two dictionaries and calculate the cosine similarity between the two
            //Then add the cosine similarity to the relevance score
            /*
             * foreach (var keyword in keywordEmbeddings)
            {
                var keywordEmbeddingsScoreLocal = observationKeywordEmbedding.Sum(observationKeyword => MathUtils.CalculateCosineSimilarity(keyword.Value, observationKeyword.Value));
                keywordEmbeddingsScoreLocal /= observationKeywordEmbedding.Count;
                KeywordEmbeddingsScore += keywordEmbeddingsScoreLocal;
            }
            KeywordEmbeddingsScore /= keywordEmbeddings.Count;
             */
            /*foreach (var keyword in keywordEmbeddings)
            {
                Debug.Log("Key: " + keyword.Key + " Value: " + keyword.Value + " ObservationKeywordEmbedding: " + observationKeywordEmbedding + " Cosine Similarity: " + MathUtils.CalculateCosineSimilarity(keyword.Value, observationKeywordEmbedding));
                KeywordEmbeddingsScore += MathUtils.CalculateCosineSimilarity(keyword.Value, observationKeywordEmbedding);
            }
            KeywordEmbeddingsScore /= keywordEmbeddings.Count;
            Debug.Log("Relevance Score: " + RelevanceScore + " Keyword Embeddings Score: " + KeywordEmbeddingsScore);*/
        }

        #endregion

        #region Getters
        
        /// <summary>
        /// Calculates and returns the total score for stored data, summing up its recency, importance, and relevance scores.
        /// </summary>
        /// <param name="observationEmbedding">The embedding values for a current observation to compare to the stored data. This is used in calculating the relevance score.</param>
        /// <returns>The total score for the stored data.</returns>
        public float CalculateTotalScore(float[] observationEmbedding)
        {
            CalculateRecencyScore();
            CalculateImportanceScore();
            CalculateRelevanceScore(observationEmbedding);
            CalculateKeywordEmbeddingsScore(observationEmbedding);
            TotalScore = RecencyScore + ImportanceScore + RelevanceScore + KeywordEmbeddingsScore;
            return TotalScore;
        }

        #endregion
    }
}
