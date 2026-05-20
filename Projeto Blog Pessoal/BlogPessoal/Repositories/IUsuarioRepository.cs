// Interface que define o contrato do repositório de Usuários.
// Além do CRUD básico, inclui busca por email, necessária para o processo de autenticação.

using BlogPessoal.Models;

namespace BlogPessoal.Repositories;

public interface IUsuarioRepository
{
    // Retorna todos os usuários cadastrados no banco
    Task<IEnumerable<Usuario>> GetAllAsync();

    // Retorna um usuário pelo ID, ou null se não encontrar
    Task<Usuario?> GetByIdAsync(long id);

    // Retorna um usuário pelo email, usado na autenticação
    Task<Usuario?> GetByEmailAsync(string email);

    // Insere um novo usuário no banco e retorna o usuário criado
    Task<Usuario> CreateAsync(Usuario usuario);

    // Atualiza os dados de um usuário existente e retorna o usuário atualizado
    Task<Usuario> UpdateAsync(Usuario usuario);

    // Remove um usuário do banco pelo ID
    Task DeleteAsync(long id);

    // Verifica se um usuário com o ID informado existe no banco
    Task<bool> ExistsAsync(long id);

    // Verifica se já existe um usuário cadastrado com o email informado
    Task<bool> EmailExistsAsync(string email);
}