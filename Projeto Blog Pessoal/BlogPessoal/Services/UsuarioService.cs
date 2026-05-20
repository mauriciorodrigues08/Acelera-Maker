// Implementação da camada de serviço de Usuários.
// Responsável por aplicar as regras de negócio, incluindo hash de senha e geração de token JWT.

using BlogPessoal.Models;
using BlogPessoal.Repositories;

namespace BlogPessoal.Services;

public class UsuarioService : IUsuarioService
{
    // Injeta o repositório de usuários e o serviço JWT
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly JwtService _jwtService;

    public UsuarioService(IUsuarioRepository usuarioRepository, JwtService jwtService)
    {
        _usuarioRepository = usuarioRepository;
        _jwtService = jwtService;
    }

    // Delega a busca de todos os usuários ao repositório
    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _usuarioRepository.GetAllAsync();
    }

    // Delega a busca por ID ao repositório
    public async Task<Usuario?> GetByIdAsync(long id)
    {
        return await _usuarioRepository.GetByIdAsync(id);
    }

    // Cadastra um novo usuário aplicando as regras de negócio:
    // 1. Verifica se o email já está em uso
    // 2. Gera o hash da senha antes de salvar no banco
    public async Task<Usuario?> CreateAsync(Usuario usuario)
    {
        // Impede cadastro duplicado pelo mesmo email
        var emailEmUso = await _usuarioRepository.EmailExistsAsync(usuario.Email!);
        if (emailEmUso) return null;

        // Substitui a senha em texto puro pelo hash gerado pelo BCrypt
        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

        return await _usuarioRepository.CreateAsync(usuario);
    }

    // Atualiza os dados do usuário:
    // 1. Verifica se o usuário existe
    // 2. Gera novo hash caso a senha tenha sido alterada
    public async Task<Usuario?> UpdateAsync(Usuario usuario)
    {
        var exists = await _usuarioRepository.ExistsAsync(usuario.Id);
        if (!exists) return null;

        // Regera o hash da senha ao atualizar
        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

        return await _usuarioRepository.UpdateAsync(usuario);
    }

    // Verifica se o usuário existe antes de deletar
    public async Task<bool> DeleteAsync(long id)
    {
        var exists = await _usuarioRepository.ExistsAsync(id);
        if (!exists) return false;

        await _usuarioRepository.DeleteAsync(id);
        return true;
    }

    // Autentica o usuário em duas etapas:
    // 1. Busca o usuário pelo email
    // 2. Verifica se a senha informada corresponde ao hash salvo
    // Se válido, gera e retorna o token JWT
    public async Task<string?> LoginAsync(UsuarioLogin usuarioLogin)
    {
        // Busca o usuário pelo email informado
        var usuario = await _usuarioRepository.GetByEmailAsync(usuarioLogin.Email!);
        if (usuario is null) return null;

        // Compara a senha informada com o hash armazenado no banco
        var senhaValida = BCrypt.Net.BCrypt.Verify(usuarioLogin.Senha, usuario.Senha);
        if (!senhaValida) return null;

        // Gera e retorna o token JWT com os dados do usuário
        return _jwtService.GerarToken(usuario);
    }
}