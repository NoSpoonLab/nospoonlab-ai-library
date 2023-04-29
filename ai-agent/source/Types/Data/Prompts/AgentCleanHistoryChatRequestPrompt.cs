using System;
using System.Collections.Generic;
using AICore.Infrastructure.NoSpoonAI.Types.Data;
using AICore.Infrastructure.NoSpoonAI.Types.Request;
using Newtonsoft.Json;

namespace AIAgent.Types.Data.Prompts
{
    public class AgentCleanHistoryChatRequestPrompt : NoSpoonAITransformerPromptRequest
    {
        #region Initialization

        public AgentCleanHistoryChatRequestPrompt(string toAnalize, List<string> dataToAnalize) :
            base()
        {
            var system = "Analiza cada uno de los mensajes de la lista y determina cuales mensajes tienen" +
                         " una relación con la oración indicada por el usuario y devuelve un array de enteros contando desde 0 con las posiciones" + 
                         " de los mensajes que cumplen con la condición, si no hay mensajes que cumplan con la condición" +
                         " se devuelve un array vacío";
            SystemPrompts().Add(system);
            
            ExamplePrompt(new NoSpoonAIExampleData()
            {
                Input = "Analiza: Tenia una aventura con Roberto Morales. \n\n Datos a analizar:\n " +
                        JsonConvert.SerializeObject(new List<string>
                        {
                            "Han encontrado a mi marido, Carlos, muerto en su despacho después de la cena. Estoy devastada.",
                            "No, detective. Estoy tan confundida y triste. No puedo creer que alguien haría algo así.",
                            "No, detective. Estoy tan confundida y triste. No puedo creer que alguien haría algo así.",
                            "Tenia una relación amorosa con Robertos Morales, el socio de Carlos. Carlos no sabía nada de esto.",
                            "Estábamos Carlos, yo, nuestros hijos Diego y Sofía, y Roberto Morales, el socio de Carlos. Todos cenamos juntos esa noche.",
                            "Roberto Morales es el socio de negocios de Carlos, un hombre astuto y ambicioso. Siempre estuvo en desacuerdo con Carlos sobre la dirección de la empresa.",
                            "La mucama sabía que tenia una aventura con Roberto Morales."
                        }),
                Output = JsonConvert.SerializeObject(new List<int> { 3, 6 })
            });

            UserPrompts().Add("Analiza: " + toAnalize + "\n\n Datos a analizar:\n " +
                              JsonConvert.SerializeObject(dataToAnalize));
        }
        public override void OnBeginInitialize()
        {
            MaxTokens(512);
            RetryDelay(150);
            RetryLimit(10);
            SetJsonModeFormat(true);
        }

        protected override T OnSuccessMessage<T>(string value)
        {
            try
            {
                var result = base.OnSuccessMessage<T>(value);
                if(result == null) throw new Exception("Result is null");
                return result;
            }
            catch (Exception e)
            {
                return new List<int>() as T;
            }
        }
        
        #endregion
    }
}