using FluentResults;
using MediatR;

namespace BarberClub.Aplicacao.ModuloServicos.Commands.Cadastrar;

public record CadastrarServicoRequest(
    string titulo,
    decimal valor,
    int? duracao,
    bool isPromocao,
    int porcentagemPromocao
    ) : IRequest<Result<CadastrarServicoResponse>>;