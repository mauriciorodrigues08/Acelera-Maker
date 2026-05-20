// Interface que define o contrato do repositório de Temas.
// Segue o padrão Repository para desacoplar o acesso ao banco de dados da camada de negócio.

using BlogPessoal.Models;

namespace BlogPessoal.Repositories;

public interface ITemaRepository
{
    // Retorna todos os temas cadastrados no banco
    Task<IEnumerable<Tema>> GetAllAsync();

    // Retorna um tema pelo ID, ou null se não encontrar
    Task<Tema?> GetByIdAsync(long id);

    // Insere um novo tema no banco e retorna o tema criado com o ID gerado
    Task<Tema> CreateAsync(Tema tema);

    // Atualiza os dados de um tema existente e retorna o tema atualizado
    Task<Tema> UpdateAsync(Tema tema);

    // Remove um tema do banco pelo ID
    Task DeleteAsync(long id);

    // Verifica se um tema com o ID informado existe no banco
    Task<bool> ExistsAsync(long id);
}