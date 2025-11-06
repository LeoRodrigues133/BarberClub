using BarberClub.Dominio.Compartilhado;
using FluentValidation;

namespace BarberClub.Dominio.ModuloFuncionario;

public class ValidationFuncionario : AbstractValidator<Funcionario>
{
    public ValidationFuncionario()
    {
        RuleFor(x => x.Cpf)
            .NotEmpty()
            .WithMessage("O {PropertyName} é obrigatório.")
            .Must(CpfValidator.IsValid)
            .WithMessage("O {PropertyName} informado é inválido.");

        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é obrigatório.")
            .MinimumLength(3)
            .MaximumLength(50);
    }
}