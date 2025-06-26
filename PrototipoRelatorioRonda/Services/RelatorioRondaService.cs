using AutoMapper;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;
using PrototipoRelatorioRonda.Services.Interface;

namespace PrototipoRelatorioRonda.Services;

public class RelatorioRondaService : IRelatorioRondaService
{
    private readonly IRelatorioRondaRepository _relatorioRondaRepository;
    private readonly IMapper _mapper;

    public RelatorioRondaService(IRelatorioRondaRepository relatorioRondaRepository, IMapper mapper)
    {
        _relatorioRondaRepository = relatorioRondaRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RelatorioRonda>> GetAllAsync()
    {
        return await _relatorioRondaRepository.GetAllAsync();
    }

    public async Task<RelatorioRonda?> GetByIdAsync(int id)
    {
        return await _relatorioRondaRepository.GetByIdAsync(id);
    }

    public async Task<RelatorioRonda> CreateAsync(RelatorioRondaDTO relatorioDto)
    {
        // Verificar se a empresa existe
        var empresaExiste = await _relatorioRondaRepository.EmpresaExisteAsync(relatorioDto.EmpresaId);
        if (!empresaExiste)
        {
            throw new KeyNotFoundException("Empresa não encontrada");
        }

        // Verificar se o vigilante existe
        var vigilanteExiste = await _relatorioRondaRepository.VigilanteExisteAsync(relatorioDto.VigilanteId);
        if (!vigilanteExiste)
        {
            throw new KeyNotFoundException("Vigilante não encontrado");
        }

        // Verificar se já existe relatório para a mesma data, empresa e vigilante
        var relatorioExiste = await _relatorioRondaRepository.RelatorioExisteParaDataAsync(
            relatorioDto.EmpresaId, relatorioDto.VigilanteId, relatorioDto.Data);
        if (relatorioExiste)
        {
            throw new InvalidOperationException("Já existe um relatório para esta data, empresa e vigilante");
        }

        var relatorio = _mapper.Map<RelatorioRonda>(relatorioDto);
        return await _relatorioRondaRepository.AddAsync(relatorio);
    }

    public async Task UpdateAsync(int id, RelatorioRondaDTO relatorioDto)
    {
        var relatorio = await _relatorioRondaRepository.GetByIdAsync(id);
        if (relatorio is null)
        {
            throw new KeyNotFoundException("Relatório não encontrado");
        }

        // Verificar se a empresa existe
        var empresaExiste = await _relatorioRondaRepository.EmpresaExisteAsync(relatorioDto.EmpresaId);
        if (!empresaExiste)
        {
            throw new KeyNotFoundException("Empresa não encontrada");
        }

        // Verificar se o vigilante existe
        var vigilanteExiste = await _relatorioRondaRepository.VigilanteExisteAsync(relatorioDto.VigilanteId);
        if (!vigilanteExiste)
        {
            throw new KeyNotFoundException("Vigilante não encontrado");
        }

        // Verificar se já existe relatório para a mesma data, empresa e vigilante (excluindo o atual)
        var relatorioExiste = await _relatorioRondaRepository.RelatorioExisteParaDataAsync(
            relatorioDto.EmpresaId, relatorioDto.VigilanteId, relatorioDto.Data);
        if (relatorioExiste && (relatorio.EmpresaId != relatorioDto.EmpresaId || 
                               relatorio.VigilanteId != relatorioDto.VigilanteId || 
                               relatorio.Data.Date != relatorioDto.Data.Date))
        {
            throw new InvalidOperationException("Já existe um relatório para esta data, empresa e vigilante");
        }

        // Atualizar campos
        relatorio.EmpresaId = relatorioDto.EmpresaId;
        relatorio.VigilanteId = relatorioDto.VigilanteId;
        relatorio.Data = relatorioDto.Data;
        relatorio.KmSaida = relatorioDto.KmSaida;
        relatorio.KmChegada = relatorioDto.KmChegada;
        relatorio.TestemunhaSaida = relatorioDto.TestemunhaSaida;
        relatorio.TestemunhaChegada = relatorioDto.TestemunhaChegada;

        await _relatorioRondaRepository.UpdateAsync(relatorio);
    }

    public async Task DeleteAsync(int id)
    {
        var relatorio = await _relatorioRondaRepository.GetByIdAsync(id);
        if (relatorio is null)
        {
            throw new KeyNotFoundException("Relatório não encontrado");
        }

        await _relatorioRondaRepository.DeleteAsync(relatorio);
    }

    public async Task DesativarAsync(int id)
    {
        var relatorio = await _relatorioRondaRepository.GetByIdAsync(id);
        if (relatorio is null)
        {
            throw new KeyNotFoundException("Relatório não encontrado");
        }

        await _relatorioRondaRepository.DesativarAsync(relatorio);
    }

    public async Task<IEnumerable<RelatorioRonda>> GetAllWithDetailsAsync()
    {
        return await _relatorioRondaRepository.GetAllWithDetailsAsync();
    }

    public async Task<RelatorioRonda?> GetByIdWithDetailsAsync(int id)
    {
        return await _relatorioRondaRepository.GetByIdWithDetailsAsync(id);
    }

    public async Task<IEnumerable<RelatorioRonda>> GetByEmpresaAsync(int empresaId)
    {
        return await _relatorioRondaRepository.GetByEmpresaAsync(empresaId);
    }

    public async Task<IEnumerable<RelatorioRonda>> GetByVigilanteAsync(int vigilanteId)
    {
        return await _relatorioRondaRepository.GetByVigilanteAsync(vigilanteId);
    }

    public async Task<IEnumerable<RelatorioRonda>> GetByDataAsync(DateTime data)
    {
        return await _relatorioRondaRepository.GetByDataAsync(data);
    }
} 
