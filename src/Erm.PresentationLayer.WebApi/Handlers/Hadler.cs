using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        string? apiKey = context.Request.Headers["X-API-Key"];

        // Проверка наличия и валидности API-ключа
        if (apiKey != "Authorization") // Здесь должна быть реальная логика проверки
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Invalid API key");
            return;
        }

        await _next(context);
    }
}