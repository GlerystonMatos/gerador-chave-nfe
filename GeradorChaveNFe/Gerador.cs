using TextToAsciiArt;

namespace GeradorChaveNFe
{
    public static class Gerador
    {
        public static void Introducao()
        {
            IArtWriter writer = new ArtWriter();

            ArtSetting settings = new ArtSetting();
            settings.Text = "|";

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("------------------------------------------------------------------------------------\n");
            writer.WriteConsole("GERADOR", settings);
            Console.WriteLine("");
            writer.WriteConsole("CHAVE NFE", settings);
            Console.WriteLine("\n-----------------------------------------------------------------------------------");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("                                                               BY GLERYSTON MATOS    ");

            Carregando();
        }

        public static void Adeus()
        {
            IArtWriter writer = new ArtWriter();

            ArtSetting settings = new ArtSetting();
            settings.Text = "|";

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Clear();
            Console.WriteLine("------------------------------------------------------------------------------------\n");
            writer.WriteConsole("GERADOR", settings);
            Console.WriteLine("");
            writer.WriteConsole("CHAVE NFE", settings);
            Console.WriteLine("\n-----------------------------------------------------------------------------------");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("                                                               BY GLERYSTON MATOS    ");
            Console.WriteLine("ATÉ A PRÓXIMA ");

            Console.ResetColor();
        }

        public static void Carregando()
        {
            Console.ResetColor();

            Console.Write("\n  Carregando");
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
        }

        public static string ObterUF()
        {
            Dictionary<string, string> ufs = new Dictionary<string, string>
            {
                { "12", "AC - Acre" },
                { "27", "AL - Alagoas" },
                { "13", "AM - Amazonas" },
                { "16", "AP - Amapá" },
                { "29", "BA - Bahia" },
                { "23", "CE - Ceará" },
                { "53", "DF - Distrito Federal" },
                { "32", "ES - Espírito Santo" },
                { "52", "GO - Goiás" },
                { "21", "MA - Maranhão" },
                { "31", "MG - Minas Gerais" },
                { "50", "MS - Mato Grosso do Sul" },
                { "51", "MT - Mato Grosso" },
                { "15", "PA - Pará" },
                { "25", "PB - Paraíba" },
                { "26", "PE - Pernambuco" },
                { "22", "PI - Piauí" },
                { "41", "PR - Paraná" },
                { "33", "RJ - Rio de Janeiro" },
                { "24", "RN - Rio Grande do Norte" },
                { "43", "RS - Rio Grande do Sul" },
                { "11", "RO - Rondônia" },
                { "14", "RR - Roraima" },
                { "42", "SC - Santa Catarina" },
                { "35", "SP - São Paulo" },
                { "28", "SE - Sergipe" },
                { "17", "TO - Tocantins" }
            };

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Selecione a UF pelo código:");

            Console.ForegroundColor = ConsoleColor.Yellow;

            foreach (var uf in ufs)
                Console.WriteLine($"{uf.Key}: {uf.Value}");

            Console.ResetColor();

            Console.Write("Informe uma opção:");
            string codigoUF = Console.ReadLine();

            while (!ufs.ContainsKey(codigoUF))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Código da UF inválido. Por favor, selecione um código da UF válido:");

                Console.ForegroundColor = ConsoleColor.Yellow;

                foreach (var uf in ufs)
                    Console.WriteLine($"{uf.Key}: {uf.Value}");

                Console.ResetColor();

                Console.Write("Informe uma opção:");
                codigoUF = Console.ReadLine();
            }

            return codigoUF;
        }

        public static string ObterAnoMesEmissao()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Informe o Ano e Mês de Emissão (formato YYMM):");

            Console.ResetColor();

            Console.Write("YYMM:");
            string anoMes = Console.ReadLine();

            while (anoMes.Trim().Length < 4)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Ano e Mês de Emissão (formato YYMM) inválido. Por favor, informe um Ano e Mês de Emissão válido:");

                Console.ResetColor();

                Console.Write("YYMM:");
                anoMes = Console.ReadLine();
            }

            return anoMes;
        }

        public static string ObterCNPJ()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Informe o CNPJ do Emitente (14 dígitos):");

            Console.ResetColor();

            Console.Write("CNPJ:");
            string cnpj = Console.ReadLine();

            while (cnpj.Trim().Length < 14)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("CNPJ do Emitente (14 dígitos) inválido. Por favor, informe um CNPJ do Emitente válido:");

                Console.ResetColor();

                Console.Write("CNPJ:");
                cnpj = Console.ReadLine();
            }

            return cnpj;
        }

        public static string ObterSerie()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Informe a Série da NFe (ex.: 001):");

            Console.ResetColor();

            Console.Write("Série:");
            string serie = Console.ReadLine();

            while (serie.Trim().Length < 3)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Série da NFe (ex.: 001) inválida. Por favor, informe uma Série da NFe válida:");

                Console.ResetColor();

                Console.Write("Série:");
                serie = Console.ReadLine();
            }

            return serie;
        }

        public static string ObterTipoEmissao()
        {
            Dictionary<string, string> tiposEmissao = new Dictionary<string, string>
            {
                { "1", "Emissão Normal" },
                { "2", "Contingência" },
            };

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Selecione o Tipo de Emissão:");

            Console.ForegroundColor = ConsoleColor.Yellow;

            foreach (var uf in tiposEmissao)
                Console.WriteLine($"{uf.Key}: {uf.Value}");

            Console.ResetColor();

            Console.Write("Informe uma opção:");
            string tipoEmissao = Console.ReadLine();

            while (!tiposEmissao.ContainsKey(tipoEmissao))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Tipo de Emissão inválido. Por favor, selecione um Tipo de Emissão válido:");

                Console.ForegroundColor = ConsoleColor.Yellow;

                foreach (var uf in tiposEmissao)
                    Console.WriteLine($"{uf.Key}: {uf.Value}");

                Console.ResetColor();

                Console.Write("Informe uma opção:");
                tipoEmissao = Console.ReadLine();
            }

            return tipoEmissao;
        }

        public static int ObterNumeroInicialNFe()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Informe o Número Inicial da NFe:");

            Console.ResetColor();

            Console.Write("Inicial:");
            return int.Parse(Console.ReadLine());
        }

        public static int ObterNumeroFinalNFe(int numeroInicial)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Informe o Número Final da NFe (deve ser maior ou igual a {numeroInicial}):");

            Console.ResetColor();

            Console.Write("Final:");
            int numeroFinal = int.Parse(Console.ReadLine());

            while (numeroFinal < numeroInicial)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Número final inválido. Por favor, selecione um maior ou igual a {numeroInicial}:");

                Console.ResetColor();

                Console.Write("Final:");
                numeroFinal = int.Parse(Console.ReadLine());
            }

            return numeroFinal;
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