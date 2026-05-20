// Interface que define o contrato da camada de serviço de Temas.
// Aqui ficam as regras de negócio, separadas do acesso ao banco.

using BlogPessoal.Models;

namespace BlogPessoal.Services;

public interface ITemaService
{
    // Retorna todos os temas cadastrados
    Task<IEnumerable<Tema>> GetAllAsync();

    // Retorna um tema pelo ID, ou null se não encontrar
    Task<Tema?> GetByIdAsync(long id);

    // Cria um novo tema após validar as regras de negócio
    Task<Tema> CreateAsync(Tema tema);

    // Atualiza um tema existente, retorna null se não encontrar
    Task<Tema?> UpdateAsync(Tema tema);

    // Remove um tema pelo ID, retorna false se não encontrar
    Task<bool> DeleteAsync(long id);
}