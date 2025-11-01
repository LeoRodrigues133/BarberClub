using BarberClub.WebApi.Filters;

namespace BarberClub.WebApi.Config;
public static class FilterController
{
    public static void FilterControllerConfiguration(this IServiceCollection _services)
    {
        _services.AddControllers(f =>
        {
            f.Filters.Add<ResponseWrapperFilter>();
        });
    }
}