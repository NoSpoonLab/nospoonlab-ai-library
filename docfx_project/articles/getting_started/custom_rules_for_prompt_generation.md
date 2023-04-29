## Custom rules for prompt generation

We may want to add default instructions, change models depending on the prompt, change the maximum number of tokens, the maximum number of attempts, or whatever we need to do before the request is sent.

For those cases we have the OnBeginInitialize() function that will be executed just before the prompt is added to the object [AITransformerRequest](xref:AICore.Services.Types.Request.AITransformerRequest):

````csharp
public class BasicPrompt : NoSpoonAITransformerPromptRequest 
{
    #region Constructor

    // Add a constructor that receives user prompt and system prompts
    public BasicPrompt(string userPrompt) : base(userPrompt) {}

    #endregion

    public override void OnBeginInitialize()
    {
        //Add a predefined system prompt for this prompt object request
        SystemPrompts(new List<string> {"Your task is to score the following essay: "});

        //Set the max tokens for this prompt object request
        MaxTokens(2048);
        
        //Set the temperature for this prompt object request
        Temperature(0.7f);
        
        //Set the top p for this prompt object request
        TopP(1f);

        //Set the retry time between requests for this prompt object request
        RetryDelay(1000);
        
        //Set the number of retries for this prompt object request
        RetryLimit(5);

        //Use the best model available for this prompt length
        Model(TokenUtils.Tokenizer(UserPrompts().First()) > 2048 ? AIModel.gpt : AIModel.gpt_35_16k);
    }
}

````