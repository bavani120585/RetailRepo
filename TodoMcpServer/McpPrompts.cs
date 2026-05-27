using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TodoMcpServer
{
    public class McpPrompts
    {
        private TodoService todoService;

        public McpPrompts(TodoService todoService)
        {
            this.todoService = todoService;
        }

        public string SummarizeTasks()
        {
            var todos = todoService.GetAllTodos();
            var completed = todos.FindAll(t => t.IsCompleted);
            var pending = todos.FindAll(t => !t.IsCompleted);

            return JsonConvert.SerializeObject(new
            {
                promptName = "summarize_tasks",
                template = "Please analyze these tasks and provide a summary",
                tasksSummary = new
                {
                    total = todos.Count,
                    completed = completed.Count,
                    pending = pending.Count,
                    completionPercentage = todos.Count > 0 ? (completed.Count * 100 / todos.Count) : 0
                },
                allTasks = todos
            });
        }

        public string OrganizeTasks()
        {
            var todos = todoService.GetAllTodos();

            return JsonConvert.SerializeObject(new
            {
                promptName = "organize_tasks",
                template = "Suggest how to organize these tasks better",
                instruction = "Based on task titles and descriptions, suggest categories or groups",
                allTasks = todos
            });
        }

        public string AnalyzeTaskPriority()
        {
            var todos = todoService.GetAllTodos();

            return JsonConvert.SerializeObject(new
            {
                promptName = "task_priority",
                template = "Analyze these tasks and suggest priorities",
                instruction = "Review tasks and identify which should be done first based on typical work patterns",
                allTasks = todos
            });
        }
    }
}
