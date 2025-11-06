namespace BarberClub.Dominio.Compartilhado;

public static class CpfValidator
{
    public static bool IsValid(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = cpf.Trim().Replace(".", "").Replace("-", "");

        if (cpf.Length != 11)
            return false;

        if (System.Text.RegularExpressions.Regex.IsMatch(cpf, @"^(\d)\1{10}$"))
            return false;

        for (int j = 9; j < 11; j++)
        {
            int soma = 0;
            for (int i = 0; i < j; i++)
                soma += int.Parse(cpf[i].ToString()) * (j + 1 - i);

            int resto = soma % 11;
            int digitoVerificador = (resto < 2) ? 0 : (11 - resto);

            if (digitoVerificador != int.Parse(cpf[j].ToString()))
                return false;
        }

        return true;
    }
}
