namespace GeradorChaveNFe.Apresentacao;

/// <summary>Padrão de cores do console; cada cor tem um papel fixo na interface.</summary>
public static class Paleta
{
    public const ConsoleColor Titulo = ConsoleColor.Cyan;

    public const ConsoleColor Destaque = ConsoleColor.Yellow;

    public const ConsoleColor Sucesso = ConsoleColor.Green;

    public const ConsoleColor Erro = ConsoleColor.Red;

    public const ConsoleColor Info = ConsoleColor.DarkCyan;

    public const ConsoleColor Neutro = ConsoleColor.Gray;

    public static void Escrever(string texto, ConsoleColor cor)
    {
        Console.ForegroundColor = cor;
        Console.Write(texto);
        Console.ForegroundColor = Neutro;
    }

    public static void EscreverLinha(string texto, ConsoleColor cor)
    {
        Console.ForegroundColor = cor;
        Console.WriteLine(texto);
        Console.ForegroundColor = Neutro;
    }
}