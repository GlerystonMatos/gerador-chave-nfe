using GeradorChaveNFe.Apresentacao;
using GeradorChaveNFe.Dominio;
using GeradorChaveNFe.Geracao;
using GeradorChaveNFe.Persistencia;
using System.Text;

namespace GeradorChaveNFe;

public static class Program
{
    private const string Versao = "2.0.0.0";

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Tela.BemVindo(Versao);
        Tela.Carregando();

        GerarChavesEnquantoUsuarioQuiser();

        Tela.Despedida(Versao);
    }

    private static void GerarChavesEnquantoUsuarioQuiser()
    {
        AssistenteParametros assistente = new(Versao);
        bool continuar = true;

        while (continuar)
        {
            ParametrosChaveNfe parametros = ColetarParametrosConfirmados(assistente);

            IReadOnlyList<string> chaves = GeradorDeChaves.Gerar(parametros);
            ExibirChaves(parametros, chaves);
            SalvarParametros(parametros);

            continuar = Prompt.Confirmar("\n Deseja gerar mais chaves de NF-e?");
        }
    }

    private static ParametrosChaveNfe ColetarParametrosConfirmados(AssistenteParametros assistente)
    {
        while (true)
        {
            ParametrosChaveNfe parametros = new();
            ParametrosChaveNfe? salvos = ArquivoParametrosIni.Ler();

            Tela.Cabecalho(Versao, parametros);
            Paleta.EscreverLinha("\n Informe os parâmetros para a geração das chaves.", Paleta.Neutro);

            assistente.Coletar(parametros, salvos);

            Tela.Cabecalho(Versao, parametros);
            if (Prompt.Confirmar("\n Confirmar a geração das chaves com os parâmetros acima?", padraoSim: true))
                return parametros;
        }
    }

    private static void ExibirChaves(ParametrosChaveNfe parametros, IReadOnlyList<string> chaves)
    {
        Tela.Cabecalho(Versao, parametros);
        Paleta.EscreverLinha($"\n {chaves.Count} chave(s) gerada(s):\n", Paleta.Sucesso);

        foreach (string chave in chaves)
            Paleta.EscreverLinha($"  {chave}", Paleta.Neutro);
    }

    private static void SalvarParametros(ParametrosChaveNfe parametros)
    {
        try
        {
            ArquivoParametrosIni.Salvar(parametros);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Paleta.EscreverLinha("\n Não foi possível salvar os parâmetros para a próxima execução.", Paleta.Erro);
        }
    }
}