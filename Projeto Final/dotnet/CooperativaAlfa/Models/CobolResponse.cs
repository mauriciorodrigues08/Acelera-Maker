using System.Text.Json.Serialization;

namespace CooperativaAlfa.Models;

/// <summary>
/// Representa o JSON retornado pelo processo COBOL via stdout.
/// Os nomes das propriedades seguem o contrato definido em
/// docs/estrutura-compartilhada.md.
/// </summary>
public class CobolResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("mensagem")]
    public string Mensagem { get; set; } = string.Empty;

    // Campos presentes apenas na resposta de consulta bem-sucedida
    [JsonPropertyName("codigo")]
    public int? Codigo { get; set; }

    [JsonPropertyName("nome")]
    public string? Nome { get; set; }

    [JsonPropertyName("telefone")]
    public string? Telefone { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    // Helpers para facilitar a leitura do status nos controllers
    public bool Sucesso => Status == "00";
    public bool NaoEncontrado => Status == "04";
    public bool Erro => Status == "08";
}