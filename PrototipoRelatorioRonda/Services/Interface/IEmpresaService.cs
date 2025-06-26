using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;

namespace PrototipoRelatorioRonda.Services.Interface;

public interface IEmpresaService
{
    Task<IEnumerable<Empresa>> GetAllAsync();
    Task<Empresa?> GetByIdAsync(int id);
    Task<Empresa> CreateAsync(EmpresaDTO empresaDto);
    Task UpdateAsync(int id, EmpresaDTO empresaDto);
    Task DeleteAsync(int id);
    Task DesativarAsync(int id);
} 
