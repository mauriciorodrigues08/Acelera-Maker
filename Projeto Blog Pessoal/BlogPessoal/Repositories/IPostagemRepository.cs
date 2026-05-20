// Interface que define o contrato do repositório de Postagens.
// Além do CRUD básico, inclui filtros por autor e tema.

using BlogPessoal.Models;

namespace BlogPessoal.Repositories;

public interface IPostagemRepository
{
    // Retorna todas as postagens com os dados de Tema e Usuario incluídos
    Task<IEnumerable<Postagem>> GetAllAsync();

    // Retorna uma postagem pelo ID com Tema e Usuario incluídos
    Task<Postagem?> GetByIdAsync(long id);

    // Retorna postagens filtradas por autor (ID do usuário)
    Task<IEnumerable<Postagem>> GetByAutorAsync(long usuarioId);

    // Retorna postagens filtradas por tema (ID do tema)
    Task<IEnumerable<Postagem>> GetByTemaAsync(long temaId);

    // Insere uma nova postagem no banco e retorna a postagem criada
    Task<Postagem> CreateAsync(Postagem postagem);

    // Atualiza os dados de uma postagem existente e retorna a postagem atualizada
    Task<Postagem> UpdateAsync(Postagem postagem);

    // Remove uma postagem do banco pelo ID
    Task DeleteAsync(long id);

    // Verifica se uma postagem com o ID informado existe no banco
    Task<bool> ExistsAsync(long id);
}