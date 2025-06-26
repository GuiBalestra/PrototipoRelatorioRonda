using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;

namespace PrototipoRelatorioRonda.Services.Interface;

public interface IRelatorioRondaService
{
    Task<IEnumerable<RelatorioRonda>> GetAllAsync();
    Task<RelatorioRonda?> GetByIdAsync(int id);
    Task<RelatorioRonda> CreateAsync(RelatorioRondaDTO relatorioDto);
    Task UpdateAsync(int id, RelatorioRondaDTO relatorioDto);
    Task DeleteAsync(int id);
    Task DesativarAsync(int id);
    Task<IEnumerable<RelatorioRonda>> GetAllWithDetailsAsync();
    Task<RelatorioRonda?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<RelatorioRonda>> GetByEmpresaAsync(int empresaId);
    Task<IEnumerable<RelatorioRonda>> GetByVigilanteAsync(int vigilanteId);
    Task<IEnumerable<RelatorioRonda>> GetByDataAsync(DateTime data);
} 
