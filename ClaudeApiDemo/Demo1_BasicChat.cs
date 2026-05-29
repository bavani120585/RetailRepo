using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClaudeApiDemo
{
    public class Demo1_BasicChat
    {
        private readonly VertexAiClient client;

        public Demo1_BasicChat(VertexAiClient client)
        {
            this.client = client;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("\n[Demo 1: Basic Chat with Claude]");
            Console.WriteLine("=====================================");
            Console.WriteLine("Question: What is the capital of France?");
            Console.WriteLine("\nSending request to Claude...\n");

            var message = "What is the capital of France?";
            var response = await client.SendMessageAsync(message);

            Console.WriteLine("Claude's Response:");
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
        }
    }
}
