using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace BarberClub.WebApi.Config;
public static class GlobalExceptionHandler
{
    public static IApplicationBuilder UseGlobalExceptionExtension(this IApplicationBuilder app)
    {
        return app.UseExceptionHandler(builder =>
        {
            builder.Run(async httpContext =>
            {
                var manageExceptions = httpContext.Features.Get<IExceptionHandlerFeature>();

                if (manageExceptions is null) return;

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var obj = new
                {
                    Success = false,
                    Errors = new string[] { "Erro interno do servidor" }
                };

                var jsonResponse = JsonSerializer.Serialize(obj);

                await httpContext.Response.WriteAsync(jsonResponse);
            });
        });
    }
}