using Ganss.Xss;
using System.Text;

namespace UserService.Presentation.Middleware
{
    public class HtmlSanitizerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HtmlSanitizer _sanitizer;

        public HtmlSanitizerMiddleware(RequestDelegate next)
        {
            _next = next;
            _sanitizer = new HtmlSanitizer();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.ContentType != null &&
                context.Request.ContentType.Contains("application/json"))
            {
                context.Request.EnableBuffering();

                using var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 1024,
                    leaveOpen: true);

                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                if (!string.IsNullOrWhiteSpace(body))
                {
                    // Sanitize JSON string values
                    string sanitizedBody = SanitizeJsonStringValues(body);
                    var bytes = Encoding.UTF8.GetBytes(sanitizedBody);
                    context.Request.Body = new MemoryStream(bytes);
                }
            }

            await _next(context);
        }

        private string SanitizeJsonStringValues(string json)
        {
            // This is a basic implementation. For more complex JSON structures, use a JSON parser.
            var regex = new System.Text.RegularExpressions.Regex(@"""(.*?)"":\s*""(.*?)""", System.Text.RegularExpressions.RegexOptions.Compiled);
            return regex.Replace(json, m =>
            {
                string key = m.Groups[1].Value;
                string value = m.Groups[2].Value;
                string sanitized = _sanitizer.Sanitize(value);
                return $"\"{key}\":\"{sanitized}\"";
            });
        }
    }
}
