using System.Collections.Generic;
using System.Linq;
using AICore.Infrastructure.NoSpoonAI.Types.Data;
using AICore.Services.Types.Data;
using AICore.Services.Types.Request;

namespace AICore.Infrastructure.NoSpoonAI.Types.Request
{
    /// <summary>
    /// An abstract base class defining a transformer prompt request for the NoSpoonAI client interaction. 
    /// This class is designed to conveniently build the request content required when interacting with AI generative pre-trained transformer model service.
    /// This class is designed to be inherited by other classes to create a prompt object that will be sent to the AI model through the NoSpoonAI client.
    /// </summary>
    /// <example>
    /// Here is an example of an override class that implements a prompt object.
    /// <code>
    ///    public class JsonTransformerPromptRequest : NoSpoonAITransformerPromptRequest
    ///    {
    ///        
    ///        #region Initialization
    ///      
    ///        public JsonTransformerPromptRequest(List&lt;string&gt; userPrompts) : base(userPrompts){}
    ///        public JsonTransformerPromptRequest(string userPrompt) : base(userPrompt){}
    ///        public override void OnBeginInitialize()
    ///        {
    ///            //System prompts
    ///            var prompt = "Your job as an assistant is to score sentences and answer " +
    ///                         "only in RFC8259 compliant JSON format without any additional data.\n\n" +
    ///                         "On the rating scale it goes from 1 to 9, where 1 is " +
    ///                         "purely mundane (e.g., brushing teeth, making the bed) and " +
    ///                         "9 is extremely touching (e.g., a breakup, college " +
    ///                         "acceptance, a dismissal from work).\n";
    ///            prompt += "Output format:\n" + "{\n" + "  \"score\":9 \n" + "}\n";
    ///            prompt += "Rate the poignant probability of the following sentence:";
    ///            SystemPrompts(new List&lt;string&gt;{ prompt });
    ///            
    ///            //Example prompts
    ///            ExamplePrompts(new NoSpoonAIExampleData
    ///            {
    ///                Input = "Being fired from work",
    ///                Output = "{\n" + "  \"score\":9 \n" + "}"
    ///            });
    ///        }
    ///
    ///        #endregion
    ///
    ///        #region Methods
    ///        
    ///        protected override T OnSuccessMessage List&lt;T&gt; (string value)
    ///        {
    ///            try
    ///            {
    ///                return base.OnSuccessMessage List&lt;T&gt; (value);
    ///            }
    ///            catch
    ///            {
    ///                Console.WriteLine("Conversion Failed, with the data: " + value);
    ///                var regex = new Regex(@"\d+");
    ///                var matches = regex.Matches(value);
    ///                var numbers = (from Match match in matches select int.Parse(match.Value)).ToList();
    ///                if(numbers.Count == 2 || numbers.Count > 3) throw new Exception("Invalid number of numbers found, after trying to convert the data to AgentMemoryImportance");
    ///                if(numbers.Count == 1) return new JSONTransformerResult { score = numbers.First() } as T;
    ///                try
    ///                {
    ///                    return new JSONTransformerResult { score = numbers.First(it => rules ) } as T;
    ///                }
    ///                catch
    ///                {
    ///                    return new JSONTransformerResult { score = 0 } as T;
    ///                }
    ///            }
    ///        }
    ///
    ///        #endregion
    ///    }
    /// </code>
    /// </example>
    public abstract class NoSpoonAITransformerPromptRequest : NoSpoonAIRequest
    {
        #region Properties

        /// <summary>
        /// The underlying transformer request that will be sent to the AI model. 
        /// </summary>
        private readonly AITransformerRequest _request = new AITransformerRequest();

        /// <summary>
        /// A list of prompts provided by the user.
        /// </summary>
        private List<string> _userPrompts = new List<string>();

        /// <summary>
        /// A list of prompts that set the system behavior.
        /// </summary>
        private List<string> _systemPrompts = new List<string>();

        /// <summary>
        ///  Example prompts provided for the model to refer to during the conversation.
        /// </summary>
        private List<NoSpoonAIExampleData> _examplePrompts = new List<NoSpoonAIExampleData>();

        private object _toolMode;

        private List<Tool> _tools = new List<Tool>();

        /// <summary>
        /// Flag to ensure the request is only initialized once.
        /// </summary>
        private bool _initialized = false;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpoonAITransformerPromptRequest"/> class with user prompts.
        /// </summary>
        /// <param name="userPrompts">List of user prompts.</param>
        public NoSpoonAITransformerPromptRequest(List<string> userPrompts) => UserPrompts(userPrompts);

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpoonAITransformerPromptRequest"/> class with a single user prompt.
        /// </summary>
        /// <param name="userPrompt">The user prompt.</param>
        public NoSpoonAITransformerPromptRequest(string userPrompt) : this(new List<string>{userPrompt}){}

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpoonAITransformerPromptRequest"/> class with user and system prompts.
        /// </summary>
        /// <param name="userPrompts">List of user prompts.</param>
        /// <param name="systemPrompts">List of system prompts.</param>
        public NoSpoonAITransformerPromptRequest(List<string> userPrompts, List<string> systemPrompts) : this(userPrompts)
        {
            SystemPrompts(systemPrompts);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpoonAITransformerPromptRequest"/> class with a user prompt and a system prompt.
        /// </summary>
        /// <param name="userPrompt">The user prompt.</param>
        /// <param name="systemPrompt">The system prompt.</param>
        public NoSpoonAITransformerPromptRequest(string userPrompt, string systemPrompt) :
            this(new List<string>{userPrompt}, new List<string>{systemPrompt}) {}
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpoonAITransformerPromptRequest"/> class with a user prompt, a system prompt, and example data.
        /// </summary>
        /// <param name="userPrompt">The user prompt.</param>
        /// <param name="systemPrompt">The system prompt.</param>
        /// <param name="exampleData">Example data for the model reference during the conversation.</param>
        public NoSpoonAITransformerPromptRequest(string userPrompt, string systemPrompt, NoSpoonAIExampleData exampleData) :
            this(userPrompt, systemPrompt)
        {
            ExamplePrompt(exampleData);
        }

        /// <summary>
        /// Default constructor initializing a new instance of the <see cref="NoSpoonAITransformerPromptRequest"/> class.
        /// </summary>
        public NoSpoonAITransformerPromptRequest(){}

        /// <summary>
        /// Abstract method to be overridden in derived classes for performing specific initialisation steps.
        /// </summary>
        public virtual void OnBeginInitialize(){}

        /// <summary>
        /// Initializes the transformer request object by adding system, user, and example prompts to the list of messages and setting the function mode and functions, if applicable.
        /// </summary>
        public virtual void Initialize()
        {
            SystemPrompts().ForEach(it => AddMessage(it, AIRole.System));
            if (ExamplePrompts() != null)
            {
                ExamplePrompts().ForEach(it =>
                {
                    if(!string.IsNullOrEmpty(it.Input)) AddMessage(it.Input, AIRole.User);
                    if(!string.IsNullOrEmpty(it.Output)) AddMessage(it.Output, AIRole.Assistant);
                });
            }
            if (ToolsMode() != null) _request.tool_choice = ToolsMode();
            if (ToolsMode() != null && Tools().Count != 0) _request.tools = Tools();
            UserPrompts().ForEach(it => AddMessage(it, AIRole.User));
        }

        /// <summary>
        /// Method to check if the request object has been initialized.
        /// </summary>
        /// <returns>True if the request object has been initialized, otherwise false.</returns>
        internal bool IsInitialized()
        {
            var currentInitialized = _initialized;
            _initialized = true;
            return currentInitialized;
        }

        /// <summary>
        /// Adds a new message into the list of messages for the transformer request.
        /// </summary>
        /// <param name="message">The content of the message to be added.</param>
        /// <param name="role">The role of the entity (the system, user, function, or assistant) sending the message.</param>
        public void AddMessage(string message, AIRole role) => _request.messages.Add(new DataMessage{ content = message, role = role });

        #endregion
        
        #region Getters

        /// <summary>
        /// Getter for transformer request object.
        /// </summary>
        public AITransformerRequest Request() => _request;

        /// <summary>
        /// Getter for user prompts list.
        /// </summary>
        public List<string> UserPrompts() => _userPrompts;

        /// <summary>
        /// Getter for system prompts list.
        /// </summary>
        public List<string> SystemPrompts() => _systemPrompts;

        /// <summary>
        /// Getter for example prompts.
        /// </summary>
        public NoSpoonAIExampleData ExamplePrompt() => _examplePrompts.First();
        public List<NoSpoonAIExampleData> ExamplePrompts() => _examplePrompts;

        /// <summary>
        /// Getter for function mode.
        /// </summary>
        public object ToolsMode() => _toolMode;

        /// <summary>
        /// Getter for functions list.
        /// </summary>
        public List<Tool> Tools() => _tools;

        /// <summary>
        /// Getter for transformer request messages list.
        /// </summary>
        public List<DataMessage> Messages() => _request.messages;

        /// <summary>
        /// Getter for maximum tokens defined for the model to generate.
        /// </summary>
        public int MaxTokens() => _request.max_tokens;

        /// <summary>
        /// Getter for temperature setting, which controls randomness in output generation.
        /// </summary>
        public float Temperature() => _request.temperature;
        
        /// <summary>
        /// Getter for top p setting, which is used in nucleus sampling strategy.
        /// </summary>
        public float TopP() => _request.top_p;
        
        /// <summary>
        ///  Getter for AI model, which is used for the request.
        /// </summary>
        public AIModel Model() => _request.model;

        #endregion

        #region Setters

        /// <summary>
        /// Setter for user prompts list.
        /// </summary>
        public void UserPrompts(List<string> value) => _userPrompts = value;

        /// <summary>
        /// Setter for system prompts list.
        /// </summary>
        public void SystemPrompts(List<string> value) => _systemPrompts = value;
        
        /// <summary>
        /// Setter for example prompts.
        /// </summary>
        public void ExamplePrompt(NoSpoonAIExampleData value) => _examplePrompts.Add(value);
        public void ExamplePrompts(List<NoSpoonAIExampleData> value) => _examplePrompts = value;

        /// <summary>
        /// Setter for function mode.
        /// </summary>
        public void ToolsMode(object value) => _toolMode = value;

        /// <summary>
        /// Setter for functions list.
        /// </summary>
        public void Tools(List<Tool> value) => _tools = value;

        /// <summary>
        /// Setter for transformer request messages list.
        /// </summary>
        public void Messages(List<DataMessage> value) => _request.messages = value;

        /// <summary>
        /// Setter for maximum tokens defined for the model to generate.
        /// </summary>
        public void MaxTokens(int value) => _request.max_tokens = value;

        /// <summary>
        /// Setter for temperature setting, which controls randomness in output generation.
        /// </summary>
        public void Temperature(float value) => _request.temperature = value;

        /// <summary>
        /// Setter for top p setting, which is used in nucleus sampling strategy.
        /// </summary>
        public void TopP(float value) => _request.top_p = value;
        
        /// <summary>
        /// Setter for model, which is used in the request.
        /// </summary>
        public void Model(AIModel value) => _request.model = value;

        public void SetJsonModeFormat(bool value)
        {
            if(value) _request.response_format.SetTypeJson();
            else _request.response_format.SetTypeText();
        }
        
        public void SetStructuredOutputFormat<T>(bool value, List<string> requiredProperties = null)
        {
            if(value) _request.response_format.SetTypeStructuredOutput<T>(requiredProperties);
            else _request.response_format.SetTypeText();
        }
        
        #endregion
    }
}