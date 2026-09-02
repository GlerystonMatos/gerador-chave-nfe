namespace GeradorChaveNFe.Dominio;

/// <summary>
/// Parâmetros escolhidos pelo usuário. A chave de acesso tem 44 dígitos, nesta ordem:
/// UF(2) + AnoMês(4) + CNPJ(14) + Modelo(2) + Série(3) + Número(9) + TipoEmissão(1) + CódigoNumérico(8) + DV(1).
/// </summary>
public sealed class ParametrosChaveNfe
{
    public const string Modelo = "55";

    public string Uf { get; set; } = "";

    public string AnoMes { get; set; } = "";

    public string Cnpj { get; set; } = "";

    public string Serie { get; set; } = "";

    public string TipoEmissao { get; set; } = "";

    public int NumeroInicial { get; set; }

    public int NumeroFinal { get; set; }

    public int QuantidadeDeChaves =>
        NumeroFinal >= NumeroInicial ? NumeroFinal - NumeroInicial + 1 : 0;
}