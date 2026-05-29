using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClaudeApiDemo
{
    public class Demo2_TaskAnalysis
    {
        private readonly VertexAiClient client;

        public Demo2_TaskAnalysis(VertexAiClient client)
        {
            this.client = client;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("\n[Demo 2: Task Analysis]");
            Console.WriteLine("=====================================");
            Console.WriteLine("Task: Build a todo application with C# and MCP protocol");
            Console.WriteLine("\nSending request to Claude...\n");

            var message = @"I need to build a todo application with C# and MCP protocol.
Please provide:
1. Key components needed
2. Implementation plan
3. Potential challenges
4. Timeline estimate";

            var response = await client.SendMessageAsync(message);

            Console.WriteLine("Claude's Analysis:");
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
