using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Collections.Generic;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(consoleLogOptions =>
{
    // Configure all logs to go to stderr
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();
await builder.Build().RunAsync();

[McpServerToolType]
public static class EchoTool
{
    [McpServerTool, Description("Echoes the message back to the client.")]
    public static string Echo(string message) => $"hello {message}";

    [McpServerTool, Description("Echoes reversed the message back to the client.")]
    public static string Reverse(string message) => $"Reversed: {new string(message.Reverse().ToArray())}";

    [McpServerTool, Description("Reviews the given commit diff and returns a code review comment.")]
    public static string ReviewCommit(string diff)
    {
        // Basit örnek: TODO, fixme, var gibi anahtar kelimeleri uyarı olarak döndür
        var comments = new List<string>();
        if (diff.Contains("TODO", StringComparison.OrdinalIgnoreCase))
            comments.Add("Found TODO in code. Please address before merging.");
        if (diff.Contains("fixme", StringComparison.OrdinalIgnoreCase))
            comments.Add("Found FIXME in code. Please address before merging.");
        if (diff.Contains("var "))
            comments.Add("Consider using explicit types instead of 'var'.");
        if (comments.Count == 0)
            return "No issues found.";
        return string.Join(" ", comments);
    }
} 