# CLAUDE.md

Memória de contexto persistente do projeto **gerador-chave-nfe**. Serve para orientar
qualquer sessão futura sem precisar reexplicar as decisões já tomadas.

---

## 1. Visão geral

Aplicativo **console em C# / .NET 10** que gera **chaves de acesso de NF-e válidas**
(44 dígitos, com dígito verificador por módulo 11) para uso em **testes**.

- Um **assistente interativo** coleta sete parâmetros (UF, ano/mês, CNPJ, série,
  tipo de emissão, número inicial e final), gera todas as chaves do intervalo e as
  imprime no console.
- Os parâmetros da última execução são salvos em `GeradorChaveNFe.ini` (ao lado do
  executável) e oferecidos campo a campo na execução seguinte.
- **Zero dependências externas** — nenhum `PackageReference`. Build/run offline.
- **Todo o projeto está em português (pt-BR)**: interface, mensagens, comentários,
  identificadores. Só permanecem em inglês o que a plataforma exige (`Program`,
  `Main`, `Random`, `Console`, `Thread`) e a sigla `Nfe`/`NFe`.
- Interface de console com **tela de boas-vindas**, **carregamento** animado,
  **cabeçalho fixo** (mostra os parâmetros já escolhidos) e **padrão de cores**
  centralizado.

---

## 2. Como compilar e rodar

```bash
dotnet build                              # a partir da raiz do repositório
dotnet run --project GeradorChaveNFe
```

- SDK: **.NET 10** (`net10.0`). `LangVersion=latest`, `ImplicitUsings=enable`,
  `Nullable=enable`, `InvariantGlobalization=false` (necessário: nome do mês por
  extenso usa `CultureInfo("pt-BR")`).
- Solução: `GeradorChaveNFe.slnx` → `GeradorChaveNFe/GeradorChaveNFe.csproj`.
- Namespace raiz e assembly: `GeradorChaveNFe`.
- **Não há testes automatizados.** Após mudanças, rodar `dotnet build` e manter
  **0 warnings**.

### Estado da compilação

- **`Build succeeded` — 0 erros, 0 warnings.**

### Histórico de mudanças estruturais (sessão 2026-09-02)

Refatoração completa para aproximar o projeto do padrão do projeto irmão
`toggl-report` (organização, formatação, tipos explícitos, `CLAUDE.md`):

- **Estrutura**: os 5 arquivos planos (`Program`, `GeradorChave`, `ChaveNFe`,
  `IniManager`, `Utils`) viraram 4 pastas por responsabilidade — `Dominio/`,
  `Geracao/`, `Persistencia/`, `Apresentacao/` (11 arquivos nas pastas + `Program.cs`).
- **`GeradorChave.cs` (513 linhas, altamente repetitivo)** foi substituído por
  `Apresentacao/AssistenteParametros.cs`: um único método genérico
  `LerCampo(...)` (leitura → validação → confirmação) e um método curto por
  campo. As dezenas de blocos `Console.ForegroundColor = ...` copiados sumiram.
- **`Utils.cs`** foi dividido em `Apresentacao/Paleta.cs` (cores),
  `Apresentacao/Tela.cs` (telas fixas), `Apresentacao/Prompt.cs` (entrada) e
  `Dominio/Competencia.cs` / `Geracao/DigitoVerificador.cs`.
- **Dependência `TextToAsciiArt` removida** — o banner de boas-vindas passou a ser
  uma moldura Unicode desenhada à mão (alinhado à filosofia "zero dependências").
- **`net9.0` → `net10.0`**, `Nullable disable → enable`, `var` → **tipo explícito**.
- **`.editorconfig`** adicionado na raiz (CRLF + UTF-8), igual ao `toggl-report`.
- Arquivo de config renomeado de `config.ini` para **`GeradorChaveNFe.ini`**
  (padrão `<AssemblyName>.ini`) e adicionado ao `.gitignore`.
- Versão `1.0.2.0` → `1.0.3.0`.
- **Limpeza de comentários**: removidos os que apenas repetiam o código; mantidos
  só os que explicam um *porquê* ou regra de domínio (layout da chave, módulo 11,
  leitura tolerante do INI, guarda do `Console.Clear`).

O `toggl-report` recebeu depois o mesmo tratamento visual usando **a interface
deste projeto como referência** (`Apresentacao/` com `Paleta`/`Tela`/`Prompt`,
banner, cabeçalho a cada passo). Os dois projetos devem permanecer consistentes.

### Correções de bugs feitas na refatoração

- `SelecionarNumeroInicial`/`SelecionarNumeroFinal`: a validação `&&` estava
  invertida e deixava passar entrada não numérica; `int.Parse` sem guarda podia
  lançar. Agora `int.TryParse` + regra de faixa.
- `dadosChaveNFeSalvos != null` era **sempre** verdadeiro (o leitor devolvia um
  objeto com defaults). Agora `ArquivoParametrosIni.Ler()` devolve `null` sem
  arquivo, e cada campo só é oferecido para reaproveitamento se o valor salvo for
  **válido**.
- `ObterAnoMesPorExtenso` lançava `ArgumentOutOfRangeException` quando o AnoMes
  salvo era o default "YYMM" (`new DateTime(2000, 0, 1)`). Agora `Competencia`
  valida antes de formatar.
- `new Random()` dentro do laço → `Random.Shared`.
- `Console.ReadLine()` sem tratamento de `null` (NRE em EOF/Ctrl+Z) → `?? ""`.
- `Console.Clear()` lançava `IOException` com saída redirecionada → `Tela.Limpar()`
  engole a exceção (torna o app "pipeável" para testes).

### Correções após revisão por agentes independentes

- **Número da NF-e limitado a `999_999_999`** (9 posições na chave) — antes um valor
  ≥ 1e9 gerava `D9` com 10 dígitos e uma chave de 45 dígitos silenciosamente.
- **Intervalo limitado a `100_000` chaves** por vez — evita `OutOfMemoryException`.
- **`ExibirChaves` antes de salvar**, e `SalvarParametros` engole
  `IOException`/`UnauthorizedAccessException` — o resultado não se perde se o
  `GeradorChaveNFe.ini` não puder ser gravado.
- `Prompt.Confirmar` ganhou `bool padraoSim` e aceita `y/yes` (paridade com o
  `toggl-report`).
- `.sln` → `.slnx`; `.gitattributes` (`* text=auto eol=crlf`, `core.autocrlf=false`);
  `*.zip` no `.gitignore`; elementos vazios removidos do `.csproj`; comentário do
  *porquê* no `DigitoVerificador`; `PadRight(30)` na lista de opções.

### Alinhamento final de arquitetura/funcionamento (paridade com o `toggl-report`)

- **Moldura única** (`╔═╗ ║ ╠═╣ ║ ╚═╝`, `Largura = 100`) via `Borda` /
  `LinhaCentralizada` / `LinhaTexto` / `Centralizar` / `Ajustar` (trunca com `…`) —
  a tela de abertura e o cabeçalho têm **a mesma largura e o mesmo estilo**.
- **Resumo do cabeçalho em uma única linha** (`UF · AnoMês · CNPJ · Série ·
  Emissão · Números`).
- `Prompt` é **idêntico** ao do `toggl-report` (`Perguntar` / `Confirmar` /
  `EscolherOpcao`), inclusive `LerEntrada` — em **EOF** o app **encerra**
  (`Environment.Exit(0)`) em vez de entrar em laço.
- Prompt de reaproveitar valor salvo e o de confirmar tudo agora são `padraoSim`
  (Enter = sim).
- Laço principal renomeado para `GerarChavesEnquantoUsuarioQuiser` (padrão
  `Gerar<X>EnquantoUsuarioQuiser`, igual ao irmão).
- `.csproj`: removido `<BaseOutputPath>bin\</BaseOutputPath>` (era o valor padrão).

Assume terminal com ≥ 102 colunas (o padrão do Windows Terminal é 120).

> `GeradorChaveNFe_1.0.2.0.zip` (artefato de release, ainda na versão 1.0.2.0)
> continua versionado — o `README` linka para ele via *raw*. Se for lançar a
> 1.0.3.0, **regerar o zip** e trocar o link.

---

## 3. Fluxo funcional (o que `Program.Main` faz)

1. `Console.OutputEncoding = UTF8`; `Tela.BemVindo` (banner) + `Tela.Carregando`
   (animação ~1 s).
2. **`GerarChavesEnquantoUsuarioQuiser`** — laço `while (continuar)`:
   1. **`ColetarParametrosConfirmados`** — laço até o usuário confirmar o resumo:
      - `new ParametrosChaveNfe()` + `ArquivoParametrosIni.Ler()` (uma vez por
        ciclo, não uma vez por campo).
      - `AssistenteParametros.Coletar(parametros, salvos)` preenche os 7 campos.
      - Cabeçalho com o resumo → `Prompt.Confirmar("Confirmar a geração…?", padraoSim: true)`.
   2. `GeradorDeChaves.Gerar(parametros)` → `IReadOnlyList<string>`.
   3. `ExibirChaves` imprime todas.
   4. `SalvarParametros` grava o `GeradorChaveNFe.ini` (try/catch — falha não é fatal).
   5. `Prompt.Confirmar("Deseja gerar mais chaves…?")` decide se repete.
3. `Tela.Despedida` (banner + "Até a próxima!" + pausa curta).

---

## 4. Decisões de arquitetura

| Decisão | Motivo |
|---|---|
| **`AssistenteParametros.LerCampo` genérico** | Antes cada `SelecionarX` repetia o mesmo laço de validação/confirmação. Um método + `Func<string,string?> validar` + `Func<string,string> descrever` cobre todos os campos de texto e de lista. |
| **Catálogos como `record` em classe estática** (`CatalogoUf`, `CatalogoTipoEmissao`) | Dados fixos, sem estado; expõem `CodigoValido`, `Buscar`, `Descrever`, `Todas/Todos`. |
| **`ParametrosChaveNfe` com `int` para os números** | O original guardava tudo como `string` e fazia `int.Parse` em dezenas de lugares. |
| **`Paleta` centraliza as cores** | Cada `ConsoleColor` tem um papel fixo; a interface fica consistente e fácil de reajustar. |
| **`GeradorChaveNFe.ini` como único estado persistente** | Sem banco; formato simples `[Secao]` + `chave=valor`; leitura tolerante. |
| **`AppContext.BaseDirectory` para o `GeradorChaveNFe.ini`** | Robusto para publish single-file (ao contrário de `Assembly.Location`). |
| **Tipo explícito no lugar de `var`** | Preferência dos projetos. |
| **Zero dependências NuGet** | Build offline; banner feito à mão. |
| **`Random.Shared`** | Uma instância compartilhada; evita seeds repetidos em laço. |

---

## 5. Estrutura de arquivos

```
gerador-chave-nfe/
 ├─ CLAUDE.md                          # este arquivo
 ├─ README.md                          # documentação para usuário final (pt-BR)
 ├─ LICENSE                            # MIT
 ├─ .editorconfig · .gitattributes     # crlf em tudo; utf-8 em *.cs e *.json
 ├─ GeradorChaveNFe.slnx
 └─ GeradorChaveNFe/
     ├─ Program.cs                     # ponto de entrada + orquestração
     ├─ GeradorChaveNFe.csproj         # net10.0, OutputType=Exe, sem PackageReference
     ├─ icon.ico
     ├─ Dominio/
     │   ├─ ParametrosChaveNfe.cs      # 7 parâmetros + const Modelo "55" + QuantidadeDeChaves
     │   ├─ CatalogoUf.cs              # record Uf + 27 UFs + CodigoValido/Buscar/Descrever
     │   ├─ CatalogoTipoEmissao.cs     # record TipoEmissao + "1"/"2" + validação
     │   └─ Competencia.cs             # Validar(AAMM) / PorExtenso / Descrever
     ├─ Geracao/
     │   ├─ GeradorDeChaves.cs         # Gerar(parametros) -> lista de chaves de 44 dígitos
     │   └─ DigitoVerificador.cs       # Calcular(chaveSemDigito) -> int (módulo 11)
     ├─ Persistencia/
     │   └─ ArquivoParametrosIni.cs    # Ler() -> ParametrosChaveNfe? | Salvar(parametros)
     └─ Apresentacao/
         ├─ Paleta.cs                  # cores + Escrever / EscreverLinha
         ├─ Tela.cs                    # BemVindo / Carregando / Cabecalho / Despedida / Limpar
         ├─ Prompt.cs                  # Perguntar / Confirmar / EscolherOpcao
         └─ AssistenteParametros.cs    # Coletar(...) + LerCampo genérico + 1 método por campo
```

### Detalhes de domínio importantes

- **Chave de acesso (44 díg.)**: `UF(2) + AnoMês(4) + CNPJ(14) + Modelo(2) +
  Série(3) + Número(9) + TipoEmissão(1) + CódigoNumérico(8) + DV(1)`.
- **Modelo** é `const string "55"` em `ParametrosChaveNfe` (não é escolhido nem
  persistido).
- **Dígito verificador**: módulo 11, pesos 2→9 da direita para a esquerda sobre os
  43 dígitos. `resto ∈ {0,1}` ⇒ DV `0`; senão DV `11 - resto`.
- **Código numérico (cNF)**: `Random.Shared.Next(0, 100_000_000)` formatado `D8`,
  um por chave.
- **AnoMês**: formato `AAMM`; `Competencia.PorExtenso("2509")` → `"Setembro de 2025"`.

### `GeradorChaveNFe.ini` (exemplo)

```ini
[ChaveNFe]
Uf=35
AnoMes=2509
Cnpj=12345678000199
Serie=001
TipoEmissao=1
NumeroInicial=1
NumeroFinal=3
```

Gravado em `AppContext.BaseDirectory` após cada geração (`SalvarParametros`, com
`try/catch` — falha não é fatal). Leitura tolerante (campo ausente/inválido ⇒ não
é oferecido para reaproveitamento). Está no `.gitignore`.

---

## 6. Validações (contrato do assistente)

| Campo | Regra |
|---|---|
| UF | `CatalogoUf.CodigoValido` (código IBGE das 27 UFs) |
| AnoMês | 4 dígitos; mês (2 últimos) entre 01 e 12 |
| CNPJ | 14 dígitos numéricos — **não** valida DV do CNPJ (permite fictício) |
| Série | 3 dígitos numéricos |
| Tipo de emissão | `CatalogoTipoEmissao.CodigoValido` (`1` ou `2`) |
| Número inicial | inteiro de 1 a 999.999.999 |
| Número final | inteiro entre o número inicial e 999.999.999; intervalo ≤ 100.000 chaves |

Para cada campo: valor inválido → mensagem em vermelho + repete; valor válido →
confirmação → se recusado, repete.

---

## 7. Limitações conhecidas (assumidas — não são bugs)

- **App interativo**: assume terminal real. Com EOF/entrada redirecionada o app
  **encerra** (`Prompt.LerEntrada` → `Environment.Exit(0)`), mas não há modo
  "batch"/não-interativo.
- Chaves não são determinísticas (cNF aleatório) — esperado para dados de teste.
- Sem validação de DV do CNPJ (por design).
- Sem testes automatizados.

---

## 8. Convenções ao continuar o projeto

- **Tudo em português**: código, identificadores, comentários, mensagens.
  Exceções: `Program`/`Main`, tipos da BCL (`Console`, `Random`, `Thread`), sigla
  `Nfe`.
- **Tipo explícito no lugar de `var`** sempre que o tipo puder ser nomeado.
- Manter **zero dependências NuGet** salvo decisão explícita.
- Nova tela/entrada → `Apresentacao/`; nova regra de domínio → `Dominio/`.
  Sempre usar `Paleta` para cor (nunca `Console.ForegroundColor` solto).
- Métodos curtos: um por campo no assistente; extrair helper genérico em vez de
  copiar laço.
- `using` só para namespaces **não** cobertos pelo `ImplicitUsings`.
- `end_of_line = crlf` (via `.editorconfig` + `.gitattributes`, `core.autocrlf=false`),
  `charset = utf-8` (sem BOM).
- Rodar `dotnet build` após mudanças; manter **0 warnings**.

---

## 9. Projeto irmão

`C:\Projetos\toggl-report` (relatórios de tempo do Toggl Track) segue as **mesmas
convenções e o mesmo funcionamento**: `Paleta.cs`, `Prompt.cs` e a moldura de
`Tela.cs` (`Largura = 100`, helpers, `Carregando`, `Despedida`, EOF → encerra) são
**idênticos**; o assistente (`LerCampo` genérico + reaproveitar/confirmar campo a
campo + confirmação final no `Program`), o padrão do arquivo de config
(`<AssemblyName>.ini` em `AppContext.BaseDirectory`, gravação com `try/catch`) e a
nomenclatura (`Gerar<X>EnquantoUsuarioQuiser`) foram alinhados nos dois. **A
interface deste projeto foi a referência visual** para o `toggl-report`. O
`CLAUDE.md` de cada repо é a fonte da verdade daquele projeto.

### Diferenças que permanecem (justificadas pelos domínios — não são pendências)

| Ponto | Aqui (gerador-chave-nfe) | Lá (toggl-report) | Por quê |
|---|---|---|---|
| Versão | `1.0.3.0` (`X.Y.Z.W`) | `1.0.0` (`X.Y.Z`) | Cada projeto tem a sua (decisão do autor). |
| `Main` | `void Main()` | `async Task Main()` | só o toggl faz chamadas HTTP. |
| Camada de saída | não há (só `ExibirChaves`) | `Relatorios/` (agrupamento, escritores console/CSV, busca) | o gerador só imprime chaves. |
| Integração externa | não há | pasta `Toggl/` (`ClienteApiToggl` + DTOs) | só o toggl fala com uma API. |
| Descrição de exibição | `Uf.Descricao` / `TipoEmissao.Descricao` / `Competencia.Descrever` (há tipos de domínio) | `Rotulos.Agrupamento` (agrupamento é uma string de 3 valores, sem tipo) | onde há tipo, o rótulo mora nele; onde não há, num helper. |
| Nomes de pastas | `Dominio/` · `Geracao/` · `Persistencia/` | `Configuracao/` · `Toggl/` · `Relatorios/` | domínios de tamanho diferente; ambos coerentes. |
| Artefato de release | `GeradorChaveNFe_1.0.2.0.zip` versionado (linkado no `README`) | nenhum no repo | modelo de distribuição do gerador; **está defasado** (1.0.2.0 vs 1.0.3.0). |
