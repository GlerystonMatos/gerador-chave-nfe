# 🔑 gerador-chave-nfe

Aplicativo de **console em C# / .NET 10** que gera **chaves de acesso de NF-e válidas** (44 dígitos, com dígito verificador calculado por módulo 11) para uso em testes. Um assistente interativo guia a escolha dos parâmetros (UF, ano/mês, CNPJ, série, tipo de emissão e intervalo de numeração), gera todas as chaves do intervalo e salva os últimos valores usados em um `GeradorChaveNFe.ini` local para agilizar a próxima execução.

## Índice

- [🔑 gerador-chave-nfe](#-gerador-chave-nfe)
  - [Índice](#índice)
  - [Funcionalidades](#funcionalidades)
  - [Requisitos](#requisitos)
  - [Instalação](#instalação)
  - [Como usar](#como-usar)
  - [Composição da chave de acesso](#composição-da-chave-de-acesso)
  - [Fluxo do assistente](#fluxo-do-assistente)
  - [Padrão visual do console](#padrão-visual-do-console)
  - [Arquivo GeradorChaveNFe.ini](#arquivo-geradorchavenfeini)
  - [Estrutura do projeto](#estrutura-do-projeto)
  - [Validações](#validações)
  - [Limitações conhecidas](#limitações-conhecidas)
  - [Licença](#licença)

## Funcionalidades

- Gera um **intervalo** de chaves de acesso de NF-e (modelo 55) de uma só vez.
- **Assistente interativo** campo a campo, com validação e confirmação de cada valor.
- **Reaproveita os parâmetros da última geração** (lidos do `GeradorChaveNFe.ini`), perguntando campo a campo se deseja manter.
- Catálogo das **27 UFs** (código IBGE + sigla + nome) e dos **tipos de emissão**.
- Exibe o **ano/mês por extenso** ("2509 → Setembro de 2025") na confirmação.
- **Dígito verificador** calculado por módulo 11 (pesos 2 a 9).
- Interface de console com **tela de boas-vindas**, animação de **carregamento**, **cabeçalho fixo** com os parâmetros já escolhidos e **padrão de cores** consistente.
- **Zero dependências externas** — nenhum `PackageReference`; `dotnet build` / `dotnet run` funcionam offline.

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) ou superior.

## Instalação

```bash
git clone https://github.com/GlerystonMatos/gerador-chave-nfe
cd gerador-chave-nfe
dotnet build
```

## Como usar

```bash
dotnet run --project GeradorChaveNFe
```

O assistente pede, na ordem: **UF**, **ano/mês** (AAMM), **CNPJ**, **série**, **tipo de emissão**, **número inicial** e **número final**. Ao final, confirma o resumo, gera as chaves, imprime todas no console e grava o `GeradorChaveNFe.ini`. Em seguida pergunta se você quer gerar outro lote.

## Composição da chave de acesso

A chave tem **44 dígitos**, montados nesta ordem:

| Campo | Dígitos | Origem |
|---|---|---|
| UF | 2 | Código IBGE da UF do emitente |
| AnoMês | 4 | Ano e mês de emissão (AAMM) |
| CNPJ | 14 | CNPJ do emitente |
| Modelo | 2 | Fixo `55` (NF-e) |
| Série | 3 | Série informada |
| Número | 9 | Número da NF-e (preenchido com zeros à esquerda) |
| Tipo de emissão | 1 | Código informado |
| Código numérico | 8 | Aleatório por chave (`Random.Shared`) |
| Dígito verificador | 1 | Módulo 11 sobre os 43 dígitos anteriores |

## Fluxo do assistente

Para **cada campo**, o roteiro é sempre o mesmo:

1. Se há um valor salvo **válido** para o campo, pergunta se quer **reaproveitá-lo**. Se sim, segue para o próximo campo.
2. Pede o valor (campos de lista — UF e tipo de emissão — mostram as opções).
3. **Valida**. Se inválido, mostra a mensagem em vermelho e pede de novo.
4. **Confirma** o valor. Se recusado, pede de novo.

Depois dos sete campos, um **resumo** é exibido no cabeçalho e o usuário confirma a geração. Recusando o resumo, o assistente recomeça a coleta.

## Padrão visual do console

As cores têm papéis fixos (ver `Apresentacao/Paleta.cs`):

| Cor | Papel |
|---|---|
| Ciano | Título / moldura do cabeçalho |
| Amarelo | Valor que o usuário está digitando |
| Verde | Confirmações (S/N) e mensagens de sucesso |
| Vermelho | Mensagens de validação |
| Ciano-escuro | Dados salvos e dicas |
| Cinza | Texto comum e perguntas abertas |

O **cabeçalho** é redesenhado a cada passo e mostra, **em uma única linha**, os parâmetros já escolhidos (campos ainda não preenchidos aparecem como `—`):

```
╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
║ GERADOR DE CHAVE NF-e · por Gleryston Matos · v1.0.3.0                                             ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ UF: SP · AnoMês: 2509 · CNPJ: 12345678000199 · Série: 001 · Emissão: 1 · Números: 001–999 (999)    ║
╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
```

A tela de abertura e o cabeçalho usam a **mesma moldura** (largura fixa de 100 colunas, estilo `╔═╗ ║ ╠═╣ ║ ╚═╝`). Recomenda-se um terminal com **≥ 102 colunas** (o padrão do Windows Terminal é 120).

## Arquivo GeradorChaveNFe.ini

Criado/atualizado automaticamente **ao lado do executável** (`AppContext.BaseDirectory`) após cada geração:

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

A leitura é **tolerante**: se um campo estiver ausente ou inválido, ele simplesmente não é oferecido para reaproveitamento. Está no `.gitignore` e, por ficar em `bin/`, também é coberto por `[Bb]in/`.

## Estrutura do projeto

```
GeradorChaveNFe/
 ├─ Program.cs                       # Ponto de entrada: boas-vindas → coleta → geração → repetir
 ├─ Dominio/
 │   ├─ ParametrosChaveNfe.cs        # Os 7 parâmetros + modelo fixo "55"
 │   ├─ CatalogoUf.cs                # 27 UFs (código IBGE, sigla, nome) + validação
 │   ├─ CatalogoTipoEmissao.cs       # Tipos de emissão + validação
 │   └─ Competencia.cs               # Valida AAMM e converte para "Mês de AAAA"
 ├─ Geracao/
 │   ├─ GeradorDeChaves.cs           # Monta as chaves do intervalo
 │   └─ DigitoVerificador.cs         # Módulo 11 (pesos 2..9)
 ├─ Persistencia/
 │   └─ ArquivoParametrosIni.cs      # Ler()/Salvar() o GeradorChaveNFe.ini
 └─ Apresentacao/
     ├─ Paleta.cs                    # Padrão de cores + Escrever/EscreverLinha
     ├─ Tela.cs                      # BemVindo, Carregando, Cabecalho, Despedida
     ├─ Prompt.cs                    # Perguntar, Confirmar, EscolherOpcao
     └─ AssistenteParametros.cs      # Coleta dos 7 campos (reuso + validação + confirmação genéricos)
```

## Validações

| Campo | Regra |
|---|---|
| UF | Código presente no catálogo das 27 UFs |
| AnoMês | 4 dígitos, mês entre 01 e 12 |
| CNPJ | 14 dígitos numéricos (não valida dígitos verificadores — permite CNPJ fictício de teste) |
| Série | 3 dígitos numéricos |
| Tipo de emissão | Código presente no catálogo (`1` ou `2`) |
| Número inicial | Inteiro de 1 a 999.999.999 |
| Número final | Inteiro entre o número inicial e 999.999.999; o intervalo não pode passar de 100.000 chaves |

## Limitações conhecidas

- Ferramenta **interativa**: assume um terminal real; com EOF/entrada redirecionada o app **encerra** (não trava), mas não há modo não-interativo.
- O **código numérico** de cada chave é aleatório — duas execuções com os mesmos parâmetros geram chaves diferentes (comportamento esperado para dados de teste).
- Não valida os **dígitos verificadores do CNPJ**, por design (facilita o uso com CNPJs fictícios).
- Sem testes automatizados.

## Licença

`gerador-chave-nfe` é um projeto livre e de código aberto licenciado sob a [MIT License](./LICENSE).