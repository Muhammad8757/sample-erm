using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        
        // Добавляем генерацию документации Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Authorization", Version = "v1" });
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(ApiKeyPolicy.PolicyName, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("X-API-Key");
            });
        });

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authorization V1");
            });
        }

        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }

    public static class ApiKeyPolicy
    {
        public const string PolicyName = "ApiKeyPolicy";
    }
}
