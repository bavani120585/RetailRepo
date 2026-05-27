using System;
using Newtonsoft.Json;

namespace TodoMcpServer
{
    public class McpTools
    {
        private TodoService todoService;

        public McpTools(TodoService todoService)
        {
            this.todoService = todoService;
        }

        public string CreateTaskTool(string title, string description)
        {
            var todo = todoService.CreateTodo(title, description);
            return JsonConvert.SerializeObject(new
            {
                toolName = "create_task",
                success = true,
                message = $"Task created with ID {todo.Id}",
                result = todo
            });
        }

        public string UpdateTaskTool(int id, string title, string description, bool isCompleted)
        {
            var todo = todoService.UpdateTodo(id, title, description, isCompleted);
            return JsonConvert.SerializeObject(new
            {
                toolName = "update_task",
                success = todo != null,
                message = todo != null ? "Task updated" : "Task not found",
                result = todo
            });
        }

        public string DeleteTaskTool(int id)
        {
            bool deleted = todoService.DeleteTodo(id);
            return JsonConvert.SerializeObject(new
            {
                toolName = "delete_task",
                success = deleted,
                message = deleted ? $"Task {id} deleted" : "Task not found",
                taskId = id
            });
        }

        public string CompleteTaskTool(int id)
        {
            var todo = todoService.CompleteTodo(id);
            return JsonConvert.SerializeObject(new
            {
                toolName = "complete_task",
                success = todo != null,
                message = todo != null ? "Task marked complete" : "Task not found",
                result = todo
            });
        }

        public string GetAllTasksTool()
        {
            var todos = todoService.GetAllTodos();
            return JsonConvert.SerializeObject(new
            {
                toolName = "get_all_tasks",
                success = true,
                message = $"Retrieved {todos.Count} tasks",
                result = todos
            });
        }
    }
}
