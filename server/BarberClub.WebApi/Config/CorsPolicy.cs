namespace BarberClub.WebApi.Config;
public static class CorsPolicy
{
    public static void CorsPolicyConfiguration(this IServiceCollection _services, string policyName)
    {
        _services.AddCors(opt =>
        {
            opt.AddPolicy(name: policyName, policy =>
            {
                policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
    }
}