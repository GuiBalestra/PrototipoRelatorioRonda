using Microsoft.EntityFrameworkCore;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;

namespace PrototipoRelatorioRonda.Data.Repositories;

public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(RelatorioRondaContext context) : base(context) { }

    public async Task<bool> EmailExisteAsync(string email, int? idExcluir = null)
    {
        var query = _context.Usuarios.Where(u => u.Email == email && u.Ativo);
        
        if (idExcluir.HasValue)
            query = query.Where(u => u.Id != idExcluir.Value);
            
        return await query.AnyAsync();
    }

    public async Task<bool> NomeExisteAsync(string nome, int? idExcluir = null)
    {
        var query = _context.Usuarios.Where(u => u.Nome == nome && u.Ativo);
        
        if (idExcluir.HasValue)
            query = query.Where(u => u.Id != idExcluir.Value);
            
        return await query.AnyAsync();
    }

    public async Task<bool> EmpresaExisteAsync(int empresaId)
    {
        return await _context.Empresas.AnyAsync(e => e.Id == empresaId && e.Ativo);
    }

    public async Task<Usuario?> GetByIdWithEmpresaAsync(int id)
    {
        return await _context.Usuarios
            .Include(u => u.Empresa)
            .FirstOrDefaultAsync(u => u.Id == id && u.Ativo);
    }

    public async Task<IEnumerable<Usuario>> GetAllWithEmpresaAsync()
    {
        return await _context.Usuarios
            .Include(u => u.Empresa)
            .Where(u => u.Ativo)
            .ToListAsync();
    }
}
