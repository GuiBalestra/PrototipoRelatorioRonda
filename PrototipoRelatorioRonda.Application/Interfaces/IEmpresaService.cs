using PrototipoRelatorioRonda.Domain.Entities;
using PrototipoRelatorioRonda.Application.DTOs;

namespace PrototipoRelatorioRonda.Application.Interfaces;

public interface IEmpresaService
{
    Task<IEnumerable<Empresa>> GetAllAsync();
    Task<Empresa?> GetByIdAsync(int id);
    Task<Empresa> CreateAsync(EmpresaDTO empresaDto);
    Task UpdateAsync(int id, EmpresaDTO empresaDto);
    Task DeleteAsync(int id);
    Task DesativarAsync(int id);
} 
