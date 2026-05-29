using System;
using Newtonsoft.Json;

namespace TodoMcpServer
{
    public class McpTools
    {
        private TodoService todoService;
        private HookManager hookManager;

        public McpTools(TodoService todoService, HookManager hookManager)
        {
            this.todoService = todoService;
            this.hookManager = hookManager;
        }

        public string CreateTaskTool(string title, string description)
        {
            var todo = todoService.CreateTodo(title, description);
            hookManager.ExecuteTaskCreatedHooks(todo);
            return JsonConvert.SerializeObject(new
            {
                toolName = "create_task",
                success = true,
                message = $"Task created with ID {todo.Id}. Hooks fired.",
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
            var todo = todoService.GetTodoById(id);
            bool deleted = todoService.DeleteTodo(id);
            if (deleted && todo != null)
            {
                hookManager.ExecuteTaskDeletedHooks(todo);
            }
            return JsonConvert.SerializeObject(new
            {
                toolName = "delete_task",
                success = deleted,
                message = deleted ? "Task deleted. Hooks fired." : "Task not found",
                taskId = id
            });
        }

        public string CompleteTaskTool(int id)
        {
            var todo = todoService.CompleteTodo(id);
            if (todo != null)
            {
                hookManager.ExecuteTaskCompletedHooks(todo);
            }
            return JsonConvert.SerializeObject(new
            {
                toolName = "complete_task",
                success = todo != null,
                message = todo != null ? "Task marked complete. Hooks fired." : "Task not found",
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
