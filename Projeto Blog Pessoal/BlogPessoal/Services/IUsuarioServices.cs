// Interface que define o contrato da camada de serviço de Usuários.
// Inclui operações de CRUD e autenticação com geração de token JWT.

using BlogPessoal.Models;

namespace BlogPessoal.Services;

public interface IUsuarioService
{
    // Retorna todos os usuários cadastrados
    Task<IEnumerable<Usuario>> GetAllAsync();

    // Retorna um usuário pelo ID, ou null se não encontrar
    Task<Usuario?> GetByIdAsync(long id);

    // Cadastra um novo usuário após validar regras de negócio
    // Retorna null se o email já estiver em uso
    Task<Usuario?> CreateAsync(Usuario usuario);

    // Atualiza os dados de um usuário existente
    // Retorna null se o usuário não for encontrado
    Task<Usuario?> UpdateAsync(Usuario usuario);

    // Remove um usuário pelo ID
    // Retorna false se o usuário não for encontrado
    Task<bool> DeleteAsync(long id);

    // Autentica o usuário e retorna o token JWT
    // Retorna null se as credenciais forem inválidas
    Task<string?> LoginAsync(UsuarioLogin usuarioLogin);
}