namespace GeradorChaveNFe
{
    public static class GeradorChave
    {
        private static Dictionary<string, string> _ufs = new Dictionary<string, string>
        {
            { "12", "AC - Acre                " },
            { "27", "AL - Alagoas             " },
            { "13", "AM - Amazonas            " },
            { "16", "AP - Amapá               " },
            { "29", "BA - Bahia               " },
            { "23", "CE - Ceará               " },
            { "53", "DF - Distrito Federal    " },
            { "32", "ES - Espírito Santo      " },
            { "52", "GO - Goiás               " },
            { "21", "MA - Maranhão            " },
            { "31", "MG - Minas Gerais        " },
            { "50", "MS - Mato Grosso do Sul  " },
            { "51", "MT - Mato Grosso         " },
            { "15", "PA - Pará                " },
            { "25", "PB - Paraíba             " },
            { "26", "PE - Pernambuco          " },
            { "22", "PI - Piauí               " },
            { "41", "PR - Paraná              " },
            { "33", "RJ - Rio de Janeiro      " },
            { "24", "RN - Rio Grande do Norte " },
            { "43", "RS - Rio Grande do Sul   " },
            { "11", "RO - Rondônia            " },
            { "14", "RR - Roraima             " },
            { "42", "SC - Santa Catarina      " },
            { "35", "SP - São Paulo           " },
            { "28", "SE - Sergipe             " },
            { "17", "TO - Tocantins           " }
        };

        private static Dictionary<string, string> _tiposEmissao = new Dictionary<string, string>
        {
            { "1", "Emissão Normal" },
            { "2", "Contingência" },
        };

        public static string SelecionarUf(string versao, ChaveNFe chaveNFe, ChaveNFe dadosChaveNFeSalvos)
        {
            Utils.Topo(versao, chaveNFe);
            if (dadosChaveNFeSalvos != null)
            {
                Console.WriteLine("\nForam identificados dados salvos da última geração");
                if (Utils.Confirmacao($"\nConfirmar UF ({dadosChaveNFeSalvos.Uf}: {Utils.ObterNomePorCodigo(dadosChaveNFeSalvos.Uf, _ufs)}) salva?", ConsoleColor.Green).Equals("S"))
                    return dadosChaveNFeSalvos.Uf;
            }

            Utils.Topo(versao, chaveNFe);
            string codigoUfSelecionada = ObterUfSelecionada();

            bool ufValida = false;
            while (!ufValida)
            {
                while (!_ufs.ContainsKey(codigoUfSelecionada))
                {
                    Utils.Topo(versao, chaveNFe);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nOpção selecionada invalida, selecione uma das opções disponíveis!");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    codigoUfSelecionada = ObterUfSelecionada();
                }

                ufValida = true;
                while (!Utils.Confirmacao($"\nConfirmar UF ({codigoUfSelecionada}: {Utils.ObterNomePorCodigo(codigoUfSelecionada, _ufs)}) selecionada?", ConsoleColor.Green).Equals("S"))
                {
                    ufValida = false;
                    Utils.Topo(versao, chaveNFe);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInformação descartada!");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    codigoUfSelecionada = ObterUfSelecionada();

                    break;
                }
            }

            return codigoUfSelecionada;
        }

        public static string ObterUfSelecionada()
        {
            Console.WriteLine("\nOpções:\n");

            int contador = 0;
            foreach (var opcao in _ufs)
            {
                if (!string.IsNullOrEmpty(opcao.Key))
                {
                    contador += 1;
                    if (contador < 3)
                        Console.Write($"{opcao.Key}: {opcao.Value} | ");
                    else
                    {
                        contador = 1;
                        Console.Write("\n");
                        Console.Write($"{opcao.Key}: {opcao.Value} | ");
                    }
                }
                else
                    Console.Write(" ");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\n\nInforme uma das opções disponíveis: ");
            string codigoUfSelecionada = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            return codigoUfSelecionada;
        }

        public static string SelecionarAnoMes(string versao, ChaveNFe chaveNFe, ChaveNFe dadosChaveNFeSalvos)
        {
            Utils.Topo(versao, chaveNFe);
            if (dadosChaveNFeSalvos != null)
            {
                Console.WriteLine("\nForam identificados dados salvos da última geração");
                int.TryParse(dadosChaveNFeSalvos.AnoMes.Substring(0, 2), out int anoParcialSelecionado);
                int.TryParse(dadosChaveNFeSalvos.AnoMes.Substring(2, 2), out int mesSelecionado);

                if (Utils.Confirmacao($"\nConfirmar AnoMês ({dadosChaveNFeSalvos.AnoMes} - {Utils.ObterAnoMesPorExtenso(dadosChaveNFeSalvos.AnoMes, anoParcialSelecionado, mesSelecionado)}) salvo?", ConsoleColor.Green).Equals("S"))
                    return dadosChaveNFeSalvos.AnoMes;
            }

            Utils.Topo(versao, chaveNFe);
            string anoMesSelecionado = Utils.Pergunta("\nInforme o Ano e Mês de Emissão (formato YYMM - AnoMes):");

            int mes = 0;
            int anoParcial = 0;
            bool anoMesValido = false;

            while (!anoMesValido)
            {
                if (string.IsNullOrWhiteSpace(anoMesSelecionado) || anoMesSelecionado.Length != 4)
                {
                    Utils.Topo(versao, chaveNFe);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nO formato do Ano e Mês deve ser YYMM - AnoMes!");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    anoMesSelecionado = Utils.Pergunta("\nInforme o Ano e Mês de Emissão (formato YYMM - AnoMes):");

                    continue;
                }
                else if (!int.TryParse(anoMesSelecionado.Substring(0, 2), out anoParcial) || !int.TryParse(anoMesSelecionado.Substring(2, 2), out mes))
                {
                    Utils.Topo(versao, chaveNFe);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nO Ano e Mês contém caracteres inválidos!");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    anoMesSelecionado = Utils.Pergunta("\nInforme o Ano e Mês de Emissão (formato YYMM - AnoMes):");

                    continue;
                }
                else if (mes < 1 || mes > 12)
                {
                    Utils.Topo(versao, chaveNFe);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nO mês deve estar entre 01 e 12!");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    anoMesSelecionado = Utils.Pergunta("\nInforme o Ano e Mês de Emissão (formato YYMM - AnoMes):");

                    continue;
                }
                else
                    anoMesValido = true;

                if (anoMesValido)
                {
                    while (!Utils.Confirmacao($"\nConfirmar AnoMês ({anoMesSelecionado} - {Utils.ObterAnoMesPorExtenso(anoMesSelecionado, anoParcial, mes)}) informado?", ConsoleColor.Green).Equals("S"))
                    {
                        anoMesValido = false;
                        Utils.Topo(versao, chaveNFe);

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nInformação descartada!");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        anoMesSelecionado = Utils.Pergunta("\nInforme o Ano e Mês de Emissão (formato YYMM - AnoMes):");

                        break;
                    }
                }
            }

            return anoMesSelecionado;
        }

        public static string SelecionarCnpj(string versao, ChaveNFe chaveNFe, ChaveNFe dadosChaveNFeSalvos)
        {
            Utils.Topo(versao, chaveNFe);
            if (dadosChaveNFeSalvos != null)
            {
                Console.WriteLine("\nForam identificados dados salvos da última geração");
                if (Utils.Confirmacao($"\nConfirmar CNPJ ({dadosChaveNFeSalvos.Cnpj}) salvo?", ConsoleColor.Green).Equals("S"))
                    return dadosChaveNFeSalvos.Cnpj;
            }

            Utils.Topo(versao, chaveNFe);
            string cnpjSelecionado = Utils.Pergunta("\nInforme o CNPJ (14 dígitos):");

            bool cnpjValido = false;
            while (!cnpjValido)
            {
                if (cnpjSelecionado.Trim().Length != 14)
                {
                    Utils.Topo(versao, chaveNFe);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nCNPJ (14 dígitos) inválido. Por favor, informe um CNPJ válido!");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    cnpjSelecionado = Utils.Pergunta("\nInforme o CNPJ (14 dígitos):");

                    continue;
                }
                else
                    cnpjValido = true;

                if (cnpjValido)
                {
                    while (!Utils.Confirmacao($"\nConfirmar CNPJ ({cnpjSelecionado}) informado?", ConsoleColor.Green).Equals("S"))
                    {
                        cnpjValido = false;
                        Utils.Topo(versao, chaveNFe);

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nInformação descartada!");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        cnpjSelecionado = Utils.Pergunta("\nInforme o CNPJ (14 dígitos):");

                        break;
                    }
                }
            }

            return cnpjSelecionado;
        }

        public static string SelecionarSerie(string versao, ChaveNFe chaveNFe, ChaveNFe dadosChaveNFeSalvos)
        {
            Utils.Topo(versao, chaveNFe);
            if (dadosChaveNFeSalvos != null)
            {
                Console.WriteLine("\nForam identificados dados salvos da última geração");
                if (Utils.Confirmacao($"\nConfirmar Série ({dadosChaveNFeSalvos.Serie}) salva?", ConsoleColor.Green).Equals("S"))
                    return dadosChaveNFeSalvos.Serie;
            }

            Utils.Topo(versao, chaveNFe);
            string serieSelecionada = Utils.Pergunta("\nInforme a Série (A série precisa ter 3 caracteres - EX: 001):");

            bool serieValida = false;
            while (!serieValida)
            {
                if (serieSelecionada.Trim().Length != 3)
                {
                    Utils.Topo(versao, chaveNFe);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nSérie (A série precisa ter 3 caracteres - EX: 001) inválida. Por favor, informe uma Série válida!");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    serieSelecionada = Utils.Pergunta("\nInforme a Série (A série precisa ter 3 caracteres - EX: 001):");

                    continue;
                }
                else
                    serieValida = true;

                if (serieValida)
                {
                    while (!Utils.Confirmacao($"\nConfirmar Série ({serieSelecionada}) informada?", ConsoleColor.Green).Equals("S"))
                    {
                        serieValida = false;
                        Utils.Topo(versao, chaveNFe);

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nInformação descartada!");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        serieSelecionada = Utils.Pergunta("\nInforme a Série (A série precisa ter 3 caracteres - EX: 001):");

                        break;
                    }
                }
            }

            return serieSelecionada;
        }

        public static string SelecionarTipoEmissao(string versao, ChaveNFe chaveNFe, ChaveNFe dadosChaveNFeSalvos)
        {
            Utils.Topo(versao, chaveNFe);
            if (dadosChaveNFeSalvos != null)
            {
                Console.WriteLine("\nForam identificados dados salvos da última geração");
                if (Utils.Confirmacao($"\nConfirmar tipo de emissão ({dadosChaveNFeSalvos.TipoEmissao}: {Utils.ObterNomePorCodigo(dadosChaveNFeSalvos.TipoEmissao, _tiposEmissao)}) salvo?", ConsoleColor.Green).Equals("S"))
                    return dadosChaveNFeSalvos.TipoEmissao;
            }

            Utils.Topo(versao, chaveNFe);
            string codigoTipoEmissaoSelecionado = ObterTipoEmissaoSelecionado();

            bool tipoEmissaoValido = false;
            while (!tipoEmissaoValido)
            {
                while (!_tiposEmissao.ContainsKey(codigoTipoEmissaoSelecionado))
                {
                    Utils.Topo(versao, chaveNFe);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nOpção selecionada invalida, selecione uma das opções disponíveis!");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    codigoTipoEmissaoSelecionado = ObterTipoEmissaoSelecionado();
                }

                tipoEmissaoValido = true;
                while (!Utils.Confirmacao($"\nConfirmar tipo de emissão ({codigoTipoEmissaoSelecionado}: {Utils.ObterNomePorCodigo(codigoTipoEmissaoSelecionado, _tiposEmissao)}) selecionado?", ConsoleColor.Green).Equals("S"))
                {
                    tipoEmissaoValido = false;
                    Utils.Topo(versao, chaveNFe);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInformação descartada!");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    codigoTipoEmissaoSelecionado = ObterTipoEmissaoSelecionado();

                    break;
                }
            }

            return codigoTipoEmissaoSelecionado;
        }

        public static string ObterTipoEmissaoSelecionado()
        {
            Console.WriteLine("\nOpções:\n");

            foreach (var opcao in _tiposEmissao)
            {
                if (!string.IsNullOrEmpty(opcao.Key))
                    Console.WriteLine($"{opcao.Key}: {opcao.Value}");
                else
                    Console.WriteLine(" ");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nInforme uma das opções disponíveis: ");
            string codigoTipoEmissaoSelecionado = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            return codigoTipoEmissaoSelecionado;
        }

        public static string SelecionarNumeroInicial(string versao, ChaveNFe chaveNFe, ChaveNFe dadosChaveNFeSalvos)
        {
            Utils.Topo(versao, chaveNFe);
            if (dadosChaveNFeSalvos != null)
            {
                Console.WriteLine("\nForam identificados dados salvos da última geração");
                if (Utils.Confirmacao($"\nConfirmar número inicial ({int.Parse(dadosChaveNFeSalvos.NumeroInicial):D3}) salvo?", ConsoleColor.Green).Equals("S"))
                    return dadosChaveNFeSalvos.NumeroInicial;
            }

            Utils.Topo(versao, chaveNFe);
            string numeroInicialSelecionado = Utils.Pergunta("\nInforme o número inicial (EX: 001):");

            bool numeroInicialValido = false;
            while (!numeroInicialValido)
            {
                if ((string.IsNullOrEmpty(numeroInicialSelecionado)) && (!int.TryParse(numeroInicialSelecionado, out int resultado)))
                {
                    Utils.Topo(versao, chaveNFe);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nNúmero inicial inválido. Por favor, informe um número inicial válido!");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    numeroInicialSelecionado = Utils.Pergunta("\nInforme o número inicial (EX: 001):");

                    continue;
                }
                else
                    numeroInicialValido = true;

                if (numeroInicialValido)
                {
                    while (!Utils.Confirmacao($"\nConfirmar número inicial ({int.Parse(numeroInicialSelecionado):D3}) informado?", ConsoleColor.Green).Equals("S"))
                    {
                        Utils.Topo(versao, chaveNFe);

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nInformação descartada!");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        numeroInicialSelecionado = Utils.Pergunta("\nInforme o número inicial (EX: 001):");

                        break;
                    }
                }
            }

            return numeroInicialSelecionado;
        }

        public static string SelecionarNumeroFinal(string versao, ChaveNFe chaveNFe, ChaveNFe dadosChaveNFeSalvos)
        {
            Utils.Topo(versao, chaveNFe);
            if (dadosChaveNFeSalvos != null)
            {
                if ((int.Parse(dadosChaveNFeSalvos.NumeroFinal) > int.Parse(chaveNFe.NumeroInicial)))
                {
                    Console.WriteLine("\nForam identificados dados salvos da última geração");
                    if (Utils.Confirmacao($"\nConfirmar número final ({int.Parse(dadosChaveNFeSalvos.NumeroFinal):D3}) salvo?", ConsoleColor.Green).Equals("S"))
                        return dadosChaveNFeSalvos.NumeroFinal;
                }
            }

            Utils.Topo(versao, chaveNFe);
            string numeroFinalSelecionado = Utils.Pergunta($"\nInforme o número final (deve ser maior ou igual a {int.Parse(chaveNFe.NumeroInicial):D3}):");

            bool numeroFinalValido = false;
            while (!numeroFinalValido)
            {
                if (((string.IsNullOrEmpty(numeroFinalSelecionado)) && (!int.TryParse(numeroFinalSelecionado, out int resultado))))
                {
                    Utils.Topo(versao, chaveNFe);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nNúmero final inválido. Por favor, selecione um maior ou igual a {int.Parse(chaveNFe.NumeroInicial):D3}!");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    numeroFinalSelecionado = Utils.Pergunta($"\nInforme o número final (deve ser maior ou igual a {int.Parse(chaveNFe.NumeroInicial):D3}):");

                    continue;
                }
                else if ((int.Parse(numeroFinalSelecionado) < int.Parse(chaveNFe.NumeroInicial)))
                {
                    Utils.Topo(versao, chaveNFe);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nNúmero final inválido. Por favor, selecione um maior ou igual a {int.Parse(chaveNFe.NumeroInicial):D3}!");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    numeroFinalSelecionado = Utils.Pergunta($"\nInforme o número final (deve ser maior ou igual a {int.Parse(chaveNFe.NumeroInicial):D3}):");

                    continue;
                }
                else
                    numeroFinalValido = true;

                if (numeroFinalValido)
                {
                    while (!Utils.Confirmacao($"\nConfirmar número final ({int.Parse(numeroFinalSelecionado):D3}) informado?", ConsoleColor.Green).Equals("S"))
                    {
                        numeroFinalValido = false;
                        Utils.Topo(versao, chaveNFe);

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nInformação descartada!");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        numeroFinalSelecionado = Utils.Pergunta($"\nInforme o número final (deve ser maior ou igual a {int.Parse(chaveNFe.NumeroInicial):D3}):");

                        break;
                    }
                }
            }

            return numeroFinalSelecionado;
        }

        public static void Gerar(string versao, ChaveNFe chaveNFe)
        {
            Utils.Topo(versao, chaveNFe);
            IList<string> chaves = new List<string>();

            for (int numero = int.Parse(chaveNFe.NumeroInicial); numero <= int.Parse(chaveNFe.NumeroFinal); numero++)
            {
                string numeroFormatado = numero.ToString("D9");
                string codigoNumerico = new Random().Next(1, 99999999).ToString("D8");

                string chaveSemDV = chaveNFe.Uf + chaveNFe.AnoMes + chaveNFe.Cnpj + chaveNFe.ModeloNotaFiscalEletrônica + chaveNFe.Serie + numeroFormatado + chaveNFe.TipoEmissao + codigoNumerico;
                int dv = Utils.CalcularDigitoVerificador(chaveSemDV);

                string chaveAcesso = chaveSemDV + dv;
                chaves.Add(chaveAcesso);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nChaves geradas:");

            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (string chave in chaves)
                Console.WriteLine(chave);
        }
    }
}