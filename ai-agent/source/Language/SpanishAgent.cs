using System;
using System.Collections.Generic;
using System.Linq;
using AIAgent.Types.Data;
using AIAgent.Types.Data.AIAgent.Types.Data;
using Newtonsoft.Json;

namespace AIAgent.Language
{
    public class SpanishAgent : IAgentLanguage
    {
        public string Context()
        {
            return "###Contexto: \n";
        }

        public string Interlocutor()
        {
            return "Detective";
        }

        public string Agent()
        {
            return "Yo";
        }

        public string Instructions(string agentName)
        {
            var instructions = "\n###Instrucciones: \n";
            instructions += "Eres " + agentName + " y tu estado emocional dependera del contexto de tu conversación.\n" +
                            "Tu forma de hablar es informall.\n" +
                            "Tus respuestas deben ser coherentes con la conversación.\n" +
                            "No debes preguntar al usuario quien es. \n" +
                            "Debes limitar tu conocimiento intelectual al contexto.\n"+
                            "No incluyas ningún conocimiento histórico.\n" +
                            "Limita tu respuesta a un máximo de 30 palabras.\n" +
                            //"Los mensajes fuera del contexto dado son inválidos.\n" +
                            "No menciones nunca que existe un \"contexto\". No digas nunca que se te ha dado un contexto. \n" +
                            "Preguntas del usuario que salen de contexto, debes reconduncir la pregunta a alguna de tus memorias.\n\n";
            return instructions;
        }

        public string Description(string agentDescription, int agentToxicity, int agentHumor, int agentHelpfulness)
        {
            var description = "\n###Descripción del personaje: \n";
            description += "Esta es la descripción del personaje que tu eres:\n";
            description+= agentDescription;
            description += "\n";
            if (agentToxicity >= 8) {
                description += "Eres ofensivo/a, eres grosero/a, eres inflamatorio/a, eres irrespetuoso/a y no eres amigable.\n";
            } else if (agentToxicity >= 5) {
                description += "Puedes ser grosero/a en ocasiones.\n";
            }

            if (agentHumor >= 8) {
                description += "Tus respuestas tienden a estar llenas de chistes, juegos de palabras y comentarios lúdicos.\n";
            } else if (agentHumor >= 5) {
                description += "Disfrutas introduciendo humor ocasionalmente en tus respuestas.\n";
            }

            if (agentHelpfulness >= 8) {
                description += "Sueles proporcionar respuestas detalladas e informativas, y te esfuerzas por ayudar al máximo.\n";
            } else if (agentHelpfulness >= 5) {
                description += "Proporcionas respuestas útiles, pero es posible que no siempre tengas el tiempo o la inclinación para respuestas extensas.\n";
            } else {
                description += "Tus respuestas son concisas y pueden centrarse más en la brevedad que en la asistencia completa.\n";
            }
            return description;
        }

        public string AgentMessage()
        {
            return "Yo he dicho: ";
        }

        public string InterlocutorMessage()
        {
            return "Tu interlocutor ha dicho: ";
        }

        public string ObservationImportance()
        {
            var prompt = "Puntua las oraciones del 1 al 9 y responder " +
                         "solo en formato JSON compatible con RFC8259 sin datos adicionales.\n\n" +
                         "En la escala de calificación va del 1 al 9, donde 1 es " +
                         "Puramente mundano (por ejemplo, cepillarse los dientes, hacer la cama) y " +
                         "9 es extremadamente conmovedor (por ejemplo, una ruptura amorosa, la aceptación en una " +
                         "universidad, un despido del trabajo).\n";
            prompt += "Formato de salida:\n" + "{\n" + "  \"score\":9 \n" + "}\n";
            prompt += "Califique la probabilidad conmovedora de la siguiente oración:";
            return prompt;
        }

        public AgentExampleData.ExampleData ObservationImportanceExample()
        {
            return new AgentExampleData.ExampleData {
                user = "Ser despedido del trabajo",
                assitant = JsonConvert.SerializeObject(new AgentMemoryImportance { score = 9.0f })
            };
        }

        public string ReflectionInstructions()
        {
            return
                "Analiza la siguiente conversación y generar una lista de reflexión de tus pensamientos sobre la conversación.\n" +
                "Su respuesta debe estar en JSON compatible con RFC8259 sin datos adicionales.\n\n" +
                "Ejemplo de formato de salida: \n" +
                JsonConvert.SerializeObject(new List<string>{"Pensamiento 1", "Pensamiento 2", "Pensamiento 3"});
        }

        public AgentExampleData.ExampleData ReflectionExample()
        {
            return new AgentExampleData.ExampleData
            {
                user = @"
                Detective: Hola soy Usuario, ¿cómo estás?
                Tu: ¡Hola Juan! Estoy muy bien, gracias por preguntar. ¿Y tú? ¿Todo va bien?
                Detective: ¡Oye! Sí, todo va bien. Solo haciendo lo mío en la farmacia y pasando tiempo con la familia. ¡Gracias por preguntar!
                Tu: ¡Eso es genial, John! Siempre es bueno saber que las cosas van bien. Disfruta de tu tiempo en la farmacia y con tu familia. ¡Cuidarse!
                Detective: ¡Gracias, lo aprecio! ¡Disfruta tu día tú también y cuídate!
                Tu: ¡Gracias, John! Servirá. ¡Cuídate y que tengas un día fantástico!
                Detective: ¡Tú también! ¡Cuídate y que tengas un día fantástico también!
                Tu: ¡Gracias, John! Siempre eres tan amable y atento. ¡Que tengas un fantástico día también! Cuídate y te alcanzaré pronto.
                Detective: ¡Gracias por tus amables palabras! Realmente lo aprecio. ¡Que tengas un fantástico día también! Cuídate y te alcanzaré pronto.
                Tu: ¡De nada, John! Siempre es un placer charlar contigo. Que tengas un día fantástico también y cuídate hasta que nos pongamos al día nuevamente. ¡Adiós por ahora!
                Detective: ¡Gracias! El placer es mutuo. Que tengas un día fantástico también y cuídate hasta que nos pongamos al día nuevamente. ¡Adiós por ahora!",
                assitant = JsonConvert.SerializeObject(new List<string>
                {
                    "La conversación entre el usuario y el asistente fue amigable y positiva.",
                    "Tanto el detective como yo expresamos preocupación por el bienestar del otro.",
                    "El detective mencionó que estaba trabajando en una farmacia y pasando tiempo con su familia.",
                    "yo respondi con ánimo y buenos deseos.",
                    "La conversación terminó con una nota positiva y ambos nos deseamos un día fantástico y nos despedimos."
                })
            };
        }

        public dynamic AgentFunction()
        {
            return new
            {
                name = "send_message_to_user",
                description = "This function sends a message to the user with the response of the character being simulated.",
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