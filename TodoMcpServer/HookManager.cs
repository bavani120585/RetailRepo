using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TodoMcpServer
{
    public class HookManager
    {
        // Hook delegates
        public delegate void TaskCreatedHook(TodoItem task);
        public delegate void TaskCompletedHook(TodoItem task);
        public delegate void TaskDeletedHook(TodoItem task);
        public delegate void TaskUpdatedHook(TodoItem task);

        // Store handlers
        private List<TaskCreatedHook> taskCreatedHooks = new List<TaskCreatedHook>();
        private List<TaskCompletedHook> taskCompletedHooks = new List<TaskCompletedHook>();
        private List<TaskDeletedHook> taskDeletedHooks = new List<TaskDeletedHook>();
        private List<TaskUpdatedHook> taskUpdatedHooks = new List<TaskUpdatedHook>();

        // Store execution logs
        private List<HookLog> logs = new List<HookLog>();
        private int nextLogId = 1;

        // REGISTER HOOKS
        public void OnTaskCreated(TaskCreatedHook handler)
        {
            taskCreatedHooks.Add(handler);
        }

        public void OnTaskCompleted(TaskCompletedHook handler)
        {
            taskCompletedHooks.Add(handler);
        }

        public void OnTaskDeleted(TaskDeletedHook handler)
        {
            taskDeletedHooks.Add(handler);
        }

        public void OnTaskUpdated(TaskUpdatedHook handler)
        {
            taskUpdatedHooks.Add(handler);
        }

        // EXECUTE HOOKS with timing
        public void ExecuteTaskCreatedHooks(TodoItem task)
        {
            var stopwatch = Stopwatch.StartNew();
            foreach (var hook in taskCreatedHooks)
            {
                hook?.Invoke(task);
            }
            stopwatch.Stop();

            LogExecution("TaskCreated", "Multiple handlers", "task_created",
                        $"Executed {taskCreatedHooks.Count} handlers", stopwatch.ElapsedMilliseconds);
        }

        public void ExecuteTaskCompletedHooks(TodoItem task)
        {
            var stopwatch = Stopwatch.StartNew();
            foreach (var hook in taskCompletedHooks)
            {
                hook?.Invoke(task);
            }
            stopwatch.Stop();

            LogExecution("TaskCompleted", "Multiple handlers", "task_completed",
                        $"Executed {taskCompletedHooks.Count} handlers", stopwatch.ElapsedMilliseconds);
        }

        public void ExecuteTaskDeletedHooks(TodoItem task)
        {
            var stopwatch = Stopwatch.StartNew();
            foreach (var hook in taskDeletedHooks)
            {
                hook?.Invoke(task);
            }
            stopwatch.Stop();

            LogExecution("TaskDeleted", "Multiple handlers", "task_deleted",
                        $"Executed {taskDeletedHooks.Count} handlers", stopwatch.ElapsedMilliseconds);
        }

        public void ExecuteTaskUpdatedHooks(TodoItem task)
        {
            var stopwatch = Stopwatch.StartNew();
            foreach (var hook in taskUpdatedHooks)
            {
                hook?.Invoke(task);
            }
            stopwatch.Stop();

            LogExecution("TaskUpdated", "Multiple handlers", "task_updated",
                        $"Executed {taskUpdatedHooks.Count} handlers", stopwatch.ElapsedMilliseconds);
        }

        // LOG HOOK EXECUTION
        private void LogExecution(string hookName, string handlerName, string eventType,
                                  string details, long executionTimeMs)
        {
            var log = new HookLog
            {
                Id = nextLogId++,
                HookName = hookName,
                HandlerName = handlerName,
                EventType = eventType,
                Details = details,
                ExecutedAt = DateTime.Now,
                ExecutionTimeMs = executionTimeMs
            };

            logs.Add(log);
        }

        // GET LOGS
        public List<HookLog> GetAllLogs()
        {
            return logs;
        }

        public List<HookLog> GetLogsByEventType(string eventType)
        {
            return logs.FindAll(l => l.EventType == eventType);
        }

        public int GetHookCount(string hookName)
        {
            return hookName switch
            {
                "TaskCreated" => taskCreatedHooks.Count,
                "TaskCompleted" => taskCompletedHooks.Count,
                "TaskDeleted" => taskDeletedHooks.Count,
                "TaskUpdated" => taskUpdatedHooks.Count,
                _ => 0
            };
        }

        public Dictionary<string, int> GetAllHookCounts()
        {
            return new Dictionary<string, int>
            {
                { "TaskCreated", taskCreatedHooks.Count },
                { "TaskCompleted", taskCompletedHooks.Count },
                { "TaskDeleted", taskDeletedHooks.Count },
                { "TaskUpdated", taskUpdatedHooks.Count }
            };
        }
    }
}
