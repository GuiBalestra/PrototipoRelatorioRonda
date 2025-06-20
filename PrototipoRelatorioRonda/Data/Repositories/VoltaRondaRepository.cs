using Microsoft.EntityFrameworkCore;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;

namespace PrototipoRelatorioRonda.Data.Repositories;

public class VoltaRondaRepository : BaseRepository<VoltaRonda>, IVoltaRondaRepository
{
    public VoltaRondaRepository(RelatorioRondaContext context) : base(context) { }

    public async Task<bool> RelatorioRondaExisteAsync(int relatorioRondaId)
    {
        return await _context.RelatorioRondas.AnyAsync(r => r.Id == relatorioRondaId && r.Ativo);
    }

    public async Task<bool> NumeroVoltaExisteAsync(int relatorioRondaId, int numeroVolta, int? idExcluir = null)
    {
        var query = _context.VoltaRondas.Where(v => 
            v.RelatorioRondaId == relatorioRondaId && 
            v.NumeroVolta == numeroVolta && 
            v.Ativo);
        
        if (idExcluir.HasValue)
            query = query.Where(v => v.Id != idExcluir.Value);
            
        return await query.AnyAsync();
    }

    public async Task<VoltaRonda?> GetByIdWithRelatorioAsync(int id)
    {
        return await _context.VoltaRondas
            .Include(v => v.RelatorioRonda)
            .FirstOrDefaultAsync(v => v.Id == id && v.Ativo);
    }

    public async Task<IEnumerable<VoltaRonda>> GetAllWithRelatorioAsync()
    {
        return await _context.VoltaRondas
            .Include(v => v.RelatorioRonda)
            .Where(v => v.Ativo)
            .ToListAsync();
    }

    public async Task<IEnumerable<VoltaRonda>> GetByRelatorioRondaAsync(int relatorioRondaId)
    {
        return await _context.VoltaRondas
            .Include(v => v.RelatorioRonda)
            .Where(v => v.RelatorioRondaId == relatorioRondaId && v.Ativo)
            .OrderBy(v => v.NumeroVolta)
            .ToListAsync();
    }
} 
