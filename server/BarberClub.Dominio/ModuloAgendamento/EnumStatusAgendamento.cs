namespace BarberClub.Dominio.ModuloAgendamento;

public enum EnumStatusAgendamento
{
    Agendado = 0,
    Confirmado = 1, // Pago ou Cliente na loja
    Concluido = 2,
    Cancelado = 3,
    NaoCompareceu = 4
}