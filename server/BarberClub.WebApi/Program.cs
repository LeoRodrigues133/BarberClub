
using BarberClub.WebApi.Config;

namespace BarberClub.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        const string corsPolicyName = "BarberClubPolicy";

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.FilterControllerConfiguration();

        // falta serilog

        // Database provider [env SQLSERVER_CONNECTION_STRING]
        builder.Services.DbContextConfiguration(builder.Environment, builder.Configuration);

        // falta fluentValidations

        // Services
        builder.Services.RepositoriesConfiguration();
        builder.Services.MediatRConfiguration();


        // Authentication [env JWT_GENERATION_KEY, JWT_AUDIENCE_DOMAIN]
        builder.Services.AuthProviderConfiguration();
        builder.Services.JwtAuthenticationConfiguration(builder.Configuration);

        // Swagger e falta Api documentation 
        builder.Services.SwaggerConfiguration();
        // Cors
        builder.Services.CorsPolicyConfiguration(corsPolicyName);

        var app = builder.Build();

        app.UseGlobalExceptionExtension();
        app.AutoMigrateDatabase();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors(corsPolicyName);
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
