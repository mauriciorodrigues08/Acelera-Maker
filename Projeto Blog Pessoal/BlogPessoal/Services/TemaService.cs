// Implementação da camada de serviço de Temas.
// Aplica as regras de negócio antes de delegar as operações ao repositório.

using BlogPessoal.Models;
using BlogPessoal.Repositories;

namespace BlogPessoal.Services;

public class TemaService : ITemaService
{
    // Injeta o repositório de temas via injeção de dependência
    private readonly ITemaRepository _temaRepository;

    public TemaService(ITemaRepository temaRepository)
    {
        _temaRepository = temaRepository;
    }

    // Delega a busca de todos os temas ao repositório
    public async Task<IEnumerable<Tema>> GetAllAsync()
    {
        return await _temaRepository.GetAllAsync();
    }

    // Delega a busca por ID ao repositório
    public async Task<Tema?> GetByIdAsync(long id)
    {
        return await _temaRepository.GetByIdAsync(id);
    }

    // Cria um novo tema sem regras adicionais por enquanto
    public async Task<Tema> CreateAsync(Tema tema)
    {
        return await _temaRepository.CreateAsync(tema);
    }

    // Verifica se o tema existe antes de atualizar
    // Retorna null se o tema não for encontrado
    public async Task<Tema?> UpdateAsync(Tema tema)
    {
        var exists = await _temaRepository.ExistsAsync(tema.Id);
        if (!exists) return null;

        return await _temaRepository.UpdateAsync(tema);
    }

    // Verifica se o tema existe antes de deletar
    // Retorna false se o tema não for encontrado
    public async Task<bool> DeleteAsync(long id)
    {
        var exists = await _temaRepository.ExistsAsync(id);
        if (!exists) return false;

        await _temaRepository.DeleteAsync(id);
        return true;
    }
}