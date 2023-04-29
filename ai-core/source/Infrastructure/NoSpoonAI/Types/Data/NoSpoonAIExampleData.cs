namespace AICore.Infrastructure.NoSpoonAI.Types.Data
{
    /// <summary>
    /// This class holds the data for the examples which will be used in model prompts.
    /// These examples provide the model with some contextual understanding of how it should manage its responses.
    /// </summary>
    /// <example>
    /// <code>
    /// Example: 
    /// Assuming the instruction is "Classify the significance of a statement."
    /// The input value is "I dropped my ice cream on the ground."
    /// The output value is { "score": 1 }
    /// </code>
    /// This is how we make sure you know the format in which we expect responses.
    /// </example>
    public class NoSpoonAIExampleData
    {
        /// <summary>
        /// Gets or sets the input data - the statement or task to be classified or performed by the AI model.
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// Gets or sets the output data - the outcome or result of the classification or task performed by the AI model.
        /// </summary>
        public string Output { get; set; }
    }
}