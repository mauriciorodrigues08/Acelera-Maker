// Implementação do repositório de Postagens.
// Utiliza Include para carregar os dados relacionados de Tema e Usuario junto com cada postagem.

using BlogPessoal.Data;
using BlogPessoal.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogPessoal.Repositories;

public class PostagemRepository : IPostagemRepository
{
    // Injeta o DbContext para acessar o banco de dados
    private readonly AppDbContext _context;

    public PostagemRepository(AppDbContext context)
    {
        _context = context;
    }

    // Busca todas as postagens incluindo os dados de Tema e Usuario
    // Include faz o JOIN automaticamente, evitando múltiplas queries
    public async Task<IEnumerable<Postagem>> GetAllAsync()
    {
        return await _context.Postagens
            .Include(p => p.Tema)
            .Include(p => p.Usuario)
            .ToListAsync();
    }

    // Busca uma postagem pelo ID já com Tema e Usuario carregados
    public async Task<Postagem?> GetByIdAsync(long id)
    {
        return await _context.Postagens
            .Include(p => p.Tema)
            .Include(p => p.Usuario)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    // Filtra postagens pelo ID do usuário autor
    // Útil para exibir apenas as postagens de um determinado autor
    public async Task<IEnumerable<Postagem>> GetByAutorAsync(long usuarioId)
    {
        return await _context.Postagens
            .Include(p => p.Tema)
            .Include(p => p.Usuario)
            .Where(p => p.Usuario!.Id == usuarioId)
            .ToListAsync();
    }

    // Filtra postagens pelo ID do tema
    // Útil para listar todas as postagens de uma categoria
    public async Task<IEnumerable<Postagem>> GetByTemaAsync(long temaId)
    {
        return await _context.Postagens
            .Include(p => p.Tema)
            .Include(p => p.Usuario)
            .Where(p => p.Tema!.Id == temaId)
            .ToListAsync();
    }

    // Adiciona a postagem ao contexto e persiste no banco
    // O EF Core preenche o Id e a Data automaticamente
    public async Task<Postagem> CreateAsync(Postagem postagem)
    {
        // Busca os objetos reais do banco pelo ID
        // evitando que o EF Core tente inserir duplicatas
        if (postagem.Tema is not null)
            postagem.Tema = await _context.Temas
                .FindAsync(postagem.Tema.Id);

        if (postagem.Usuario is not null)
            postagem.Usuario = await _context.Usuarios
                .FindAsync(postagem.Usuario.Id);

        _context.Postagens.Add(postagem);
        await _context.SaveChangesAsync();
        return postagem;
    }

    public async Task<Postagem> UpdateAsync(Postagem postagem)
    {
        // Busca os objetos reais do banco pelo ID
        // evitando que o EF Core tente inserir duplicatas
        if (postagem.Tema is not null)
            postagem.Tema = await _context.Temas
                .FindAsync(postagem.Tema.Id);

        if (postagem.Usuario is not null)
            postagem.Usuario = await _context.Usuarios
                .FindAsync(postagem.Usuario.Id);

        _context.Postagens.Update(postagem);
        await _context.SaveChangesAsync();
        return postagem;
    }

    // Busca a postagem pelo ID e, se existir, remove do contexto e persiste
    public async Task DeleteAsync(long id)
    {
        var postagem = await GetByIdAsync(id);
        if (postagem is not null)
        {
            _context.Postagens.Remove(postagem);
            await _context.SaveChangesAsync();
        }
    }

    // Usa AnyAsync para verificar existência de forma eficiente,
    // sem precisar carregar o objeto inteiro da memória
    public async Task<bool> ExistsAsync(long id)
    {
        return await _context.Postagens.AnyAsync(p => p.Id == id);
    }
}