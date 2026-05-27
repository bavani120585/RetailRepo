using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoMcpServer
{
    public class TodoService
    {
        private List<TodoItem> todos = new List<TodoItem>();
        private int nextId = 1;

        public List<TodoItem> GetAllTodos()
        {
            return todos.OrderBy(t => t.CreatedDate).ToList();
        }

        public TodoItem GetTodoById(int id)
        {
            return todos.FirstOrDefault(t => t.Id == id);
        }

        public TodoItem CreateTodo(string title, string description)
        {
            var todo = new TodoItem
            {
                Id = nextId++,
                Title = title,
                Description = description,
                IsCompleted = false,
                CreatedDate = DateTime.Now
            };
            todos.Add(todo);
            return todo;
        }

        public TodoItem UpdateTodo(int id, string title, string description, bool isCompleted)
        {
            var todo = GetTodoById(id);
            if (todo != null)
            {
                todo.Title = title;
                todo.Description = description;
                todo.IsCompleted = isCompleted;
            }
            return todo;
        }

        public bool DeleteTodo(int id)
        {
            var todo = GetTodoById(id);
            if (todo != null)
            {
                todos.Remove(todo);
                return true;
            }
            return false;
        }

        public TodoItem CompleteTodo(int id)
        {
            var todo = GetTodoById(id);
            if (todo != null)
            {
                todo.IsCompleted = true;
            }
            return todo;
        }
    }
}
