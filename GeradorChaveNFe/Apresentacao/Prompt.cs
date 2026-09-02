namespace GeradorChaveNFe.Apresentacao;

/// <summary>Entrada de dados pelo console, com as cores do padrão aplicadas.</summary>
public static class Prompt
{
    public static string Perguntar(string pergunta)
    {
        Paleta.Escrever($"{pergunta} ", Paleta.Neutro);
        Console.ForegroundColor = Paleta.Destaque;
        string resposta = LerEntrada();
        Console.ForegroundColor = Paleta.Neutro;
        return resposta.Trim();
    }

    /// <summary>Pergunta S/N; Enter sem texto devolve <paramref name="padraoSim"/>.</summary>
    public static bool Confirmar(string pergunta, bool padraoSim = false)
    {
        string sufixo = padraoSim ? "[S/n]" : "[s/N]";
        Paleta.Escrever($"{pergunta} {sufixo}: ", Paleta.Sucesso);
        Console.ForegroundColor = Paleta.Destaque;
        string resposta = LerEntrada().Trim().ToLowerInvariant();
        Console.ForegroundColor = Paleta.Neutro;

        if (resposta.Length == 0)
            return padraoSim;

        return resposta is "s" or "sim" or "y" or "yes";
    }

    /// <summary>Mostra as opções ("codigo - descrição") em colunas e lê o código escolhido.</summary>
    public static string EscolherOpcao(string pergunta, IEnumerable<string> opcoes, int colunas = 1)
    {
        Console.WriteLine();
        Paleta.EscreverLinha(" Opções:", Paleta.Neutro);
        Console.WriteLine();

        int coluna = 0;
        foreach (string opcao in opcoes)
        {
            Paleta.Escrever($"  {opcao.PadRight(30)}", Paleta.Info);
            if (++coluna % colunas == 0)
                Console.WriteLine();
        }
        if (coluna % colunas != 0)
            Console.WriteLine();

        return Perguntar(pergunta);
    }

    private static string LerEntrada()
    {
        string? linha = Console.ReadLine();
        if (linha is not null)
            return linha;

        // stdin encerrado (EOF): não há como continuar interagindo.
        Console.ResetColor();
        Console.WriteLine();
        Environment.Exit(0);
        return "";
    }
}