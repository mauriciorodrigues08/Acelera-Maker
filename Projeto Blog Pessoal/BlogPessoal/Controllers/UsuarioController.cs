// Controller responsável pelos endpoints de Usuários.
// Gerencia cadastro, atualização, exclusão e autenticação.
// O endpoint de cadastro e login são públicos (sem JWT).
// Os demais endpoints são protegidos por autenticação JWT.

using BlogPessoal.DTOs;
using BlogPessoal.Models;
using BlogPessoal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogPessoal.Controllers;

[Authorize]
[ApiController]
[Route("api/usuarios")]
public class UsuarioController : ControllerBase
{
    // Injeta o serviço de usuários via injeção de dependência
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    // mapeia a entidade Usuario para o DTO de resposta sem expor a senha
    private static UsuarioResponseDTO MapToDTO(Usuario u) => new()
    {
        Id = u.Id,
        Nome = u.Nome,
        Email = u.Email,
        Foto = u.Foto
    };

    // GET api/usuarios
    // Retorna todos os usuários cadastrados
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // busca todos os usuários
        var usuarios = await _usuarioService.GetAllAsync();
        // converte cada usuario para o DTO antes de retornar
        return Ok(usuarios.Select(MapToDTO));
    }

    // GET api/usuarios/{id}
    // Retorna um usuário pelo ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        // busca o usuário pelo ID
        var usuario = await _usuarioService.GetByIdAsync(id);

        // retorna 404 caso o usuário não for encontrado
        if (usuario is null) return NotFound();

        // converte para DTO antes de retornar
        return Ok(MapToDTO(usuario));
    }

    // POST api/usuarios/cadastrar
    // Cadastra um novo usuário (endpoint público)
    [HttpPost("cadastrar")]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] UsuarioRequestDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var usuario = new Usuario
        {
            Nome  = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha,
            Foto  = dto.Foto
        };

        var created = await _usuarioService.CreateAsync(usuario);
        if (created is null) return Conflict("Email já cadastrado.");

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToDTO(created));
    }

    // PUT api/usuarios/{id}
    // Atualiza os dados de um usuário existente
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UsuarioRequestDTO dto)
    {
        // verifica se o usuário autenticado é o dono do recurso
        var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        if (userId != id) return Forbid();

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var usuario = new Usuario
        {
            Id    = id,      // ID vem da rota, não do body
            Nome  = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha,
            Foto  = dto.Foto
        };

        var updated = await _usuarioService.UpdateAsync(usuario);
        if (updated is null) return NotFound();

        return Ok(MapToDTO(updated));
    }

    // DELETE api/usuarios/{id}
    // Remove um usuário pelo ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        // verifica se o usuário autenticado é o dono do recurso
        var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        if (userId != id) return Forbid();

        var deleted = await _usuarioService.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }

    // POST api/usuarios/login
    // Autentica o usuário e retorna o token JWT (Endpoint público)
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UsuarioLogin usuarioLogin)
    {
        // retorna 400 se os dados enviados não passarem nas validações do modelo
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // tenta autenticar o usuário e gerar o token
        var token = await _usuarioService.LoginAsync(usuarioLogin);

        // retorna 401 se as credenciais forem inválidas
        if (token is null) return Unauthorized("Email ou senha inválidos.");
        
        // retorna 200 com o token JWT gerado
        return Ok(new { token });
    }
}