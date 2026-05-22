// Controller responsável pelos endpoints de Postagens.
// Gerencia criação, atualização, exclusão e listagem de postagens.
// Todos os endpoints são protegidos por autenticação JWT.

using BlogPessoal.DTOs;
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
    private readonly IPostagemService _postagemService;
    private readonly ITemaService _temaService;
    private readonly IUsuarioService _usuarioService;

    public PostagemController(
        IPostagemService postagemService,
        ITemaService temaService,
        IUsuarioService usuarioService)
    {
        _postagemService  = postagemService;
        _temaService      = temaService;
        _usuarioService   = usuarioService;
    }

    // mapeia a entidade Postagem para o DTO de resposta
    private static PostagemResponseDTO MapToDTO(Postagem p) => new()
    {
        Id = p.Id,
        Titulo = p.Titulo,
        Texto = p.Texto,
        Data = p.Data,
        ResumoIA = p.ResumoIA,
        TagsIA = p.TagsIA,
        CategoriaIA = p.CategoriaIA,
        Tema = p.Tema is null ? null : new TemaResumoDTO
        {
            Id = p.Tema.Id,
            Descricao = p.Tema.Descricao
        },
        Usuario = p.Usuario is null ? null : new UsuarioResumoDTO
        {
            Id = p.Usuario.Id,
            Nome = p.Usuario.Nome,
            Email = p.Usuario.Email,
            Foto = p.Usuario.Foto
        }
    };

    // GET api/postagens
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var postagens = await _postagemService.GetAllAsync();
        // converte cada postagem para o DTO antes de retornar
        return Ok(postagens.Select(MapToDTO));
    }

    // GET api/postagens/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var postagem = await _postagemService.GetByIdAsync(id);
        if (postagem is null) return NotFound();
        // converte para DTO antes de retornar
        return Ok(MapToDTO(postagem));
    }

    // GET api/postagens/filtro
    [HttpGet("filtro")]
    public async Task<IActionResult> GetByFiltro(
        [FromQuery] long? autor,
        [FromQuery] long? tema)
    {
        if (autor is null && tema is null)
            return BadRequest("Informe ao menos um filtro: autor ou tema.");

        if (autor is not null)
        {
            var porAutor = await _postagemService.GetByAutorAsync(autor.Value);
            return Ok(porAutor.Select(MapToDTO));
        }

        var porTema = await _postagemService.GetByTemaAsync(tema!.Value);
        return Ok(porTema.Select(MapToDTO));
    }

    // POST api/postagens
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PostagemRequestDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var postagem = new Postagem
        {
            Titulo  = dto.Titulo,
            Texto   = dto.Texto,
            Tema    = dto.TemaId.HasValue
                        ? await _temaService.GetByIdAsync(dto.TemaId.Value)
                        : null,
            Usuario = dto.UsuarioId.HasValue
                        ? await _usuarioService.GetByIdAsync(dto.UsuarioId.Value)
                        : null
        };

        var created = await _postagemService.CreateAsync(postagem);
        if (created is null) return BadRequest();

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToDTO(created));
    }

    // PUT api/postagens/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] PostagemRequestDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var postagem = new Postagem
        {
            Id      = id,
            Titulo  = dto.Titulo,
            Texto   = dto.Texto,
            Tema    = dto.TemaId.HasValue
                        ? await _temaService.GetByIdAsync(dto.TemaId.Value)
                        : null,
            Usuario = dto.UsuarioId.HasValue
                        ? await _usuarioService.GetByIdAsync(dto.UsuarioId.Value)
                        : null
        };

        var updated = await _postagemService.UpdateAsync(postagem);
        if (updated is null) return NotFound();

        return Ok(MapToDTO(updated));
    }

    // DELETE api/postagens/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var deleted = await _postagemService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}