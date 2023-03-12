using System.Text;
using Lab1;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStatusCodePages(async (statusCodeContext) =>
{
    var response = statusCodeContext.HttpContext.Response;
    if (response.StatusCode == 404)
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("wwwroot/404.html");
    }
    else
    {
        await response.WriteAsync($"Error. Status code: {response.StatusCode}");
    }
});
app.UseLogger();

app.MapGet("/", () => "Hello World!");
app.Map("/home/index", () => "Home / Index");
app.Map("/home/about", () => "Home / About");
app.UseWhen(context => context.Request.Path.Value.Contains("/secret-"),
    (app) => app.UseSecret());
app.UseFileServer();
app.UseFileServer(new FileServerOptions
{
    EnableDirectoryBrowsing = true,
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @"static"))
});
app.Run();

