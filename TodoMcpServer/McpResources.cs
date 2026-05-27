using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TodoMcpServer
{
    public class McpResources
    {
        private TodoService todoService;

        public McpResources(TodoService todoService)
        {
            this.todoService = todoService;
        }

        public string GetAllTasksResource()
        {
            var todos = todoService.GetAllTodos();
            return JsonConvert.SerializeObject(new
            {
                resourceUri = "todo://tasks",
                resourceType = "list",
                description = "All to-do tasks",
                data = todos
            });
        }

        public string GetCompletedTasksResource()
        {
            var todos = todoService.GetAllTodos();
            var completed = todos.FindAll(t => t.IsCompleted);
            return JsonConvert.SerializeObject(new
            {
                resourceUri = "todo://tasks/completed",
                resourceType = "list",
                description = "Completed tasks",
                data = completed
            });
        }

        public string GetPendingTasksResource()
        {
            var todos = todoService.GetAllTodos();
            var pending = todos.FindAll(t => !t.IsCompleted);
            return JsonConvert.SerializeObject(new
            {
                resourceUri = "todo://tasks/pending",
                resourceType = "list",
                description = "Pending tasks",
                data = pending
            });
        }

        public string GetTaskResourceById(int id)
        {
            var todo = todoService.GetTodoById(id);
            return JsonConvert.SerializeObject(new
            {
                resourceUri = $"todo://task/{id}",
                resourceType = "single",
                description = $"Task with ID {id}",
                data = todo
            });
        }
    }
}
