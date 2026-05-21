// DTO que representa o resultado retornado pela API de IA.
// Contém o resumo, as tags e a categoria gerados automaticamente a partir do conteúdo de uma postagem.

namespace BlogPessoal.DTOs;

public class ResultadoIA
{
    // Resumo curto gerado pela IA sobre o conteúdo da postagem
    public string Resumo { get; set; } = string.Empty;

    // Palavras-chave relacionadas ao conteúdo da postagem
    public string Tags { get; set; } = string.Empty;

    // Categoria sugerida pela IA para classificar a postagem
    public string Categoria { get; set; } = string.Empty;
}