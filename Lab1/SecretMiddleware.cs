using System.Linq;
using System.Text.RegularExpressions;
using Lab1;

namespace Lab1
{
    public class SecretMiddleware
    {
        private readonly RequestDelegate next;
        List<KeyValuePair<int, string>> dummy_secrets = new List<KeyValuePair<int, string>>()
        {
            new KeyValuePair<int, string>(32432554, "Secret 1 message"),
            new KeyValuePair<int, string>(83274893, "Secret 2 message")
        };

        public SecretMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            int key = ParseKey(context.Request.Path.ToString());
            if (key != 0)
            {
                foreach (var keyValuePair in dummy_secrets)
                {
                    if (keyValuePair.Key == key)
                    {
                        await context.Response.WriteAsync(keyValuePair.Value);
                        return;
                    }
                }
            }

            await next.Invoke(context);

        }

        private static int ParseKey(string path)
        {
            int result = 0;
            Regex keyRegex = new Regex(@"\/secret-([\d]{8})$");
            MatchCollection matches = keyRegex.Matches(path);
            if (matches.Count != 0)
            {
                result = int.Parse(matches[0].Groups[1].Value);
            }
            return result;
        }
    }

}

public static class SecretExtensions
{
    public static IApplicationBuilder UseSecret(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecretMiddleware>();
    }
}