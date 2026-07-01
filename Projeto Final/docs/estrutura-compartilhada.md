# Estrutura Compartilhada de Dados

Este documento define o contrato de dados utilizado para comunicação entre a aplicação .NET e o componente COBOL. Qualquer alteração nesta estrutura impacta ambos os lados da solução e deve ser versionada aqui.

## Formato de troca

**JSON via stdin/stdout**

O .NET escreve um JSON no `stdin` do processo COBOL ao iniciá-lo. O COBOL processa a operação e escreve um JSON de resposta no `stdout`. O .NET lê esse retorno assim que o processo encerra.

Esse formato foi escolhido por ser legível, extensível (novos campos podem ser adicionados sem quebrar versões anteriores) e alinhado com o padrão de APIs REST estudado no curso (JSON como formato de troca).

## Campos da estrutura de cliente

| Campo | Tipo COBOL | Tamanho | Tipo .NET | Tipo SQLite | Descrição |
|---|---|---|---|---|---|
| codigo | PIC 9(9) | 9 dígitos | `int` | INTEGER (PK) | Código único do cliente |
| nome | PIC X(50) | 50 chars | `string` | TEXT | Nome completo |
| telefone | PIC X(15) | 15 chars | `string` | TEXT | Telefone de contato |
| email | PIC X(60) | 60 chars | `string` | TEXT | E-mail de contato |

## Campos de controle (apenas na troca .NET ↔ COBOL, não persistidos)

| Campo | Tipo COBOL | Tamanho | Tipo .NET | Descrição |
|---|---|---|---|---|
| operacao | PIC X(1) | 1 char | `string` | `"C"` = consultar, `"A"` = atualizar |
| status | PIC X(2) | 2 chars | `string` | `"00"` = ok, `"04"` = não encontrado, `"08"` = erro |
| mensagem | PIC X(100) | 100 chars | `string` | Descrição legível do resultado |

## JSON de entrada (.NET → COBOL via stdin)

### Consulta
```json
{
  "operacao": "C",
  "codigo": 1
}
```

### Atualização
```json
{
  "operacao": "A",
  "codigo": 1,
  "telefone": "11988887777",
  "email": "joao.novo@teste.com"
}
```

## JSON de saída (COBOL → .NET via stdout)

### Sucesso — consulta
```json
{
  "status": "00",
  "mensagem": "Cliente encontrado.",
  "codigo": 1,
  "nome": "Joao Silva",
  "telefone": "11999999999",
  "email": "joao@teste.com"
}
```

### Sucesso — atualização
```json
{
  "status": "00",
  "mensagem": "Dados atualizados com sucesso."
}
```

### Cliente não encontrado
```json
{
  "status": "04",
  "mensagem": "Cliente nao encontrado."
}
```

### Erro de conexão/execução
```json
{
  "status": "08",
  "mensagem": "Erro interno ao acessar o banco de dados."
}
```

## Schema do banco SQLite

```sql
CREATE TABLE clientes (
    codigo    INTEGER PRIMARY KEY,
    nome      TEXT    NOT NULL,
    telefone  TEXT    NOT NULL,
    email     TEXT    NOT NULL
);
```

## Mapeamento de tipos

| Tipo COBOL | Tipo .NET | Tipo SQLite | Observação |
|---|---|---|---|
| PIC 9(9) | `int` | INTEGER | COBOL usa texto fixo com zeros à esquerda; .NET e SQLite usam inteiro nativo |
| PIC X(n) | `string` | TEXT | COBOL preenche com espaços à direita até o tamanho máximo; .NET e SQLite usam string com tamanho variável — o wrapper C já remove os espaços antes de gravar |

## Regras de validação

- `codigo` deve ser maior que zero
- `telefone` deve conter apenas dígitos e ter entre 10 e 15 caracteres
- `email` deve conter `@` e ter no máximo 60 caracteres
- `operacao` deve ser `"C"` ou `"A"`