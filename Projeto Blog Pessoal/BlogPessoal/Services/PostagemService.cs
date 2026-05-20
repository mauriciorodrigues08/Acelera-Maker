// Implementação da camada de serviço de Postagens.
// Valida a existência de Tema e Usuario antes de criar ou atualizar uma postagem.

using BlogPessoal.Models;
using BlogPessoal.Repositories;

namespace BlogPessoal.Services;

public class PostagemService : IPostagemService
{
    // Injeta os repositórios necessários via injeção de dependência
    private readonly IPostagemRepository _postagemRepository;
    private readonly ITemaRepository _temaRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public PostagemService(
        IPostagemRepository postagemRepository,
        ITemaRepository temaRepository,
        IUsuarioRepository usuarioRepository)
    {
        _postagemRepository = postagemRepository;
        _temaRepository = temaRepository;
        _usuarioRepository = usuarioRepository;
    }

    // Delega a busca de todas as postagens ao repositório
    public async Task<IEnumerable<Postagem>> GetAllAsync()
    {
        return await _postagemRepository.GetAllAsync();
    }

    // Delega a busca por ID ao repositório
    public async Task<Postagem?> GetByIdAsync(long id)
    {
        return await _postagemRepository.GetByIdAsync(id);
    }

    // Delega o filtro por autor ao repositório
    public async Task<IEnumerable<Postagem>> GetByAutorAsync(long usuarioId)
    {
        return await _postagemRepository.GetByAutorAsync(usuarioId);
    }

    // Delega o filtro por tema ao repositório
    public async Task<IEnumerable<Postagem>> GetByTemaAsync(long temaId)
    {
        return await _postagemRepository.GetByTemaAsync(temaId);
    }

    // Cria uma nova postagem aplicando as regras de negócio:
    // 1. Verifica se o Tema informado existe no banco
    // 2. Verifica se o Usuario informado existe no banco
    // 3. Só persiste se ambos existirem
    public async Task<Postagem?> CreateAsync(Postagem postagem)
    {
        // Valida se o tema existe antes de criar a postagem
        if (postagem.Tema is not null)
        {
            var temaExists = await _temaRepository.ExistsAsync(postagem.Tema.Id);
            if (!temaExists) return null;
        }

        // Valida se o usuário existe antes de criar a postagem
        if (postagem.Usuario is not null)
        {
            var usuarioExists = await _usuarioRepository.ExistsAsync(postagem.Usuario.Id);
            if (!usuarioExists) return null;
        }

        // Define a data de criação como o momento atual
        postagem.Data = DateTime.UtcNow;

        return await _postagemRepository.CreateAsync(postagem);
    }

    // Atualiza uma postagem aplicando as mesmas validações do Create:
    // 1. Verifica se a postagem existe
    // 2. Verifica se o Tema e Usuario informados existem
    public async Task<Postagem?> UpdateAsync(Postagem postagem)
    {
        // Verifica se a postagem que será atualizada existe
        var exists = await _postagemRepository.ExistsAsync(postagem.Id);
        if (!exists) return null;

        // Revalida o tema ao atualizar
        if (postagem.Tema is not null)
        {
            var temaExists = await _temaRepository.ExistsAsync(postagem.Tema.Id);
            if (!temaExists) return null;
        }

        // Revalida o usuário ao atualizar
        if (postagem.Usuario is not null)
        {
            var usuarioExists = await _usuarioRepository.ExistsAsync(postagem.Usuario.Id);
            if (!usuarioExists) return null;
        }

        // Atualiza a data para o momento da modificação
        postagem.Data = DateTime.UtcNow;

        return await _postagemRepository.UpdateAsync(postagem);
    }

    // Verifica se a postagem existe antes de deletar
    public async Task<bool> DeleteAsync(long id)
    {
        var exists = await _postagemRepository.ExistsAsync(id);
        if (!exists) return false;

        await _postagemRepository.DeleteAsync(id);
        return true;
    }
}