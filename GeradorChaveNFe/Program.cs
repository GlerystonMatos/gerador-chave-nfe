namespace GeradorChaveNFe
{
    public class Program
    {
        private static string _versao = "1.0.2.0";
        private static ChaveNFe _chaveNFe = new ChaveNFe();
        private static ChaveNFe _dadosChaveNFeSalvos = new ChaveNFe();

        public static void Main(string[] args)
        {
            Utils.Introducao(_versao);
            Iniciar();
            Utils.Adeus(_versao);
        }

        private static void Iniciar()
        {
            Utils.Topo(_versao, _chaveNFe);
            Console.WriteLine("\nInformações para geração das chaves\n");
            SelecionarInformacoes();

            Utils.Topo(_versao, _chaveNFe);
            while (!Utils.Confirmacao($"\nConfirmar a geração das chaves das NFe com as informações selecionadas?", ConsoleColor.Green).Equals("S"))
            {
                SelecionarInformacoes();
                Utils.Topo(_versao, _chaveNFe);
            }

            GeradorChave.Gerar(_versao, _chaveNFe);
            IniManager.SalvarConfiguracao(_chaveNFe);

            if (Utils.Confirmacao("\nDeseja gerar mais chaves de NFe?", ConsoleColor.Green).Equals("S"))
                Iniciar();
        }

        private static void SelecionarInformacoes()
        {
            _dadosChaveNFeSalvos = IniManager.LerConfiguracao();
            _chaveNFe.Uf = GeradorChave.SelecionarUf(_versao, _chaveNFe, _dadosChaveNFeSalvos);
            _chaveNFe.AnoMes = GeradorChave.SelecionarAnoMes(_versao, _chaveNFe, _dadosChaveNFeSalvos);
            _chaveNFe.Cnpj = GeradorChave.SelecionarCnpj(_versao, _chaveNFe, _dadosChaveNFeSalvos);
            _chaveNFe.Serie = GeradorChave.SelecionarSerie(_versao, _chaveNFe, _dadosChaveNFeSalvos);
            _chaveNFe.TipoEmissao = GeradorChave.SelecionarTipoEmissao(_versao, _chaveNFe, _dadosChaveNFeSalvos);
            _chaveNFe.NumeroInicial = GeradorChave.SelecionarNumeroInicial(_versao, _chaveNFe, _dadosChaveNFeSalvos);
            _chaveNFe.NumeroFinal = GeradorChave.SelecionarNumeroFinal(_versao, _chaveNFe, _dadosChaveNFeSalvos);
        }
    }
}