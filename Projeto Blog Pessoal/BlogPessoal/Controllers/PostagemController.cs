// Controller responsável pelos endpoints de Postagens.
// Gerencia criação, atualização, exclusão e listagem de postagens.
// Todos os endpoints são protegidos por autenticação JWT.

using BlogPessoal.Models;
using BlogPessoal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPessoal.Controllers;

[Authorize]
[ApiController]
[Route("api/postagens")]
public class PostagemController : ControllerBase
{
    // Injeta o serviço de postagens via injeção de dependência
    private readonly IPostagemService _postagemService;

    public PostagemController(IPostagemService postagemService)
    {
        _postagemService = postagemService;
    }

    // GET api/postagens
    // Retorna todas as postagens com Tema e Usuario incluídos
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // busca todas as postagens
        var postagens = await _postagemService.GetAllAsync();

        // retorna 200 com a lista de postagens
        return Ok(postagens);
    }

    // GET api/postagens/{id}
    // Retorna uma postagem pelo ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        // busca a postagem pelo ID
        var postagem = await _postagemService.GetByIdAsync(id);

        // retorna 404 caso a postagem não for encontrada
        if (postagem is null) return NotFound();

        // retorna 200 com a postagem encontrada
        return Ok(postagem);
    }

    // GET api/postagens/filtro?autor={id}&tema={id}
    // Filtra postagens por autor e/ou tema
    [HttpGet("filtro")]
    public async Task<IActionResult> GetByFiltro(
        [FromQuery] long? autor,
        [FromQuery] long? tema)
    {
        // retorna 400 se nenhum filtro for informado
        if (autor is null && tema is null)
            return BadRequest("Informe ao menos um filtro (autor ou tema).");

        // filtra por autor se o parâmetro for informado
        if (autor is not null)
        {
            // busca postagens pelo ID do autor
            var porAutor = await _postagemService.GetByAutorAsync(autor.Value);
         
            // retorna 200 com as postagens encontradas
            return Ok(porAutor);
        }

        // filtra por tema se o parâmetro for informado
        // busca postagens pelo ID do tema
        var porTema = await _postagemService.GetByTemaAsync(tema!.Value);

        // retorna 200 com as postagens encontradas
        return Ok(porTema);
    }

    // POST api/postagens
    // Cria uma nova postagem vinculada a um Tema e um Usuario
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Postagem postagem)
    {
        // retorna 400 se os dados enviados não passarem nas validações do modelo
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // tenta criar a postagem
        var created = await _postagemService.CreateAsync(postagem);

        // retorna 400 se o Tema ou Usuario informado não existir
        if (created is null)
            return BadRequest("Tema ou Usuário informado não existe.");

        // retorna 201 com a postagem criada
        return CreatedAtAction(
                            nameof(GetById),
                            new { id = created.Id },
                            created
                            );
    }

    // PUT api/postagens/{id}
    // Atualiza uma postagem existente
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] Postagem postagem)
    {
        // retorna 400 se os dados enviados não passarem nas validações do modelo
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // garante que o ID da rota é o mesmo do corpo da requisição
        postagem.Id = id;

        // tenta atualizar a postagem
        var updated = await _postagemService.UpdateAsync(postagem);
        
        // retorna 404 caso a postagem não for encontrada
        if (updated is null) return NotFound();
        
        // retorna 200 com a postagem atualizada
        return Ok(updated);
    }

    // DELETE api/postagens/{id}
    // Remove uma postagem pelo ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        // tenta deletar a postagem
        var deleted = await _postagemService.DeleteAsync(id);

        // retorna 404 caso a postagem não for encontrada
        if (!deleted) return NotFound();

        // retorna 204 se for excluída com sucesso
        return NoContent();
    }
}