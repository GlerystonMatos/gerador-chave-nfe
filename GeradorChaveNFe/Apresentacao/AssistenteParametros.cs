using GeradorChaveNFe.Dominio;

namespace GeradorChaveNFe.Apresentacao;

/// <summary>
/// Coleta os sete parâmetros da chave, campo a campo. Cada campo segue o mesmo roteiro:
/// oferecer o valor salvo da última execução, pedir a entrada, validar e confirmar.
/// </summary>
public sealed class AssistenteParametros
{
    private readonly string _versao;

    public AssistenteParametros(string versao) => _versao = versao;

    public ParametrosChaveNfe Coletar(ParametrosChaveNfe atual, ParametrosChaveNfe? salvos)
    {
        atual.Uf = LerUf(atual, salvos?.Uf);
        atual.AnoMes = LerAnoMes(atual, salvos?.AnoMes);
        atual.Cnpj = LerCnpj(atual, salvos?.Cnpj);
        atual.Serie = LerSerie(atual, salvos?.Serie);
        atual.TipoEmissao = LerTipoEmissao(atual, salvos?.TipoEmissao);
        atual.NumeroInicial = LerNumeroInicial(atual, salvos?.NumeroInicial);
        atual.NumeroFinal = LerNumeroFinal(atual, salvos?.NumeroFinal, atual.NumeroInicial);
        return atual;
    }

    private string LerUf(ParametrosChaveNfe atual, string? salvo)
    {
        if (PodeReaproveitar(atual, "UF", salvo, CatalogoUf.CodigoValido, CatalogoUf.Descrever))
            return salvo!;

        return LerCampo(atual, "UF", "Informe o código da UF:",
            validar: codigo => CatalogoUf.CodigoValido(codigo) ? null : "Código de UF inválido. Escolha um da lista.",
            descrever: CatalogoUf.Descrever,
            opcoes: CatalogoUf.Todas.Select(uf => uf.Descricao),
            colunas: 3);
    }

    private string LerAnoMes(ParametrosChaveNfe atual, string? salvo)
    {
        if (PodeReaproveitar(atual, "Ano/Mês", salvo, v => Competencia.Validar(v) is null, Competencia.Descrever))
            return salvo!;

        return LerCampo(atual, "Ano/Mês", "Informe o ano e o mês de emissão (formato AAMM):",
            validar: Competencia.Validar,
            descrever: Competencia.Descrever);
    }

    private string LerCnpj(ParametrosChaveNfe atual, string? salvo)
    {
        if (PodeReaproveitar(atual, "CNPJ", salvo, v => ValidarCnpj(v) is null, valor => valor))
            return salvo!;

        return LerCampo(atual, "CNPJ", "Informe o CNPJ do emitente (14 dígitos):",
            validar: ValidarCnpj,
            descrever: valor => valor);
    }

    private string LerSerie(ParametrosChaveNfe atual, string? salvo)
    {
        if (PodeReaproveitar(atual, "Série", salvo, v => ValidarSerie(v) is null, valor => valor))
            return salvo!;

        return LerCampo(atual, "Série", "Informe a série da NF-e (3 dígitos, ex.: 001):",
            validar: ValidarSerie,
            descrever: valor => valor);
    }

    private string LerTipoEmissao(ParametrosChaveNfe atual, string? salvo)
    {
        if (PodeReaproveitar(atual, "Tipo de emissão", salvo, CatalogoTipoEmissao.CodigoValido, CatalogoTipoEmissao.Descrever))
            return salvo!;

        return LerCampo(atual, "Tipo de emissão", "Informe o código do tipo de emissão:",
            validar: codigo => CatalogoTipoEmissao.CodigoValido(codigo) ? null : "Tipo de emissão inválido. Escolha um da lista.",
            descrever: CatalogoTipoEmissao.Descrever,
            opcoes: CatalogoTipoEmissao.Todos.Select(tipo => tipo.Descricao));
    }

    // O número da NF-e ocupa 9 posições na chave (máx. 999.999.999).
    private const int NumeroMaximo = 999_999_999;

    // Teto de chaves geradas de uma vez, para não estourar a memória.
    private const int LoteMaximo = 100_000;

    private int LerNumeroInicial(ParametrosChaveNfe atual, int? salvo)
    {
        if (salvo is > 0 and <= NumeroMaximo && OfereceValorSalvo(atual, "Número inicial", $"{salvo:D3}"))
            return salvo.Value;

        string valor = LerCampo(atual, "Número inicial", "Informe o número inicial da NF-e (ex.: 1):",
            validar: texto => int.TryParse(texto, out int n) && n is >= 1 and <= NumeroMaximo
                ? null
                : $"Informe um número inteiro entre 1 e {NumeroMaximo}.",
            descrever: texto => $"{int.Parse(texto):D3}");
        return int.Parse(valor);
    }

    private int LerNumeroFinal(ParametrosChaveNfe atual, int? salvo, int numeroInicial)
    {
        if (salvo is int guardado && guardado >= numeroInicial && guardado <= NumeroMaximo &&
            guardado - numeroInicial + 1 <= LoteMaximo &&
            OfereceValorSalvo(atual, "Número final", $"{guardado:D3}"))
            return guardado;

        string valor = LerCampo(atual, "Número final", $"Informe o número final da NF-e (>= {numeroInicial:D3}):",
            validar: texto => ValidarNumeroFinal(texto, numeroInicial),
            descrever: texto => $"{int.Parse(texto):D3}");
        return int.Parse(valor);
    }

    private static string? ValidarNumeroFinal(string texto, int numeroInicial)
    {
        if (!int.TryParse(texto, out int numero) || numero < numeroInicial || numero > NumeroMaximo)
            return $"Informe um número inteiro entre {numeroInicial} e {NumeroMaximo}.";

        if (numero - numeroInicial + 1 > LoteMaximo)
            return $"O intervalo não pode passar de {LoteMaximo:N0} chaves.";

        return null;
    }

    /// <summary>Laço genérico: mostra o cabeçalho, lê, valida e confirma o valor de um campo.</summary>
    private string LerCampo(ParametrosChaveNfe atual, string rotulo, string instrucao, Func<string, string?> validar, Func<string, string> descrever, IEnumerable<string>? opcoes = null, int colunas = 1)
    {
        string? aviso = null;

        while (true)
        {
            Tela.Cabecalho(_versao, atual);
            if (aviso is not null)
                Paleta.EscreverLinha($"\n {aviso}", Paleta.Erro);

            string entrada = opcoes is null
                ? Prompt.Perguntar($"\n {instrucao}")
                : Prompt.EscolherOpcao($"\n {instrucao}", opcoes, colunas);

            string? erro = validar(entrada);
            if (erro is not null)
            {
                aviso = erro;
                continue;
            }

            Tela.Cabecalho(_versao, atual);
            if (Prompt.Confirmar($"\n Confirmar {rotulo}: {descrever(entrada)}?"))
                return entrada;

            aviso = "Valor descartado. Informe novamente.";
        }
    }

    private bool PodeReaproveitar(ParametrosChaveNfe atual, string rotulo, string? salvo, Func<string, bool> valido, Func<string, string> descrever)
        => !string.IsNullOrWhiteSpace(salvo) && valido(salvo) && OfereceValorSalvo(atual, rotulo, descrever(salvo));

    private bool OfereceValorSalvo(ParametrosChaveNfe atual, string rotulo, string descricao)
    {
        Tela.Cabecalho(_versao, atual);
        Paleta.EscreverLinha("\n Foram encontrados dados salvos da última execução.", Paleta.Info);
        return Prompt.Confirmar($"\n Reaproveitar {rotulo} ({descricao})?", padraoSim: true);
    }

    private static string? ValidarCnpj(string texto) =>
        texto.Length == 14 && texto.All(char.IsDigit) ? null : "O CNPJ deve ter 14 dígitos numéricos.";

    private static string? ValidarSerie(string texto) =>
        texto.Length == 3 && texto.All(char.IsDigit) ? null : "A série deve ter 3 dígitos (ex.: 001).";
}