using System.Reflection;

namespace GeradorChaveNFe
{
    public static class IniManager
    {
        private static readonly string _iniPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
            AppDomain.CurrentDomain.BaseDirectory,
            "config.ini");

        public static void SalvarConfiguracao(ChaveNFe chave)
        {
            string[] linhas = new[]
            {
                "[ChaveNFe]",
                $"Uf={chave.Uf}",
                $"AnoMes={chave.AnoMes}",
                $"Cnpj={chave.Cnpj}",
                $"Serie={chave.Serie}",
                $"TipoEmissao={chave.TipoEmissao}",
                $"NumeroInicial={chave.NumeroInicial}",
                $"NumeroFinal={chave.NumeroFinal}",
                $"ModeloNotaFiscalEletrônica={chave.ModeloNotaFiscalEletrônica}"
            };

            File.WriteAllLines(_iniPath, linhas);
        }

        public static ChaveNFe LerConfiguracao()
        {
            if (!File.Exists(_iniPath))
                return null;

            ChaveNFe chave = new ChaveNFe();
            string[] linhas = File.ReadAllLines(_iniPath);

            foreach (var linha in linhas)
            {
                if (linha.StartsWith("["))
                    continue;

                string[] partes = linha.Split('=', 2);
                if (partes.Length != 2) continue;

                string chaveIni = partes[0].Trim();
                string valor = partes[1].Trim();

                switch (chaveIni)
                {
                    case "Uf": chave.Uf = valor; break;
                    case "AnoMes": chave.AnoMes = valor; break;
                    case "Cnpj": chave.Cnpj = valor; break;
                    case "Serie": chave.Serie = valor; break;
                    case "TipoEmissao": chave.TipoEmissao = valor; break;
                    case "NumeroInicial": chave.NumeroInicial = valor; break;
                    case "NumeroFinal": chave.NumeroFinal = valor; break;
                    case "ModeloNotaFiscalEletrônica": chave.ModeloNotaFiscalEletrônica = valor; break;
                }
            }

            return chave;
        }
    }
}