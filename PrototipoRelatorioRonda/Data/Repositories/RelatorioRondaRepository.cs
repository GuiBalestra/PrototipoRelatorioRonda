using Microsoft.EntityFrameworkCore;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;

namespace PrototipoRelatorioRonda.Data.Repositories;

public class RelatorioRondaRepository : BaseRepository<RelatorioRonda>, IRelatorioRondaRepository
{
    public RelatorioRondaRepository(RelatorioRondaContext context) : base(context) { }

    public async Task<bool> EmpresaExisteAsync(int empresaId)
    {
        return await _context.Empresas.AnyAsync(e => e.Id == empresaId && e.Ativo);
    }

    public async Task<bool> VigilanteExisteAsync(int vigilanteId)
    {
        return await _context.Usuarios.AnyAsync(u => u.Id == vigilanteId && u.Ativo);
    }

    public async Task<bool> RelatorioExisteParaDataAsync(int empresaId, int vigilanteId, DateTime data)
    {
        return await _context.RelatorioRondas.AnyAsync(r => 
            r.EmpresaId == empresaId && 
            r.VigilanteId == vigilanteId && 
            r.Data.Date == data.Date && 
            r.Ativo);
    }

    public async Task<RelatorioRonda?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.RelatorioRondas
            .Include(r => r.Empresa)
            .Include(r => r.Vigilante)
            .Include(r => r.Voltas)
            .FirstOrDefaultAsync(r => r.Id == id && r.Ativo);
    }

    public async Task<IEnumerable<RelatorioRonda>> GetAllWithDetailsAsync()
    {
        return await _context.RelatorioRondas
            .Include(r => r.Empresa)
            .Include(r => r.Vigilante)
            .Include(r => r.Voltas)
            .Where(r => r.Ativo)
            .ToListAsync();
    }

    public async Task<IEnumerable<RelatorioRonda>> GetByEmpresaAsync(int empresaId)
    {
        return await _context.RelatorioRondas
            .Include(r => r.Empresa)
            .Include(r => r.Vigilante)
            .Include(r => r.Voltas)
            .Where(r => r.EmpresaId == empresaId && r.Ativo)
            .ToListAsync();
    }

    public async Task<IEnumerable<RelatorioRonda>> GetByVigilanteAsync(int vigilanteId)
    {
        return await _context.RelatorioRondas
            .Include(r => r.Empresa)
            .Include(r => r.Vigilante)
            .Include(r => r.Voltas)
            .Where(r => r.VigilanteId == vigilanteId && r.Ativo)
            .ToListAsync();
    }

    public async Task<IEnumerable<RelatorioRonda>> GetByDataAsync(DateTime data)
    {
        return await _context.RelatorioRondas
            .Include(r => r.Empresa)
            .Include(r => r.Vigilante)
            .Include(r => r.Voltas)
            .Where(r => r.Data.Date == data.Date && r.Ativo)
            .ToListAsync();
    }
}
