// Implementação do repositório de Temas.
// Realiza as operações de acesso ao banco de dados utilizando o Entity Framework Core.

using BlogPessoal.Data;
using BlogPessoal.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogPessoal.Repositories;

public class TemaRepository : ITemaRepository
{
    // Injeta o DbContext para acessar o banco de dados
    private readonly AppDbContext _context;

    public TemaRepository(AppDbContext context)
    {
        _context = context;
    }

    // Busca todos os temas da tabela tb_temas
    public async Task<IEnumerable<Tema>> GetAllAsync()
    {
        return await _context.Temas.ToListAsync();
    }

    // Busca um tema pelo ID usando FirstOrDefault, que retorna null caso não encontre
    public async Task<Tema?> GetByIdAsync(long id)
    {
        return await _context.Temas.FirstOrDefaultAsync(t => t.Id == id);
    }

    // Adiciona o tema ao contexto e persiste no banco com SaveChanges 
    // O EF Core preenche o Id automaticamente após a inserção
    public async Task<Tema> CreateAsync(Tema tema)
    {
        _context.Temas.Add(tema);
        await _context.SaveChangesAsync();
        return tema;
    }

    // Marca o tema como modificado no contexto e persiste as alterações
    public async Task<Tema> UpdateAsync(Tema tema)
    {
        _context.Temas.Update(tema);
        await _context.SaveChangesAsync();
        return tema;
    }

    // Busca o tema pelo ID e, se existir, remove do contexto e persiste
    public async Task DeleteAsync(long id)
    {
        var tema = await GetByIdAsync(id);
        if (tema is not null)
        {
            _context.Temas.Remove(tema);
            await _context.SaveChangesAsync();
        }
    }

    // Usa AnyAsync para verificar existência sem precisar carregar o objeto inteiro da memória
    public async Task<bool> ExistsAsync(long id)
    {
        return await _context.Temas.AnyAsync(t => t.Id == id);
    }
}