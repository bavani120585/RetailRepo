using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ClaudeApiDemo;

Console.WriteLine("=====================================");
Console.WriteLine("Claude API with Vertex AI Demo");
Console.WriteLine("=====================================\n");

try
{
    var configPath = "gcp-config.json";
    if (!File.Exists(configPath))
    {
        Console.WriteLine("Error: gcp-config.json not found!");
        return;
    }

    var configJson = File.ReadAllText(configPath);
    dynamic config = JsonConvert.DeserializeObject(configJson);

    string projectId = config["projectId"];
    string region = config["region"];
    string model = config["model"];
    string apiEndpoint = config["apiEndpoint"];

    Console.WriteLine("[Vertex AI Integration]");
    Console.WriteLine($"  Project: {projectId}");
    Console.WriteLine($"  Region: {region}");
    Console.WriteLine($"  Model: {model}\n");

    var client = new VertexAiClient(projectId, region, model, apiEndpoint);

    while (true)
    {
        Console.WriteLine("\nSelect Demo:");
        Console.WriteLine("  1 = Basic Chat (Simple question)");
        Console.WriteLine("  2 = Task Analysis (Complex breakdown)");
        Console.WriteLine("  3 = Multiple Messages (Multi-turn)");
        Console.WriteLine("  4 = Exit\n");
        Console.Write("Enter choice (1-4): ");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                var demo1 = new Demo1_BasicChat(client);
                await demo1.RunAsync();
                break;

            case "2":
                var demo2 = new Demo2_TaskAnalysis(client);
                await demo2.RunAsync();
                break;

            case "3":
                var demo3 = new Demo3_MultipleMessages(client);
                await demo3.RunAsync();
                break;

            case "4":
                Console.WriteLine("\nGoodbye!");
                return;

            default:
                Console.WriteLine("\nInvalid choice. Please try again.");
                break;
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
}
