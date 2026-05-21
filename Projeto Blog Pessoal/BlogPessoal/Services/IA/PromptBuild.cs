// Responsável por construir o prompt enviado à API de IA.
// Centraliza a lógica de formatação das instruções, facilitando ajustes futuros no comportamento da IA.

namespace BlogPessoal.Services.IA;

public static class PromptBuilder
{
    // Monta o prompt com instruções claras para a IA
    // retornar exatamente o formato JSON esperado
    public static string BuildResumoPrompt(string conteudo)
{
    return "Analise o seguinte texto de uma postagem de blog e retorne APENAS um JSON válido, " +
           "sem explicações, sem markdown, sem blocos de código. " +
           "O JSON deve ter exatamente este formato: " +
           "{\"Resumo\": \"resumo curto em até 2 frases\", " +
           "\"Tags\": \"tag1, tag2, tag3\", " +
           "\"Categoria\": \"uma categoria principal\"} " +
           "Texto da postagem: " + conteudo;
}
}