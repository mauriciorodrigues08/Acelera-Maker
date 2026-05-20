// Interface que define o contrato da camada de serviço de Postagens.
// Inclui operações de CRUD e filtros por autor e tema.

using BlogPessoal.Models;

namespace BlogPessoal.Services;

public interface IPostagemService
{
    // Retorna todas as postagens com Tema e Usuario incluídos
    Task<IEnumerable<Postagem>> GetAllAsync();

    // Retorna uma postagem pelo ID, ou null se não encontrar
    Task<Postagem?> GetByIdAsync(long id);

    // Retorna postagens filtradas pelo ID do autor
    Task<IEnumerable<Postagem>> GetByAutorAsync(long usuarioId);

    // Retorna postagens filtradas pelo ID do tema
    Task<IEnumerable<Postagem>> GetByTemaAsync(long temaId);

    // Cria uma nova postagem após validar regras de negócio
    // Retorna null se o Tema ou Usuario informado não existir
    Task<Postagem?> CreateAsync(Postagem postagem);

    // Atualiza uma postagem existente
    // Retorna null se a postagem não for encontrada
    Task<Postagem?> UpdateAsync(Postagem postagem);

    // Remove uma postagem pelo ID
    // Retorna false se a postagem não for encontrada
    Task<bool> DeleteAsync(long id);
}