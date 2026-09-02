using System.Globalization;

namespace GeradorChaveNFe.Dominio;

/// <summary>
/// Ano e mês de emissão da NF-e no formato AAMM (ex.: "2509" = setembro de 2025).
/// </summary>
public static class Competencia
{
    private static readonly CultureInfo PtBr = CultureInfo.GetCultureInfo("pt-BR");

    /// <summary>Valida o texto AAMM. Retorna a mensagem de erro, ou <c>null</c> quando válido.</summary>
    public static string? Validar(string aamm)
    {
        if (aamm.Length != 4 || !aamm.All(char.IsDigit))
            return "O ano/mês deve ter 4 dígitos no formato AAMM.";

        int mes = int.Parse(aamm[2..]);
        if (mes is < 1 or > 12)
            return "O mês (dois últimos dígitos) deve estar entre 01 e 12.";

        return null;
    }

    /// <summary>Converte "2509" em "Setembro de 2025". Assume texto já validado.</summary>
    public static string PorExtenso(string aamm)
    {
        int ano = 2000 + int.Parse(aamm[..2]);
        int mes = int.Parse(aamm[2..]);

        string nomeMes = PtBr.DateTimeFormat.GetMonthName(mes);
        return $"{char.ToUpper(nomeMes[0])}{nomeMes[1..]} de {ano}";
    }

    /// <summary>Descrição amigável ("2509 — Setembro de 2025"), tolerante a valores inválidos.</summary>
    public static string Descrever(string aamm) =>
        Validar(aamm) is null ? $"{aamm} — {PorExtenso(aamm)}" : aamm;
}