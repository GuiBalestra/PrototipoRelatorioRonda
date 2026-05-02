using PrototipoRelatorioRonda.Domain.Entities;
using PrototipoRelatorioRonda.Application.DTOs;

namespace PrototipoRelatorioRonda.Application.Interfaces;

public interface IVoltaRondaService
{
    Task<IEnumerable<VoltaRonda>> GetAllAsync();
    Task<VoltaRonda?> GetByIdAsync(int id);
    Task<VoltaRonda> CreateAsync(VoltaRondaDTO voltaDto);
    Task UpdateAsync(int id, VoltaRondaDTO voltaDto);
    Task DeleteAsync(int id);
    Task DesativarAsync(int id);
    Task<IEnumerable<VoltaRonda>> GetAllWithRelatorioAsync();
    Task<VoltaRonda?> GetByIdWithRelatorioAsync(int id);
    Task<IEnumerable<VoltaRonda>> GetByRelatorioRondaAsync(int relatorioRondaId);
} 
