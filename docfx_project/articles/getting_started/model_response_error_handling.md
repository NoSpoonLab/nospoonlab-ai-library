## Model response error handling

### Error example

It is possible that even if our request is successful, it does not come in the format in which we expected.

But even so, there is the possibility of extracting the data of our interest.

For example:

Let's say we expect an object with the following format:

JSON format:
````json
{
    "score": 3
}
````

Because our data model is the following:

C# format:
````csharp
public struct BasicPromptResponse
{
    public int score { get; set; }
}
````

This in principle if we get the answer above we would not have any problem.

But if the model responded with something like this:

````string
Seems like a very rare case, my score for this is 7
````

When parsing JSON to BasicPromptResponse we will get an exception.

### Solution

To solve this problem we have the following methods that we can override in our request class [NoSpoonAIRequest](xref:AICore.Infrastructure.NoSpoonAI.Types.Request.NoSpoonAIRequest) this class is the base class of [NoSpoonAITransformerPromptRequest](xref:AICore.Infrastructure.NoSpoonAI.Types.Request.NoSpoonAITransformerPromptRequest):

````csharp
protected virtual T OnSuccessMessage<T>(string value)
protected virtual T OnSuccessFunction<T>(string value)
//If we need to know the usage cost of the request override this methods
protected virtual T OnSuccessMessage<T>(string value, UsageData usageData)
protected virtual T OnSuccessFunction<T>(string value, UsageData usageData) 
````

This method is called in order to parse the response into the type we want.

So the idea is to try using the natural language tools to extract the data we want.
For example we can do the following:

````csharp
public class BasicPrompt : NoSpoonAITransformerPromptRequest 
{
    public BasicPrompt(string userPrompt) : base(userPrompt) {}
    public override void OnBeginInitialize() {}

    protected override T OnSuccessMessage<T>(string value)
    {
        try
        {
            return base.OnSuccessMessage<T>(value);
        }
        catch
        {
            var regex = new Regex(@"\d+");
            var matches = regex.Matches(value);
            var numbers = (from Match match in matches select int.Parse(match.Value)).ToList();
            if(numbers.Count == 2 || numbers.Count > 3) throw new Exception("Invalid number of numbers found, after trying to convert the data to AgentMemoryImportance");
            if(numbers.Count == 1) return new BasicPromptResponse { score = numbers.First() } as T;
            return new BasicPromptResponse { score = numbers.First(it => it != 1 && it != 9 && it != 10 && it != 0) } as T;
        }
    }
````
If even after these rules to extract information we are not able to extract the data that interests us.

We will throw an exception which will cause the execution of a new request to the AI service until the maximum number of attempts are exhausted.