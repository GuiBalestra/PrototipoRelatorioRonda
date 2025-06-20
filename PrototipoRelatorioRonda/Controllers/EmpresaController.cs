using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;

namespace PrototipoRelatorioRonda.Controllers;

[Route("[controller]")]
[ApiController]
public class EmpresaController : ControllerBase
{
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IMapper _mapper;

    public EmpresaController(IEmpresaRepository empresaRepository, IMapper mapper)
    {
        _empresaRepository = empresaRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Busca todas as empresas
    /// </summary>
    /// <returns>Todas as empresas</returns>
    /// <response code="200">Em caso de sucesso</response>
    /// <response code="500">Em caso de erro</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Empresa>>> GetAll()
    {
        try
        {
            var empresas = await _empresaRepository.GetAllAsync();
            return Ok(empresas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.", error = ex.Message });
        }
    }

    /// <summary>
    /// Busca empresa por id
    /// </summary>
    /// <param name="id">Id da empresa</param>
    /// <returns>Empresa</returns>
    /// <response code="200">Em caso de sucesso</response>
    /// <response code="404">Em caso de não achar empresa</response>
    /// <response code="500">Em caso de erro</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Empresa>> GetById(int id)
    {
        try
        {
            var empresa = await _empresaRepository.GetByIdAsync(id);
            if (empresa is null) 
                return NotFound(new { message = "Empresa não encontrada" });

            return Ok(empresa);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.", error = ex.Message });
        }
    }

    /// <summary>
    /// Adiciona empresa
    /// </summary>
    /// <param name="empresaDto">Empresa</param>
    /// <returns>Empresa</returns>
    /// <response code="201">Em caso de sucesso</response>
    /// <response code="400">Em caso de dados inválidos</response>
    /// <response code="409">Em caso de conflito (nome já existe)</response>
    /// <response code="500">Em caso de erro</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] EmpresaDTO empresaDto)
    {
        try
        {
            // Validação do modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            // Verificar se o nome já existe
            var nomeExiste = await _empresaRepository.NomeExisteAsync(empresaDto.Nome);
            if (nomeExiste)
            {
                return Conflict(new { message = "Nome da empresa já está em uso" });
            }

            var empresa = _mapper.Map<Empresa>(empresaDto);
            var empresaCriada = await _empresaRepository.AddAsync(empresa);
            
            return CreatedAtAction(nameof(GetById), new { id = empresaCriada.Id }, empresaCriada);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new { message = "Erro ao salvar no banco de dados", error = ex.InnerException?.Message ?? ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.", error = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza empresa
    /// </summary>
    /// <param name="id">Id da empresa</param>
    /// <param name="empresaDto">Empresa</param>
    /// <returns></returns>
    /// <response code="204">Em caso de sucesso</response>
    /// <response code="400">Em caso de dados inválidos</response>
    /// <response code="404">Em caso de não achar empresa</response>
    /// <response code="409">Em caso de conflito (nome já existe)</response>
    /// <response code="500">Em caso de erro</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put(int id, [FromBody] EmpresaDTO empresaDto)
    {
        try
        {
            // Validação do modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var empresa = await _empresaRepository.GetByIdAsync(id);
            if (empresa is null) 
                return NotFound(new { message = "Empresa não encontrada" });

            // Verificar se o nome já existe (excluindo a empresa atual)
            var nomeExiste = await _empresaRepository.NomeExisteAsync(empresaDto.Nome, id);
            if (nomeExiste)
            {
                return Conflict(new { message = "Nome da empresa já está em uso" });
            }

            // Atualizar apenas os campos necessários
            empresa.Nome = empresaDto.Nome;

            await _empresaRepository.UpdateAsync(empresa);

            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new { message = "Erro ao atualizar no banco de dados", error = ex.InnerException?.Message ?? ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.", error = ex.Message });
        }
    }

    /// <summary>
    /// Deleta empresa
    /// </summary>
    /// <param name="id">Id da empresa</param>
    /// <returns></returns>
    /// <response code="204">Em caso de sucesso</response>
    /// <response code="404">Em caso de não achar empresa</response>
    /// <response code="500">Em caso de erro</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var empresa = await _empresaRepository.GetByIdAsync(id);
            if (empresa is null) 
                return NotFound(new { message = "Empresa não encontrada" });

            await _empresaRepository.DeleteAsync(empresa);

            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new { message = "Erro ao deletar no banco de dados", error = ex.InnerException?.Message ?? ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.", error = ex.Message });
        }
    }

    /// <summary>
    /// Desativa empresa
    /// </summary>
    /// <param name="id">Id da empresa</param>
    /// <returns></returns>
    /// <response code="204">Em caso de sucesso</response>
    /// <response code="404">Em caso de não achar empresa</response>
    /// <response code="500">Em caso de erro</response>
    [HttpDelete("desativar/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Desativar(int id)
    {
        try
        {
            var empresa = await _empresaRepository.GetByIdAsync(id);
            if (empresa is null) 
                return NotFound(new { message = "Empresa não encontrada" });

            await _empresaRepository.DesativarAsync(empresa);

            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new { message = "Erro ao desativar no banco de dados", error = ex.InnerException?.Message ?? ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.", error = ex.Message });
        }
    }
}
