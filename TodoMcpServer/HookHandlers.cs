using System;

namespace TodoMcpServer
{
    public class HookHandlers
    {
        // HANDLER 1: Log task operations
        public static void LogTaskOperation(TodoItem task)
        {
            Console.WriteLine($"[Hook: Logger] Task {task.Id} - {task.Title} logged");
        }

        // HANDLER 2: Send notifications
        public static void NotifyTaskOperation(TodoItem task)
        {
            Console.WriteLine($"[Hook: Notifier] Notification sent for task {task.Id}");
        }

        // HANDLER 3: Update dashboard
        public static void UpdateDashboard(TodoItem task)
        {
            Console.WriteLine($"[Hook: Dashboard] Dashboard updated for task {task.Id}");
        }

        // HANDLER 4: Calculate statistics
        public static void CalculateStatistics(TodoItem task)
        {
            Console.WriteLine($"[Hook: Statistics] Stats updated for task {task.Id}");
        }

        // HANDLER 5: Archive completed tasks
        public static void ArchiveCompleted(TodoItem task)
        {
            if (task.IsCompleted)
            {
                Console.WriteLine($"[Hook: Archive] Task {task.Id} archived");
            }
        }
    }
}
