using System;

namespace TodoMcpServer
{
    public class HookLog
    {
        public int Id { get; set; }
        public string HookName { get; set; }
        public string HandlerName { get; set; }
        public string EventType { get; set; }
        public string Details { get; set; }
        public DateTime ExecutedAt { get; set; }
        public long ExecutionTimeMs { get; set; }
    }
}
