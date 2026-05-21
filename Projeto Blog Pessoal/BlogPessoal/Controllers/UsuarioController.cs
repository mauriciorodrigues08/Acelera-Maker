// Controller responsável pelos endpoints de Usuários.
// Gerencia cadastro, atualização, exclusão e autenticação.
// O endpoint de cadastro e login são públicos (sem JWT).
// Os demais endpoints são protegidos por autenticação JWT.

using BlogPessoal.Models;
using BlogPessoal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    // GET api/usuarios
    // Retorna todos os usuários cadastrados
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // busca todos os usuários
        var usuarios = await _usuarioService.GetAllAsync();

        // retorna 200 com a lista de usuários
        return Ok(usuarios);
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

        // retorna 200 com o usuário encontrado
        return Ok(usuario);
    }

    // POST api/usuarios/cadastrar
    // Cadastra um novo usuário (endpoint público)
    [HttpPost("cadastrar")]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] Usuario usuario)
    {
        // retorna 400 se os dados enviados não passarem nas validações do modelo
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // tenta criar o usuário
        var created = await _usuarioService.CreateAsync(usuario);

        // retorna 409 se o email já estiver em uso
        if (created is null) return Conflict("Email já cadastrado.");

        // retorna 201 com o usuário criado
        return CreatedAtAction(
                            nameof(GetById),
                            new { id = created.Id },
                            created
                            );
    }

    // PUT api/usuarios/{id}
    // Atualiza os dados de um usuário existente
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] Usuario usuario)
    {
        // retorna 400 se os dados enviados não passarem nas validações do modelo
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // garante que o ID da rota é o mesmo do corpo da requisição
        usuario.Id = id;

        // tenta atualizar o usuário
        var updated = await _usuarioService.UpdateAsync(usuario);
        // retorna 404 caso o usuário não for encontrado
        if (updated is null) return NotFound();
        // retorna 200 com o usuário atualizado
        return Ok(updated);
    }

    // DELETE api/usuarios/{id}
    // Remove um usuário pelo ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        // tenta deletar o usuário
        var deleted = await _usuarioService.DeleteAsync(id);
        
        // retorna 404 caso o usuário não for encontrado
        if (!deleted) return NotFound();

        // retorna 204 se for excluído com sucesso
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