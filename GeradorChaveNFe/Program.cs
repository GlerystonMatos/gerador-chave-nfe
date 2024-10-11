namespace GeradorChaveNFe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Gerador.Introducao();
            Iniciar();
            Continuar();
            Gerador.Adeus();
        }

        public static void Iniciar()
        {
            string uf = Gerador.ObterUF();
            string anoMesEmissao = Gerador.ObterAnoMesEmissao();
            string cnpj = Gerador.ObterCNPJ();
            string modeloNotaFiscalEletrônica = "55";
            string serie = Gerador.ObterSerie();
            string tipoEmissao = Gerador.ObterTipoEmissao();
            int numeroInicialNFe = Gerador.ObterNumeroInicialNFe();
            int numeroFinalNFe = Gerador.ObterNumeroFinalNFe(numeroInicialNFe);

            IList<string> chavesGeradas = new List<string>();

            for (int numeroAtualNFe = numeroInicialNFe; numeroAtualNFe <= numeroFinalNFe; numeroAtualNFe++)
            {
                string numeroNFe = numeroAtualNFe.ToString("D9");
                string codigoNumerico = new Random().Next(1, 99999999).ToString("D8");

                string chaveSemDV = uf + anoMesEmissao + cnpj + modeloNotaFiscalEletrônica + serie + numeroNFe + tipoEmissao + codigoNumerico;
                int dv = Gerador.CalcularDigitoVerificador(chaveSemDV);

                string chaveAcesso = chaveSemDV + dv;
                chavesGeradas.Add(chaveAcesso);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nChaves de Acesso Geradas:");

            foreach (var chave in chavesGeradas)
                Console.WriteLine(chave);

            Console.ResetColor();
        }

        public static void Continuar()
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Deseja gerar mais NFe?");

            Console.ResetColor();

            Console.Write("(Y/N):");
            string opcao = Console.ReadLine();

            while (opcao.ToUpper().Equals("Y"))
            {
                Iniciar();

                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Deseja gerar mais NFe?");

                Console.ResetColor();

                Console.Write("(Y/N):");
                opcao = Console.ReadLine();
            }
        }
    }
}