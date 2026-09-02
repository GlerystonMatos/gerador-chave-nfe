using GeradorChaveNFe.Dominio;

namespace GeradorChaveNFe.Apresentacao;

/// <summary>Telas fixas: boas-vindas, carregamento, cabeçalho (repetido a cada passo) e despedida.</summary>
public static class Tela
{
    private const int Largura = 100;

    private static readonly string LinhaCheia = new('═', Largura);

    private const string Autor = "por Gleryston Matos";

    public static void BemVindo(string versao)
    {
        Limpar();
        Paleta.EscreverLinha(Borda('╔', '╗'), Paleta.Titulo);
        Paleta.EscreverLinha(LinhaCentralizada(""), Paleta.Titulo);
        Paleta.EscreverLinha(LinhaCentralizada("G E R A D O R   D E   C H A V E   N F - e"), Paleta.Titulo);
        Paleta.EscreverLinha(LinhaCentralizada(""), Paleta.Titulo);
        Paleta.EscreverLinha(LinhaCentralizada($"{Autor}   ·   versão {versao}"), Paleta.Titulo);
        Paleta.EscreverLinha(LinhaCentralizada(""), Paleta.Titulo);
        Paleta.EscreverLinha(Borda('╚', '╝'), Paleta.Titulo);
        Console.WriteLine();
    }

    public static void Carregando()
    {
        Paleta.Escrever(" Carregando ", Paleta.Sucesso);
        for (int i = 0; i < 20; i++)
        {
            Paleta.Escrever("▪", Paleta.Sucesso);
            Thread.Sleep(70);
        }
        Console.WriteLine();
        Console.WriteLine();
    }

    public static void Cabecalho(string versao, ParametrosChaveNfe parametros)
    {
        Limpar();
        Paleta.EscreverLinha(Borda('╔', '╗'), Paleta.Titulo);
        Paleta.EscreverLinha(LinhaTexto($" GERADOR DE CHAVE NF-e · {Autor} · v{versao}"), Paleta.Titulo);
        Paleta.EscreverLinha(Borda('╠', '╣'), Paleta.Titulo);
        Paleta.EscreverLinha(LinhaTexto(Resumo(parametros)), Paleta.Info);
        Paleta.EscreverLinha(Borda('╚', '╝'), Paleta.Titulo);
    }

    public static void Despedida(string versao)
    {
        BemVindo(versao);
        Paleta.EscreverLinha(LinhaCentralizada("Até a próxima!"), Paleta.Sucesso);
        Console.WriteLine();
        Thread.Sleep(1200);
        Console.ResetColor();
    }

    private static string Resumo(ParametrosChaveNfe p)
    {
        return $" UF: {Campo(CatalogoUf.Buscar(p.Uf)?.Sigla ?? p.Uf)}"
             + $" · AnoMês: {Campo(p.AnoMes)}"
             + $" · CNPJ: {Campo(p.Cnpj)}"
             + $" · Série: {Campo(p.Serie)}"
             + $" · Emissão: {Campo(p.TipoEmissao)}"
             + $" · Números: {Intervalo(p)}";
    }

    private static string Intervalo(ParametrosChaveNfe p)
    {
        if (p.NumeroInicial <= 0 && p.NumeroFinal <= 0)
            return "—";
        if (p.NumeroFinal < p.NumeroInicial)
            return $"{p.NumeroInicial:D3}…";
        return $"{p.NumeroInicial:D3}–{p.NumeroFinal:D3} ({p.QuantidadeDeChaves})";
    }

    private static string Campo(string valor) => string.IsNullOrWhiteSpace(valor) ? "—" : valor;

    private static string Borda(char inicio, char fim) => $"{inicio}{LinhaCheia}{fim}";

    private static string LinhaCentralizada(string texto) => $"║{Centralizar(texto)}║";

    private static string LinhaTexto(string texto) => $"║{Ajustar(texto)}║";

    private static string Centralizar(string texto)
    {
        if (texto.Length >= Largura)
            return texto[..Largura];

        int espacos = Largura - texto.Length;
        int esquerda = espacos / 2;
        return new string(' ', esquerda) + texto + new string(' ', espacos - esquerda);
    }

    private static string Ajustar(string texto) =>
        texto.Length > Largura ? texto[..(Largura - 1)] + "…" : texto.PadRight(Largura);

    private static void Limpar()
    {
        try
        {
            Console.Clear();
        }
        catch (IOException)
        {
            // Console.Clear lança quando a saída está redirecionada (pipe/arquivo/testes).
        }
    }
}