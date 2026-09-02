namespace GeradorChaveNFe.Dominio;

public sealed record Uf(string Codigo, string Sigla, string Nome)
{
    public string Descricao => $"{Codigo} - {Sigla} ({Nome})";
}

/// <summary>Catálogo estático das 27 UFs, indexado pelo código IBGE.</summary>
public static class CatalogoUf
{
    public static readonly IReadOnlyList<Uf> Todas = new List<Uf>
    {
        new("11", "RO", "Rondônia"),
        new("12", "AC", "Acre"),
        new("13", "AM", "Amazonas"),
        new("14", "RR", "Roraima"),
        new("15", "PA", "Pará"),
        new("16", "AP", "Amapá"),
        new("17", "TO", "Tocantins"),
        new("21", "MA", "Maranhão"),
        new("22", "PI", "Piauí"),
        new("23", "CE", "Ceará"),
        new("24", "RN", "Rio Grande do Norte"),
        new("25", "PB", "Paraíba"),
        new("26", "PE", "Pernambuco"),
        new("27", "AL", "Alagoas"),
        new("28", "SE", "Sergipe"),
        new("29", "BA", "Bahia"),
        new("31", "MG", "Minas Gerais"),
        new("32", "ES", "Espírito Santo"),
        new("33", "RJ", "Rio de Janeiro"),
        new("35", "SP", "São Paulo"),
        new("41", "PR", "Paraná"),
        new("42", "SC", "Santa Catarina"),
        new("43", "RS", "Rio Grande do Sul"),
        new("50", "MS", "Mato Grosso do Sul"),
        new("51", "MT", "Mato Grosso"),
        new("52", "GO", "Goiás"),
        new("53", "DF", "Distrito Federal"),
    };

    public static bool CodigoValido(string codigo) => Buscar(codigo) is not null;

    public static Uf? Buscar(string codigo) =>
        Todas.FirstOrDefault(uf => uf.Codigo == codigo);

    public static string Descrever(string codigo) => Buscar(codigo)?.Descricao ?? codigo;
}