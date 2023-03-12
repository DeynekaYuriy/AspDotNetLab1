using System.Reflection.Metadata.Ecma335;

namespace Lab1;

public class LoggerMiddleware
{
    private readonly RequestDelegate next;

    public LoggerMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
         await File.AppendAllTextAsync("access.txt", $"{DateTime.Now} {context.Request.Path}\n");
         await next.Invoke(context);
    }
}

public static class LoggerExtensions
{
    public static IApplicationBuilder UseLogger(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggerMiddleware>();
    }
}