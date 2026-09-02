namespace GeradorChaveNFe.Geracao;

/// <summary>
/// Dígito verificador da chave de acesso da NF-e: módulo 11 com pesos de 2 a 9,
/// aplicados da direita para a esquerda sobre os 43 primeiros dígitos.
/// </summary>
public static class DigitoVerificador
{
    public static int Calcular(string chaveSemDigito)
    {
        int soma = 0;
        int peso = 2;

        for (int i = chaveSemDigito.Length - 1; i >= 0; i--)
        {
            soma += (chaveSemDigito[i] - '0') * peso;
            peso = peso == 9 ? 2 : peso + 1;
        }

        int resto = soma % 11;
        int digito = 11 - resto;

        // resto 0 ou 1 → digito 11 ou 10 → DV = 0 (regra da SEFAZ).
        return digito >= 10 ? 0 : digito;
    }
}