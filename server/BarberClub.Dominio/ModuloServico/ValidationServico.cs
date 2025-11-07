using FluentValidation;

namespace BarberClub.Dominio.ModuloServico;

public class ValidationServico : AbstractValidator<Servico>
{
    public ValidationServico()
    {
        RuleFor(x => x.FuncionarioId)
            .NotNull()
            .WithMessage("O funcionário é obrigatório.");

        RuleFor(x => x.Titulo)
          .NotEmpty()
            .WithMessage("O campo {PropertyName} é obrigatório.")
            .MinimumLength(3)
            .WithMessage("O campo {PropertyName} deve conter no mínimo {MinLength} caracteres.")
            .MaximumLength(30)
            .WithMessage("O campo {PropertyName} deve conter no máximo {MaxLength} caracteres.");

        RuleFor(x => x.Valor)
            .GreaterThan(0)
            .WithMessage("O {PropertyName} deve ser maior que zero.")
            .LessThanOrEqualTo(1000)
            .WithMessage("O {PropertyName} não pode exceder R$ 1.000.");

            RuleFor(x => x.Duracao)
                .GreaterThanOrEqualTo(10)
                .WithMessage("A {PropertyName} deve ser igual ou maior que 10 minutos.")
                .LessThanOrEqualTo(120)
                .WithMessage("A {PropertyName} não pode exceder 2 horas.");


        When(x => x.IsPromocao, () =>
        {
            RuleFor(x => x.PorcentagemPromocao)
                .NotNull()
                .WithMessage("A porcentagem de promoção é obrigatória quando o serviço está em promoção.")
                .InclusiveBetween(1, 100)
                .WithMessage("A porcentagem deve estar entre 1% e 100%.");
        });

        When(x => !x.IsPromocao, () =>
        {
            RuleFor(x => x.PorcentagemPromocao)
                .Must(p => !p.HasValue)
                .WithMessage("A porcentagem deve ser nula quando o serviço não está em promoção.");
        });
    }
}