// Implementação do repositório de Usuários.
// Realiza as operações de acesso ao banco de dados utilizando o Entity Framework Core.

using BlogPessoal.Data;
using BlogPessoal.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogPessoal.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    // Injeta o DbContext para acessar o banco de dados
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    // Busca todos os usuários da tabela tb_usuarios
    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios.ToListAsync();
    }

    // Busca um usuário pelo ID usando FirstOrDefault, retorna null caso não encontre
    public async Task<Usuario?> GetByIdAsync(long id)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    // Busca um usuário pelo email usado no login para localizar o usuário antes de validar a senha
    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    // Adiciona o usuário ao contexto e persiste no banco
    // O EF Core preenche o Id automaticamente após a inserção
    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    // Marca o usuário como modificado no contexto e persiste as alterações
    public async Task<Usuario> UpdateAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    // Busca o usuário pelo ID e, se existir, remove do contexto e persiste
    public async Task DeleteAsync(long id)
    {
        var usuario = await GetByIdAsync(id);
        if (usuario is not null)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }

    // Usa AnyAsync para verificar existência sem precisar carregar o objeto inteiro da memória
    public async Task<bool> ExistsAsync(long id)
    {
        return await _context.Usuarios.AnyAsync(u => u.Id == id);
    }

    // Verifica se o email já está cadastrado antes de criar um novo usuário
    // Evita duplicações na base de dados
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Usuarios.AnyAsync(u => u.Email == email);
    }
}