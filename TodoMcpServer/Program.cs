using TodoMcpServer;

class Program
{
    static void Main(string[] args)
    {
        var server = new McpTodoServer();
        server.Start();
    }
}
