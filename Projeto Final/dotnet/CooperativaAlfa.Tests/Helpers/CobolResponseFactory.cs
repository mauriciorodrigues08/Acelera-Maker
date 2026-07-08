using CooperativaAlfa.Models;

namespace CooperativaAlfa.Tests.Helpers;

/// <summary>
/// Factory de respostas COBOL para uso nos testes.
/// Centraliza a criação de respostas simuladas, evitando
/// repetição e facilitando manutenção.
/// </summary>
public static class CobolResponseFactory
{
    public static CobolResponse ClienteEncontrado(
        int codigo = 1,
        string nome = "Joao Silva",
        string telefone = "11999999999",
        string email = "joao@teste.com") => new()
    {
        Status   = "00",
        Mensagem = "Cliente encontrado.",
        Codigo   = codigo,
        Nome     = nome,
        Telefone = telefone,
        Email    = email
    };

    public static CobolResponse AtualizacaoSucesso() => new()
    {
        Status   = "00",
        Mensagem = "Dados atualizados com sucesso."
    };

    public static CobolResponse ClienteNaoEncontrado() => new()
    {
        Status   = "04",
        Mensagem = "Cliente nao encontrado."
    };

    public static CobolResponse ErroInterno() => new()
    {
        Status   = "08",
        Mensagem = "Erro ao consultar o banco de dados."
    };
}