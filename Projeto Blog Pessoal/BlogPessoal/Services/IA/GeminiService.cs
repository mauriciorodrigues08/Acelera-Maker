// Implementação do serviço de IA usando a API do Gemini.
// Envia o conteúdo da postagem para o Gemini e converte a resposta para o DTO ResultadoIA.

using System.Text;
using System.Text.Json;
using BlogPessoal.DTOs;

namespace BlogPessoal.Services.IA;

public class GeminiService : IIAService
{
    private const string CategoriaDefault = "Geral";

    private static readonly JsonSerializerOptions _jsonOptions =
        new() { PropertyNameCaseInsensitive = true };

    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Gemini:ApiKey"]!;
    }

    public async Task<ResultadoIA> GerarResumoAsync(string conteudo)
    {
        var prompt = PromptBuilder.BuildResumoPrompt(conteudo);

        var requestBody = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = prompt } } }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";
        var response = await _httpClient.PostAsync(url, content);

        // trata erros de status HTTP sem derrubar a aplicação
        if (!response.IsSuccessStatusCode)
        {
            return new ResultadoIA
            {
                Resumo = $"Serviço de IA indisponível ({(int)response.StatusCode}).",
                Tags = "",
                Categoria = CategoriaDefault
            };
        }

        var responseBody = await response.Content.ReadAsStringAsync();

        try
        {
            using var doc = JsonDocument.Parse(responseBody);

            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString()!;

            text = text.Trim().TrimStart('`').TrimEnd('`');
            if (text.StartsWith("json")) text = text[4..].Trim();

            var resultado = JsonSerializer.Deserialize<ResultadoIA>(text, _jsonOptions);

            return resultado ?? new ResultadoIA
            {
                Resumo = "Não foi possível gerar o resumo.",
                Tags = "",
                Categoria = CategoriaDefault
            };
        }
        catch (JsonException)
        {
            return new ResultadoIA
            {
                Resumo = "Resposta inválida da IA.",
                Tags = "",
                Categoria = CategoriaDefault
            };
        }
        catch (InvalidOperationException)
        {
            return new ResultadoIA
            {
                Resumo = "Estrutura de resposta da IA inesperada.",
                Tags = "",
                Categoria = CategoriaDefault
            };
        }
    }
}