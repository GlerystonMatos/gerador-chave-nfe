using GeradorChaveNFe.Dominio;
using System.Text;

namespace GeradorChaveNFe.Persistencia;

/// <summary>
/// Lê e grava os últimos parâmetros usados em um <c>GeradorChaveNFe.ini</c> ao lado do executável,
/// para que a próxima execução possa reaproveitá-los. A leitura é tolerante: campos
/// ausentes ou inválidos simplesmente não são preenchidos.
/// </summary>
public static class ArquivoParametrosIni
{
    private const string Secao = "ChaveNFe";

    private static readonly string Caminho = Path.Combine(AppContext.BaseDirectory, "GeradorChaveNFe.ini");

    public static ParametrosChaveNfe? Ler()
    {
        if (!File.Exists(Caminho))
            return null;

        Dictionary<string, string> valores = LerPares();
        ParametrosChaveNfe parametros = new()
        {
            Uf = Texto(valores, "Uf"),
            AnoMes = Texto(valores, "AnoMes"),
            Cnpj = Texto(valores, "Cnpj"),
            Serie = Texto(valores, "Serie"),
            TipoEmissao = Texto(valores, "TipoEmissao"),
            NumeroInicial = Inteiro(valores, "NumeroInicial"),
            NumeroFinal = Inteiro(valores, "NumeroFinal"),
        };

        return parametros;
    }

    public static void Salvar(ParametrosChaveNfe parametros)
    {
        StringBuilder conteudo = new();
        conteudo.AppendLine($"[{Secao}]");
        conteudo.AppendLine($"Uf={parametros.Uf}");
        conteudo.AppendLine($"AnoMes={parametros.AnoMes}");
        conteudo.AppendLine($"Cnpj={parametros.Cnpj}");
        conteudo.AppendLine($"Serie={parametros.Serie}");
        conteudo.AppendLine($"TipoEmissao={parametros.TipoEmissao}");
        conteudo.AppendLine($"NumeroInicial={parametros.NumeroInicial}");
        conteudo.AppendLine($"NumeroFinal={parametros.NumeroFinal}");

        File.WriteAllText(Caminho, conteudo.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
    }

    private static Dictionary<string, string> LerPares()
    {
        Dictionary<string, string> valores = new(StringComparer.OrdinalIgnoreCase);

        foreach (string linhaBruta in File.ReadAllLines(Caminho))
        {
            string linha = linhaBruta.Trim();
            if (linha.Length == 0 || linha.StartsWith('[') || linha.StartsWith(';') || linha.StartsWith('#'))
                continue;

            int separador = linha.IndexOf('=');
            if (separador <= 0)
                continue;

            valores[linha[..separador].Trim()] = linha[(separador + 1)..].Trim();
        }

        return valores;
    }

    private static string Texto(Dictionary<string, string> valores, string chave) =>
        valores.TryGetValue(chave, out string? valor) ? valor : "";

    private static int Inteiro(Dictionary<string, string> valores, string chave) =>
        valores.TryGetValue(chave, out string? valor) && int.TryParse(valor, out int numero) ? numero : 0;
}