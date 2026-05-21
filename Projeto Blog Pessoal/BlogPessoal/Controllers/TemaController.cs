// Controller responsável pelos endpoints de Temas.
// Gerencia as requisições HTTP e delega a lógica ao TemaService.
// Todos os endpoints são protegidos por autenticação JWT.

using BlogPessoal.Models;
using BlogPessoal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPessoal.Controllers;

[Authorize]
[ApiController]
[Route("api/temas")]
public class TemaController : ControllerBase
{
    // Injeta o serviço de temas via injeção de dependência
    private readonly ITemaService _temaService;

    public TemaController(ITemaService temaService)
    {
        _temaService = temaService;
    }

    // GET api/temas
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // busca todos os temas
        var temas = await _temaService.GetAllAsync();
        
        // retorna os temas cadastrados
        return Ok(temas);
    }

    // GET api/temas/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        // busca um tema pelo id
        var tema = await _temaService.GetByIdAsync(id);

        // retorna 404 caso não encontre
        if (tema is null) return NotFound();

        // retorna o tema
        return Ok(tema);
    }

    // POST api/temas
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Tema tema)
    {
        // retorna 400 se os dados enviados não passarem nas validações do modelo
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // cria um novo tema
        var created = await _temaService.CreateAsync(tema);

        // retorna 201 com o tema criado e o seu link
        return CreatedAtAction(
                            nameof(GetById), 
                            new { id = created.Id },
                            created
                            );
    }

    // PUT api/temas/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] Tema tema)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // garante que o ID da rota é o mesmo do corpo da requisição
        tema.Id = id;

        // atualiza um tema existente
        var updated = await _temaService.UpdateAsync(tema);
        
        // retorna 404 caso o tema passado não exista 
        if (updated is null) return NotFound();

        // retorna o tema atualizado
        return Ok(updated);
    }

    // DELETE api/temas/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        // deleta o tema
        var deleted = await _temaService.DeleteAsync(id);

        // retorna 404 caso o tema não for encontrado
        if (!deleted) return NotFound();

        // retorna 204 se for excluído com sucesso
        return NoContent();
    }
}