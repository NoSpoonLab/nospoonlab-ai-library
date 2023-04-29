## Requests with Functions Calling

In the case of the OpenAI service, it implements 2 models that allow calling functions directly.

The models are:
- GPT-3.5
- GPT-4

In other words, you can have your prompt object request that, in addition to the system prompt and the user prompt, has added functions which the message-based model can make the decision to call a function or send a message.

Let's simulate for a moment a prompt with functions:

System Prompt: You are an assistant and your function is to talk with a potential client. If the client indicates that he is interested in buying, you must pass the conversation on to a human agent and if the query is to find out more details about the product, you will send an email with product details.

Function Mode: Auto

Features:
- transfer_conversation_to_human_agent(userName)
- send_email_with_product_details(userEmail, productId)

User Prompt: (Depending on what the user sends here, one of the functions will be called or it will be answered with a message)

Let's go to a use case that you can find in the test project:

### Function modes

Setup the function mode we have 3 options:
- default (Model will no call any function will send only messages)
- auto (Model will call the functions when the model decides to do so, but can also only send a message)
- force (Model will call always one function)

default:
```string
default is set by default, so you don't need to do anything.
```

Auto: 
```csharp
public override void OnBeginInitialize()
    {
        FunctionMode("auto");
    }
```

Force:
```csharp
public override void OnBeginInitialize()
    {
        FunctionMode(new FunctionDetail{ name = "call_to_my_function"});
    }
```

### Creating a function

The function object already exists, but what does not exist are the properties that we will have to create for each of our functions.

These properties are the arguments that our function will receive.

- Object [Function](xref:AICore.Services.Types.Request.Function)
- Object [FunctionParameter](xref:AICore.Services.Types.Request.FunctionParameter)

We need to create our properties, let's say we have two properties, one is the user's email and the other is the product id:

```csharp
public class EmailProperty
    {
        public string type{ get; set; }
        public string description{ get; set; }
    }

public class ProductIdProperty
    {
        public string type{ get; set; }
        public string description{ get; set; }
    
        [JsonProperty("enum")]
        public List<string> enumerator { get; set; }
    }
```

Now we need to create an object that will contain all our properties:

```csharp
public class SendEmailProperties
    {
        public EmailProperty email{ get; set; }
        public ProductIdProperty productId{ get; set; }
    }
```

Now we have to create the function object and define its name, its type and also define its properties:

```csharp
    new Function
    {
        name = "send_email_with_product_details",
        description = "Send an email to the user with the details of the product that is currently being discussed in this conversation.",
        parameters = new FunctionParameter
        {
            type = "object",
            required = new List<string> { "email","productId" }, //This is the name of the property that must be included in the model response
            properties = new SendEmailProperties
            {
                email = new EmailProperty
                {
                    type = "string",
                    description = "User email extracted from the user message"
                },
                productId = new ProductIdProperty{
                    type = "integer",
                    description = "Product id extracted from the user message",
                    enumerator = new List<string>
                    {
                        "1", "2", "3", "4", "5", "6","7", "8", "9", "10"
                    }
                }
            }
        }
    };
```

### Response object

To get a response we can either receive the string directly without parsing or parse it before it reaches us.

What should a response object contain?

The arguments in string format, for example:

```csharp
 public class SendEmailResponse
    {
        public string email;
        public int productId;
    }
```

### The full code example

```csharp

using System.Collections.Generic;
using AICore.Infrastructure.NoSpoonAI.Types.Request;
using AICore.Services.Types.Request;
using Newtonsoft.Json;

public class FunctionPrompt : NoSpoonAITransformerPromptRequest
{
    public FunctionPrompt(string userPrompt) : base(userPrompt)
    {
    }

    public override void OnBeginInitialize()
    {
        FunctionMode(new FunctionDetail { name = "" });
        Functions(new List<Function>
        {
            new Function
            {
            name = "send_email_with_product_details",
            description = "Send an email to the user with the details of the product that is currently being discussed in this conversation.",
            parameters = new FunctionParameter
            {
                type = "object",
                required = new List<string> { "email","productId" },//This is the name of the property that must be included in the model response
                properties = new SendEmailProperties
                {
                    email = new EmailProperty
                    {
                        type = "string",
                        description = "User email extracted from the user message"
                    },
                    productId = new ProductIdProperty{
                        type = "integer",
                        description = "Product id extracted from the user message",
                        enumerator = new List<string>
                        {
                            "1", "2", "3", "4", "5", "6","7", "8", "9", "10"
                        }
                    }
                }
            }
        }
        });
    }
    
    public class SendEmailProperties
    {
        public EmailProperty email{ get; set; }
        public ProductIdProperty productId{ get; set; }
    }
    
    public class EmailProperty
    {
        public string type{ get; set; }
        public string description{ get; set; }
    }

    public class ProductIdProperty
    {
        public string type{ get; set; }
        public string description{ get; set; }
    
        [JsonProperty("enum")]
        public List<string> enumerator { get; set; }
    }
}

```

### How to send a function request

If we want the response to be of the SendEmailResponse type, we would do it this way:

```csharp
var promptRequest = new FunctionPrompt("I want to buy product 3");
var response = await _noSpoonAIClient.SendTransformerRequest<SendEmailResponse, FunctionPrompt>(promptRequest);
```

If we want the response to be in raw format as a string of this:
```csharp
var promptRequest = new FunctionPrompt("I want to buy product 3");
var response = await _noSpoonAIClient.SendTransformerRequest(promptRequest);
```
