# Getting Started

<br>

## Introduction

NoSpoonLab AI Library is a C# solution that integrates five sub-solutions, each providing a specific utility. The primary objective here is to simplify the developmental process for applications that interact with artificial intelligence, thereby allowing developers to focus on innovating rather than troubleshooting. Whether you are looking to build a revolutionary AI-integrated system or fine-tuning your current projects, our library offers unmatched utility rooted in professionalism and technical precision.

After the installation of the library you will be able to use the NoSpoonLab AI Library in your projects. 
The library is designed to be used in any C# project, including Unity projects. 
The library is compatible with .NET Framework 4.6.1 and .NET Standard 2.0.

Let's get started!

## Create a prompt object request

First lets create a prompt object request to do that you must to inherit from ```NoSpoonAITransformerPromptRequest ``` class and override the ```OnBeginInitialize``` method.

```csharp
public class BasicPrompt : NoSpoonAITransformerPromptRequest 
{
    public override void OnBeginInitialize() {}
}
```

Now lets create a constructor that receives the user prompt and system prompt using the base constructor.

```csharp
public class BasicPrompt : NoSpoonAITransformerPromptRequest 
{
    #region Constructor

    // Add a constructor that receives user prompt and system prompts
    public BasicPrompt(string userPrompt, string systemPrompt) : base(userPrompt, systemPrompt) {}

    #endregion
    
    public override void OnBeginInitialize() {}
}
```

Full example of a use case of the prompt object request

```csharp
public class AIExample : MonoBehaviour
{
    public async void Awake()
    {
        //First load the IAGPTService
        var openAIClient = new OpenAIClient();
        
        //Set the settings for the OpenAI Service
        openAIClient.SetSettings(new AIServiceInitData
        {
            APIKey = "Your OpenAI API Key",
            Model = AIModel.gpt_35_16k, //Default model we want to use in case we don't specify one in the prompt object request
            Type = AIServiceType.OpenAI
        });
        
        //Then load the NoSpoonAIClient
        var noSpoonAIClient = new NoSpoonAIClient();
        noSpoonAIClient.InitializeService<IAIGPTService>(openAIClient);
        
        //Create the prompt object request 
        var promptRequest = new BasicPrompt("What's on the menu for today and how much does it cost? ",
            "Your task is to report the prices of the menu. " +
             "- Coffee $1.10 " +
             "- Breakfast $3.50 " +
             "- Dinner $7.55");
        
        //Send the request and wait for the response
        var response = await noSpoonAIClient.SendTransformerRequest(promptRequest);
        
        //Log the response
        Debug.Log("AI response: " + response);
    }
}

And that's it! You have successfully created a prompt object request and sent it to the AI service.
```
