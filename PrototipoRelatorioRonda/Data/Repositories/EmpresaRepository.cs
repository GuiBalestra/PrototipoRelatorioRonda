using Microsoft.EntityFrameworkCore;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;

namespace PrototipoRelatorioRonda.Data.Repositories;

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
