using FluentResults;

namespace BarberClub.Aplicacao.Compartilhado;

public class ErrorsResult
{
    public static Error BadRequestError(List<string> erros)
    {
        return new Error("Requisição mal formatada.")
            .CausedBy(erros)
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error NotFoundError(Guid id)
    {
        return new Error("Registro não encontrado")
            .CausedBy($"Não foi possível encontrar o registro ID {id}")
            .WithMetadata("ErrorType", "NotFound");
    }

    public static Error InternalServerError(Exception ex)
    {
        return new Error("Erro interno do servidor")
            .CausedBy(ex)
            .WithMetadata("ErrorType", "InternalServer");
    }
}
