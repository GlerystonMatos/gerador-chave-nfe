namespace GeradorChaveNFe.Dominio;

public sealed record TipoEmissao(string Codigo, string Nome)
{
    public string Descricao => $"{Codigo} - {Nome}";
}

/// <summary>Catálogo dos tipos de emissão suportados pelo gerador.</summary>
public static class CatalogoTipoEmissao
{
    public static readonly IReadOnlyList<TipoEmissao> Todos = new List<TipoEmissao>
    {
        new("1", "Emissão normal"),
        new("2", "Contingência"),
    };

    public static bool CodigoValido(string codigo) => Buscar(codigo) is not null;

    public static TipoEmissao? Buscar(string codigo) =>
        Todos.FirstOrDefault(tipo => tipo.Codigo == codigo);

    public static string Descrever(string codigo) => Buscar(codigo)?.Descricao ?? codigo;
}