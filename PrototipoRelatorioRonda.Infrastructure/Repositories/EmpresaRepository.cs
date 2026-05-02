using Microsoft.EntityFrameworkCore;
using PrototipoRelatorioRonda.Application.Interfaces;
using PrototipoRelatorioRonda.Domain.Interfaces;
using PrototipoRelatorioRonda.Domain.Entities;
using PrototipoRelatorioRonda.Infrastructure.Data;

namespace PrototipoRelatorioRonda.Infrastructure.Repositories;

public class EmpresaRepository : BaseRepository<Empresa>, IEmpresaRepository
{
    public EmpresaRepository(RelatorioRondaContext context) : base(context) { }

    public async Task<bool> NomeExisteAsync(string nome, int? idExcluir = null)
    {
        var query = _context.Empresas.Where(e => e.Nome == nome && e.Ativo);
        
        if (idExcluir.HasValue)
            query = query.Where(e => e.Id != idExcluir.Value);
            
        return await query.AnyAsync();
    }
}
