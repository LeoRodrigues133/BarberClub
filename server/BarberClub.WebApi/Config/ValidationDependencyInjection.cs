using BarberClub.Dominio.ModuloFuncionario;
using FluentValidation;

namespace BarberClub.WebApi.Config;

public static class ValidationDependencyInjection
{
    public static void ConfigureFluentValidation(this IServiceCollection service)
    {
        service.AddValidatorsFromAssemblyContaining<ValidationFuncionario>();

    }
}