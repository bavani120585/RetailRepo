using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClaudeApiDemo
{
    public class Demo3_MultipleMessages
    {
        private readonly VertexAiClient client;

        public Demo3_MultipleMessages(VertexAiClient client)
        {
            this.client = client;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("\n[Demo 3: Multiple Messages / Multi-turn Conversation]");
            Console.WriteLine("====================================================");

            var questions = new List<string>
            {
                "What are hooks in C# and how do they work?",
                "Can you show me a practical example of hooks?",
                "How do hooks differ from delegates?"
            };

            int turnNumber = 1;
            foreach (var question in questions)
            {
                Console.WriteLine($"\n--- Turn {turnNumber} ---");
                Console.WriteLine($"Question: {question}");
                Console.WriteLine("Sending request to Claude...\n");

                var response = await client.SendMessageAsync(question);

                Console.WriteLine($"Response:");
                Console.WriteLine("----");

                try
                {
                    var formatted = JsonConvert.SerializeObject(
                        JsonConvert.DeserializeObject(response),
                        Formatting.Indented);
                    Console.WriteLine(formatted);
                }
                catch
                {
                    Console.WriteLine(response);
                }

                Console.WriteLine("----");
                turnNumber++;

                if (turnNumber <= questions.Count)
                {
                    Console.WriteLine("\nWaiting before next question...");
                    await Task.Delay(1000);
                }
            }
        }
    }
}
