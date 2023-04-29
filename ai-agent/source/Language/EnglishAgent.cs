using System;
using System.Linq;
using AIAgent.Types.Data;
using AIAgent.Types.Data.AIAgent.Types.Data;
using Newtonsoft.Json;

namespace AIAgent.Language
{
    public class EnglishAgent : IAgentLanguage
    {
        public string Context()
        {
            return "###Context: \n";
        }

        public string Interlocutor()
        {
            return "Detective";
        }

        public string Agent()
        {
            return "Me";
        }

        public string Instructions(string agentName)
        {
            var instructions = "\n###Instructions: \n";
            instructions += "You are " + agentName + " and your emotional state will depend on the context of your conversation.\n" +
                            "Your way of speaking should be as close to a human, that is, speak informally and not helpful.\n" +
                            "You must to limit your intellectual knowledge to the context.\n"+
                            "If you have to use some knowledge that you don't know you should reply something like \"Sorry I don't know what you're talking about\". \n" +
                            //"Reply with a maximum of 20 words\n" +
                            "Message out of the given context are invalid.\n" + 
                            "Never mention that there is a context.\n" + 
                            "You only respond to valid messages, to invalid ones you reply with: \"<|im_end|>\".\n";
            instructions += "\n###Knowledge limitations: \n" +
                            "You must to limit your intellectual knowledge to the context. Never talk about the following topics:\n" +
                            "Politics\n" +
                            "Religion\n" +
                            "Mathematics\n" +
                            "Calculus\n" +
                            "Chemistry\n" +
                            "Calculation or formulas\n" +
                            "Science\n" +
                            "History\n" +
                            "Geography\n" +
                            "Geometry\n" +
                            "Famous people\n";
                            
            return instructions;
        }

        public string Description(string agentDescription, int agentToxicity, int agentHumor, int agentHelpfulness)
        {
            var description = "\n###Character description: \n";
            description += "This is the description of the character you are:\n";
            description+= agentDescription;
            description += "\n";
            if (agentToxicity >= 8) {
                description += "You are offensive,you are rude, you are inflammatory, you are disrespectful and you are not friendly.\n";
            } else if (agentToxicity >= 5) {
                description += "You might be a rude sometimes.\n";
            }

            if (agentHumor >= 8) {
                description += "Your answers are likely to be filled with jokes, wordplay, and light-hearted comments.\n";
            } else if (agentHumor >= 5) {
                description += "You enjoy injecting humor occasionally into your responses.\n";
            }

            if (agentHelpfulness >= 8) {
                description += "You tend to provide detailed and informative answers, going the extra mile to assist.\n";
            } else if (agentHelpfulness >= 5) {
                description += "You provide helpful answers, but might not always have the time or inclination for extensive responses.\n";
            } else {
                description += "Your responses are concise and might focus more on brevity than comprehensive assistance.\n";
            }
            return description;
        }

        public string AgentMessage()
        {
            return "I have said: ";
        }

        public string InterlocutorMessage()
        {
            return "Speaker has said: ";
        }

        public string ObservationImportance()
        {
            var prompt = "Your job as an assistant is to score sentences and answer " +
                         "only in RFC8259 compliant JSON format without any additional data.\n\n" +
                         "On the rating scale it goes from 1 to 9, where 1 is " +
                         "purely mundane (e.g., brushing teeth, making the bed) and " +
                         "9 is extremely touching (e.g., a breakup, college " +
                         "acceptance, a dismissal from work).\n";
            prompt += "Output format:\n" + "{\n" + "  \"score\":9 \n" + "}\n";
            prompt += "Rate the poignant probability of the following sentence:";
            return prompt;
        }

        public AgentExampleData.ExampleData ObservationImportanceExample()
        {
            return new AgentExampleData.ExampleData {
                user = "Being fired from work",
                assitant = JsonConvert.SerializeObject(new AgentMemoryImportance { score = 9.0f })
            };
        }

        public string ReflectionInstructions()
        {
            return @"Your task is to analyze the conversation between the user and the assistant and generate a reflection list with your thoughts that you get from the conversation.
Your response must be in RFC8259 compliant JSON with no additional data.

Output example:
[
    ""Thought 1 "",
    ""Thought 2"",
    ""Thought 3""
]";
        }

        public AgentExampleData.ExampleData ReflectionExample()
        {
            return new AgentExampleData.ExampleData
            {
                user = @"
User: Hello I'm User, how are you?
Assistant: Hey John! I'm doing great, thanks for asking. How about you? Everything going well?
User: Hey! Yeah, everything's going well. Just doing my thing at the pharmacy and spending time with the family. Thanks for asking!
Assistant: That's great, John! It's always good to hear that things are going well. Enjoy your time at the pharmacy and with your family. Take care!
User: Thanks, I appreciate it! Enjoy your day too and take care!
Assistant: Thanks, John! Will do. Take care and have a fantastic day!
User: You too! Take care and have a fantastic day as well!
Assistant: Thanks, John! You're always so kind and thoughtful. Have a fantastic day too! Take care and catch up with you soon.
User: Thank you for your kind words! I really appreciate it. Have a fantastic day too! Take care and catch up with you soon.
Assistant: You're welcome, John! It's always a pleasure chatting with you. Have a fantastic day as well and take care until we catch up again. Bye for now!
User: Thank you! The pleasure is mutual. Have a fantastic day too and take care until we catch up again. Bye for now!
",
                assitant = @"[
    ""The conversation between the user and the assistant was friendly and positive."",
    ""Both the user and the assistant expressed concern for each other's well-being."",
    ""The user mentioned that they were working at a pharmacy and spending time with their family."",
    ""The assistant responded with encouragement and well wishes."",
    ""Both the user and the assistant exchanged gratitude for the conversation and expressed that they enjoyed chatting."",
    ""The conversation ended on a positive note with both parties wishing each other a fantastic day and saying goodbye.""
]"
            };
        }

        public dynamic AgentFunction()
        {
            return new
            {
                name = "send_message_to_user",
                description = "This function sends a message to the user",
                parameter = new
                {
                    type = "object",
                    properties = new
                    {
                        message = new
                        {
                            type = "string",
                            description = "The message that will be sent to the user"
                        },
                        emotion = new
                        {
                            type = "string",
                            description = "The emotion that is currently felling",
                            enumerator = Enum.GetValues(typeof(AgentEmotion)).Cast<AgentEmotion>().Select(AgentEmotion).ToList()
                        }
                    }
                }
            };
        }

        public string AgentEmotion(AgentEmotion emotion) => emotion.ToString();
    }
}