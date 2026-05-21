// Controller responsável pelo endpoint de IA.
// Recebe o texto de uma postagem, envia para o GeminiService 
// Retorna o resumo, tags e categoria gerados pela IA.

using BlogPessoal.Services.IA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPessoal.Controllers.IA;

[Authorize]
[ApiController]
[Route("api/ia")]
public class IAController : ControllerBase
{
    // Injeta o serviço de IA via injeção de dependência
    private readonly IIAService _iaService;

    public IAController(IIAService iaService)
    {
        _iaService = iaService;
    }

    // POST api/ia/resumir
    [HttpPost("resumir")]
    public async Task<IActionResult> Resumir([FromBody] string texto)
    {
        // retorna 400 se o texto estiver vazio
        if (string.IsNullOrWhiteSpace(texto))
            return BadRequest("O texto não pode ser vazio.");

        // envia o texto para o serviço de IA
        // retorna um resumo do texto
        var resultado = await _iaService.GerarResumoAsync(texto);

        // retorna 200 com o resultado gerado pela IA
        return Ok(resultado);
    }
}