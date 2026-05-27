using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TodoMcpServer
{
    public class McpTodoServer
    {
        private TodoService todoService;
        private McpResources mcpResources;
        private McpTools mcpTools;
        private McpPrompts mcpPrompts;

        public McpTodoServer()
        {
            todoService = new TodoService();
            mcpResources = new McpResources(todoService);
            mcpTools = new McpTools(todoService);
            mcpPrompts = new McpPrompts(todoService);
        }

        public void Start()
        {
            Console.WriteLine("[MCP Server] To-Do Task Server Started");
            Console.WriteLine("[MCP Server] Listening for requests...");
            Console.WriteLine("[MCP Server] Type JSON requests and press Enter");
            Console.WriteLine();

            todoService.CreateTodo("Complete RetailApp", "Finish the MCP custom server demo");
            todoService.CreateTodo("Learn MCP Protocol", "Understand how MCP works with Resources, Tools, and Prompts");

            ListenForRequests();
        }

        private void ListenForRequests()
        {
            while (true)
            {
                try
                {
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input)) continue;

                    JObject request = JObject.Parse(input);
                    string action = request["action"]?.ToString();

                    string response = ProcessRequest(action, request);
                    Console.WriteLine(response);
                }
                catch (JsonException ex)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(new
                    {
                        error = "Invalid JSON format",
                        details = ex.Message
                    }));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(new
                    {
                        error = ex.Message
                    }));
                }
            }
        }

        private string ProcessRequest(string action, JObject request)
        {
            return action switch
            {
                "resource" => ProcessResource(request),
                "tool" => ProcessTool(request),
                "prompt" => ProcessPrompt(request),
                _ => JsonConvert.SerializeObject(new { error = $"Unknown action: {action}. Use 'resource', 'tool', or 'prompt'" })
            };
        }

        private string ProcessResource(JObject request)
        {
            string resource = request["resource"]?.ToString();
            return resource switch
            {
                "todo://tasks" => mcpResources.GetAllTasksResource(),
                "todo://tasks/completed" => mcpResources.GetCompletedTasksResource(),
                "todo://tasks/pending" => mcpResources.GetPendingTasksResource(),
                _ when resource?.StartsWith("todo://task/") == true =>
                    mcpResources.GetTaskResourceById(int.Parse(resource.Replace("todo://task/", ""))),
                _ => JsonConvert.SerializeObject(new { error = $"Unknown resource: {resource}" })
            };
        }

        private string ProcessTool(JObject request)
        {
            string tool = request["tool"]?.ToString() ?? "";
            return tool switch
            {
                "create_task" => mcpTools.CreateTaskTool(
                    request["title"]?.ToString() ?? "",
                    request["description"]?.ToString() ?? ""),
                "update_task" => mcpTools.UpdateTaskTool(
                    (int)(request["id"] ?? 0),
                    request["title"]?.ToString() ?? "",
                    request["description"]?.ToString() ?? "",
                    (bool)(request["isCompleted"] ?? false)),
                "delete_task" => mcpTools.DeleteTaskTool((int)(request["id"] ?? 0)),
                "complete_task" => mcpTools.CompleteTaskTool((int)(request["id"] ?? 0)),
                "get_all_tasks" => mcpTools.GetAllTasksTool(),
                _ => JsonConvert.SerializeObject(new { error = $"Unknown tool: {tool}" })
            };
        }

        private string ProcessPrompt(JObject request)
        {
            string prompt = request["prompt"]?.ToString();
            return prompt switch
            {
                "summarize_tasks" => mcpPrompts.SummarizeTasks(),
                "organize_tasks" => mcpPrompts.OrganizeTasks(),
                "task_priority" => mcpPrompts.AnalyzeTaskPriority(),
                _ => JsonConvert.SerializeObject(new { error = $"Unknown prompt: {prompt}" })
            };
        }
    }
}
