using System.Globalization;
using TextToAsciiArt;

namespace GeradorChaveNFe
{
    public static class Utils
    {
        public static void Introducao(string versao)
        {
            BemVindo(versao);
            Carregando();
        }

        public static void BemVindo(string versao)
        {
            IArtWriter writer = new ArtWriter();

            ArtSetting settings = new ArtSetting();
            settings.Text = "|";

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("---------------------------------------------------------------------------------------------------\n");
            writer.WriteConsole("GERADOR DE", settings);
            Console.WriteLine("");
            writer.WriteConsole("CHAVE DE NFE", settings);
            Console.WriteLine("\n---------------------------------------------------------------------------------------------------");
            Console.WriteLine($" BY GLERYSTON MATOS | VERSÃO {versao} |                                                            |");
            Console.WriteLine("---------------------------------------------------------------------------------------------------");
        }

        private static void Carregando()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Console.Write(" Carregando");
            Thread.Sleep(500);
            Console.Write(".");
            Thread.Sleep(500);
            Console.Write(".");
            Thread.Sleep(500);
            Console.Write(".");
            Thread.Sleep(500);
            Console.Write(".");
            Thread.Sleep(500);
            Console.Write(".");
            Thread.Sleep(500);
            Console.Write(".");
            Thread.Sleep(500);
            Console.Write(".");
            Thread.Sleep(500);
            Console.Write(".");

            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        public static void Adeus(string versao)
        {
            BemVindo(versao);

            Console.WriteLine(" ATÉ A PRÓXIMA                                                                                    |");
            Console.WriteLine("---------------------------------------------------------------------------------------------------");

            Console.ForegroundColor = ConsoleColor.Gray;
            Thread.Sleep(2000);
        }

        public static void Topo(string versao, ChaveNFe chaveNFe)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($" GERADOR CHAVE NFE | BY GLERYSTON MATOS | VERSÃO {versao} |                                                           |");
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($" UF: {chaveNFe.Uf} | YYMM: {chaveNFe.AnoMes} | CNPJ: {chaveNFe.Cnpj} | SÉRIE: {chaveNFe.Serie} | TIPO EMISSÃO: {chaveNFe.TipoEmissao} | NÚMERO INICIAL: {int.Parse(chaveNFe.NumeroInicial):D3} | NÚMERO FINAL: {int.Parse(chaveNFe.NumeroFinal):D3} |");
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
        }

        public static string Confirmacao(string pergunta, ConsoleColor foregroundColor = ConsoleColor.Gray)
        {
            Console.ForegroundColor = foregroundColor;

            Console.Write($"{pergunta} (S/N): ");
            string resposta = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            return resposta.ToUpper();
        }

        public static string Pergunta(string pergunta)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{pergunta} ");
            string resposta = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            return resposta;
        }

        public static string ObterNomePorCodigo(string codigo, Dictionary<string, string> dictionary)
        {
            string nome = "";
            dictionary.TryGetValue(codigo, out nome);
            return nome.Trim();
        }

        public static string ObterAnoMesPorExtenso(string yymm, int anoParcial, int mes)
        {
            int anoCompleto = 2000 + anoParcial;

            DateTime data = new DateTime(anoCompleto, mes, 1);
            string mesPorExtenso = CultureInfo.GetCultureInfo("pt-BR").DateTimeFormat.GetMonthName(mes);

            return $"{char.ToUpper(mesPorExtenso[0]) + mesPorExtenso.Substring(1)} de {anoCompleto}";
        }

        public static int CalcularDigitoVerificador(string chaveSemDV)
        {
            int soma = 0;
            int peso = 2;

            for (int i = chaveSemDV.Length - 1; i >= 0; i--)
            {
                int num = int.Parse(chaveSemDV[i].ToString());
                soma += num * peso;

                peso++;
                if (peso > 9) peso = 2;
            }

            int resto = soma % 11;
            int dv = 11 - resto;

            if (dv >= 10)
                dv = 0;

            return dv;
        }
    }
}