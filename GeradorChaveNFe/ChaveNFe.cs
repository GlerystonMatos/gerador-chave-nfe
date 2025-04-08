namespace GeradorChaveNFe
{
    public class ChaveNFe
    {
        public ChaveNFe()
        {
            Uf = "00";
            AnoMes = "YYMM";
            Cnpj = "00000000000000";
            Serie = "000";
            TipoEmissao = "0";
            NumeroInicial = "000";
            NumeroFinal = "000";
            ModeloNotaFiscalEletrônica = "55";
        }

        public string Uf { get; set; }

        public string AnoMes { get; set; }

        public string Cnpj { get; set; }

        public string Serie { get; set; }

        public string TipoEmissao { get; set; }

        public string NumeroInicial { get; set; }

        public string NumeroFinal { get; set; }

        public string ModeloNotaFiscalEletrônica { get; set; }
    }
}