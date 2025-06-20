using PrototipoRelatorioRonda.Models;

namespace PrototipoRelatorioRonda.Data.Interface;

public interface IRelatorioRondaRepository : IBaseRepository<RelatorioRonda>
{
    Task<bool> EmpresaExisteAsync(int empresaId);
    Task<bool> VigilanteExisteAsync(int vigilanteId);
    Task<bool> RelatorioExisteParaDataAsync(int empresaId, int vigilanteId, DateTime data);
    Task<RelatorioRonda?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<RelatorioRonda>> GetAllWithDetailsAsync();
    Task<IEnumerable<RelatorioRonda>> GetByEmpresaAsync(int empresaId);
    Task<IEnumerable<RelatorioRonda>> GetByVigilanteAsync(int vigilanteId);
    Task<IEnumerable<RelatorioRonda>> GetByDataAsync(DateTime data);
}
