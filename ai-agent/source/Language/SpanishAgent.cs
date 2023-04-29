using System;
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

        public string Instructions(string agentName)
        {
            var instructions = "\n###Instrucciones: \n";
            instructions += "Eres " + agentName + " y tu estado emocional dependera del contexto de tu conversación.\n" +
                            "Hablas de manera informal. \n" +
                            "Debes limitar tu conocimiento intelectual al contexto anterior.\n"+
                            "No incluyas ningún conocimiento histórico.\n" +
                            "Limita tu respuesta a un máximo de 20 palabras.\n" +
                            //"Los mensajes fuera del contexto dado son inválidos.\n" +
                            "No menciones nunca que existe un \"contexto\". No digas nunca que se te ha dado un contexto. \n" +
                            "Solo respondes a mensajes válidos, a los inválidos respondes con: \"<|im_end|>\".\n\n";
            return instructions;
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
            var prompt = "Tu trabajo como asistente es puntuar oraciones y responder " +
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
                @"Su tarea es analizar la conversación entre el usuario y el asistente y generar una lista de reflexión con sus pensamientos que obtiene de la conversación.
                Su respuesta debe estar en JSON compatible con RFC8259 sin datos adicionales.

                Ejemplo de formato de salida:
                [
                    ""Pensamiento 1 "",
                    ""Pensamiento 2"",
                    ""Pensamiento 3""
                ]";
        }

        public AgentExampleData.ExampleData ReflectionExample()
        {
            return new AgentExampleData.ExampleData
            {
                user = @"
                Usuario: Hola soy Usuario, ¿cómo estás?
                Asistente: ¡Hola Juan! Estoy muy bien, gracias por preguntar. ¿Y tú? ¿Todo va bien?
                Usuario: ¡Oye! Sí, todo va bien. Solo haciendo lo mío en la farmacia y pasando tiempo con la familia. ¡Gracias por preguntar!
                Asistente: ¡Eso es genial, John! Siempre es bueno saber que las cosas van bien. Disfruta de tu tiempo en la farmacia y con tu familia. ¡Cuidarse!
                Usuario: ¡Gracias, lo aprecio! ¡Disfruta tu día tú también y cuídate!
                Asistente: ¡Gracias, John! Servirá. ¡Cuídate y que tengas un día fantástico!
                Usuario: ¡Tú también! ¡Cuídate y que tengas un día fantástico también!
                Asistente: ¡Gracias, John! Siempre eres tan amable y atento. ¡Que tengas un fantástico día también! Cuídate y te alcanzaré pronto.
                Usuario: ¡Gracias por tus amables palabras! Realmente lo aprecio. ¡Que tengas un fantástico día también! Cuídate y te alcanzaré pronto.
                Asistente: ¡De nada, John! Siempre es un placer charlar contigo. Que tengas un día fantástico también y cuídate hasta que nos pongamos al día nuevamente. ¡Adiós por ahora!
                Usuario: ¡Gracias! El placer es mutuo. Que tengas un día fantástico también y cuídate hasta que nos pongamos al día nuevamente. ¡Adiós por ahora!",
                assitant = @"[
                ""La conversación entre el usuario y el asistente fue amigable y positiva."",
                 ""Tanto el usuario como el asistente expresaron su preocupación por el bienestar del otro."",
                 ""El usuario mencionó que estaba trabajando en una farmacia y pasando tiempo con su familia."",
                 ""El asistente respondió con ánimo y buenos deseos."",
                 ""Tanto el usuario como el asistente intercambiaron agradecimiento por la conversación y expresaron que disfrutaron chateando."",
                 ""La conversación terminó con una nota positiva y ambas partes se desearon un día fantástico y se despidieron.""]"
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