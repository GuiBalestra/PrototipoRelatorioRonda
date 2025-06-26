using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;
using PrototipoRelatorioRonda.Services.Interface;

namespace PrototipoRelatorioRonda.Services;

public class EmpresaService : IEmpresaService
{
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IMapper _mapper;

    public EmpresaService(IEmpresaRepository empresaRepository, IMapper mapper)
    {
        _empresaRepository = empresaRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Empresa>> GetAllAsync()
    {
        return await _empresaRepository.GetAllAsync();
    }

    public async Task<Empresa?> GetByIdAsync(int id)
    {
        return await _empresaRepository.GetByIdAsync(id);
    }

    public async Task<Empresa> CreateAsync(EmpresaDTO empresaDto)
    {
        // Verificar se o nome já existe
        var nomeExiste = await _empresaRepository.NomeExisteAsync(empresaDto.Nome);
        if (nomeExiste)
        {
            throw new InvalidOperationException("Nome da empresa já está em uso");
        }

        var empresa = _mapper.Map<Empresa>(empresaDto);
        return await _empresaRepository.AddAsync(empresa);
    }

    public async Task UpdateAsync(int id, EmpresaDTO empresaDto)
    {
        var empresa = await _empresaRepository.GetByIdAsync(id);
        if (empresa is null)
        {
            throw new KeyNotFoundException("Empresa não encontrada");
        }

        // Verificar se o nome já existe (excluindo a empresa atual)
        var nomeExiste = await _empresaRepository.NomeExisteAsync(empresaDto.Nome, id);
        if (nomeExiste)
        {
            throw new InvalidOperationException("Nome da empresa já está em uso");
        }

        // Atualizar apenas os campos necessários
        empresa.Nome = empresaDto.Nome;
        await _empresaRepository.UpdateAsync(empresa);
    }

    public async Task DeleteAsync(int id)
    {
        var empresa = await _empresaRepository.GetByIdAsync(id);
        if (empresa is null)
        {
            throw new KeyNotFoundException("Empresa não encontrada");
        }

        await _empresaRepository.DeleteAsync(empresa);
    }

    public async Task DesativarAsync(int id)
    {
        var empresa = await _empresaRepository.GetByIdAsync(id);
        if (empresa is null)
        {
            throw new KeyNotFoundException("Empresa não encontrada");
        }

        await _empresaRepository.DesativarAsync(empresa);
    }
} 
