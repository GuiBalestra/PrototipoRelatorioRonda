using AutoMapper;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;
using PrototipoRelatorioRonda.Services.Interface;

namespace PrototipoRelatorioRonda.Services;

public class VoltaRondaService : IVoltaRondaService
{
    private readonly IVoltaRondaRepository _voltaRondaRepository;
    private readonly IMapper _mapper;

    public VoltaRondaService(IVoltaRondaRepository voltaRondaRepository, IMapper mapper)
    {
        _voltaRondaRepository = voltaRondaRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VoltaRonda>> GetAllAsync()
    {
        return await _voltaRondaRepository.GetAllAsync();
    }

    public async Task<VoltaRonda?> GetByIdAsync(int id)
    {
        return await _voltaRondaRepository.GetByIdAsync(id);
    }

    public async Task<VoltaRonda> CreateAsync(VoltaRondaDTO voltaDto)
    {
        // Verificar se o relatório de ronda existe
        var relatorioExiste = await _voltaRondaRepository.RelatorioRondaExisteAsync(voltaDto.RelatorioRondaId);
        if (!relatorioExiste)
        {
            throw new KeyNotFoundException("Relatório de ronda não encontrado");
        }

        // Verificar se o número da volta já existe para este relatório
        var numeroVoltaExiste = await _voltaRondaRepository.NumeroVoltaExisteAsync(
            voltaDto.RelatorioRondaId, voltaDto.NumeroVolta);
        if (numeroVoltaExiste)
        {
            throw new InvalidOperationException("Número da volta já existe para este relatório");
        }

        var volta = _mapper.Map<VoltaRonda>(voltaDto);
        return await _voltaRondaRepository.AddAsync(volta);
    }

    public async Task UpdateAsync(int id, VoltaRondaDTO voltaDto)
    {
        var volta = await _voltaRondaRepository.GetByIdAsync(id);
        if (volta is null)
        {
            throw new KeyNotFoundException("Volta não encontrada");
        }

        // Verificar se o relatório de ronda existe
        var relatorioExiste = await _voltaRondaRepository.RelatorioRondaExisteAsync(voltaDto.RelatorioRondaId);
        if (!relatorioExiste)
        {
            throw new KeyNotFoundException("Relatório de ronda não encontrado");
        }

        // Verificar se o número da volta já existe para este relatório (excluindo a volta atual)
        var numeroVoltaExiste = await _voltaRondaRepository.NumeroVoltaExisteAsync(
            voltaDto.RelatorioRondaId, voltaDto.NumeroVolta, id);
        if (numeroVoltaExiste)
        {
            throw new InvalidOperationException("Número da volta já existe para este relatório");
        }

        // Atualizar campos
        volta.RelatorioRondaId = voltaDto.RelatorioRondaId;
        volta.NumeroVolta = voltaDto.NumeroVolta;
        volta.HoraSaida = voltaDto.HoraSaida;
        volta.HoraChegada = voltaDto.HoraChegada;
        volta.HoraDescanso = voltaDto.HoraDescanso;
        volta.Observacoes = voltaDto.Observacoes;

        await _voltaRondaRepository.UpdateAsync(volta);
    }

    public async Task DeleteAsync(int id)
    {
        var volta = await _voltaRondaRepository.GetByIdAsync(id);
        if (volta is null)
        {
            throw new KeyNotFoundException("Volta não encontrada");
        }

        await _voltaRondaRepository.DeleteAsync(volta);
    }

    public async Task DesativarAsync(int id)
    {
        var volta = await _voltaRondaRepository.GetByIdAsync(id);
        if (volta is null)
        {
            throw new KeyNotFoundException("Volta não encontrada");
        }

        await _voltaRondaRepository.DesativarAsync(volta);
    }

    public async Task<IEnumerable<VoltaRonda>> GetAllWithRelatorioAsync()
    {
        return await _voltaRondaRepository.GetAllWithRelatorioAsync();
    }

    public async Task<VoltaRonda?> GetByIdWithRelatorioAsync(int id)
    {
        return await _voltaRondaRepository.GetByIdWithRelatorioAsync(id);
    }

    public async Task<IEnumerable<VoltaRonda>> GetByRelatorioRondaAsync(int relatorioRondaId)
    {
        return await _voltaRondaRepository.GetByRelatorioRondaAsync(relatorioRondaId);
    }
} 
