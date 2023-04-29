## Prompt Constructors

As we saw in the Getting started step the use of a constructor of type (string, string) we will now see all the possible constructors that we can use.

#### Constructor (string userPrompt)

In the parent constructor [NoSpoonAITransformerPromptRequest](xref:AICore.Infrastructure.NoSpoonAI.Types.Request.NoSpoonAITransformerPromptRequest) the functionality that the first string type parameter passed is added to the [AITransformerRequest](xref:AICore.Services.Types.Request.AITransformerRequest) object as a new message sent by the user is implemented.

```csharp
public class BasicPrompt : NoSpoonAITransformerPromptRequest 
{
    public BasicPrompt(string userPrompt) : base(userPrompt) {}
}
```

#### Constructor (string userPrompt, string systemPrompt)

The parent class [NoSpoonAITransformerPromptRequest](xref:AICore.Infrastructure.NoSpoonAI.Types.Request.NoSpoonAITransformerPromptRequest) sends to [AITransformerRequest](xref:AICore.Services.Types.Request.AITransformerRequest) the first parameter as a user message and the second parameter as a system message.

```csharp
public class BasicPrompt : NoSpoonAITransformerPromptRequest 
{
    public BasicPrompt(string userPrompt, string systemPrompt) : base(userPrompt, systemPrompt) {}
}
```

#### Constructor (string userPrompt, string systemPrompt, NoSpoonAIExampleData exampleData)

The parent class [NoSpoonAITransformerPromptRequest](xref:AICore.Infrastructure.NoSpoonAI.Types.Request.NoSpoonAITransformerPromptRequest) sends to [AITransformerRequest](xref:AICore.Services.Types.Request.AITransformerRequest) the first parameter as a user message and the second parameter as a system message and the third parameter as sample data, these sample data will be sent as user message and assistant response message.

```csharp
public class BasicPrompt : NoSpoonAITransformerPromptRequest 
{
    public BasicPrompt(string userPrompt, string systemPrompt, NoSpoonAIExampleData exampleData) : base(userPrompt, systemPrompt, exampleData) {}
}
```

The purpose of this is to orient the model to the type of response we expect.


#### Constructor string of type (List<string> userPrompts, List<string> systemPrompts, NoSpoonAIExampleData exampleData)

string parameter can be also a list of strings.

```csharp
public class BasicPrompt : NoSpoonAITransformerPromptRequest 
{
    public BasicPrompt(List<string> userPrompts, List<string> systemPrompts, NoSpoonAIExampleData exampleData) : base(userPrompts, systemPrompts, exampleData) {}
}
```