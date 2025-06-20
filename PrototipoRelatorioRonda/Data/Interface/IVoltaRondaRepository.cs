using PrototipoRelatorioRonda.Models;

namespace PrototipoRelatorioRonda.Data.Interface;

public interface IVoltaRondaRepository : IBaseRepository<VoltaRonda>
{
    Task<bool> RelatorioRondaExisteAsync(int relatorioRondaId);
    Task<bool> NumeroVoltaExisteAsync(int relatorioRondaId, int numeroVolta, int? idExcluir = null);
    Task<VoltaRonda?> GetByIdWithRelatorioAsync(int id);
    Task<IEnumerable<VoltaRonda>> GetAllWithRelatorioAsync();
    Task<IEnumerable<VoltaRonda>> GetByRelatorioRondaAsync(int relatorioRondaId);
} 
