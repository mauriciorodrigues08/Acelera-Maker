// Interface que define o contrato do serviço de IA.
// Permite trocar a implementação da API de IA sem alterar o restante do código.

using BlogPessoal.DTOs;

namespace BlogPessoal.Services.IA;

public interface IIAService
{
    // Recebe o conteúdo de uma postagem e retorna
    // resumo, tags e categoria gerados pela IA
    Task<ResultadoIA> GerarResumoAsync(string conteudo);
}