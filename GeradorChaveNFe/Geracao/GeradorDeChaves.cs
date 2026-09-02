using GeradorChaveNFe.Dominio;

namespace GeradorChaveNFe.Geracao;

/// <summary>Monta as chaves de acesso de NF-e para o intervalo de números escolhido.</summary>
public static class GeradorDeChaves
{
    public static IReadOnlyList<string> Gerar(ParametrosChaveNfe parametros)
    {
        List<string> chaves = new(parametros.QuantidadeDeChaves);

        for (int numero = parametros.NumeroInicial; numero <= parametros.NumeroFinal; numero++)
            chaves.Add(MontarChave(parametros, numero));

        return chaves;
    }

    private static string MontarChave(ParametrosChaveNfe parametros, int numeroNota)
    {
        string codigoNumerico = Random.Shared.Next(0, 100_000_000).ToString("D8");

        string chaveSemDigito = string.Concat(
            parametros.Uf,
            parametros.AnoMes,
            parametros.Cnpj,
            ParametrosChaveNfe.Modelo,
            parametros.Serie,
            numeroNota.ToString("D9"),
            parametros.TipoEmissao,
            codigoNumerico);

        return chaveSemDigito + DigitoVerificador.Calcular(chaveSemDigito);
    }
}