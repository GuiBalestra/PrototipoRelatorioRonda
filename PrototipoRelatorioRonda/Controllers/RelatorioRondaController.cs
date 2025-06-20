using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;

namespace PrototipoRelatorioRonda.Controllers;

[Route("[controller]")]
[ApiController]
public class RelatorioRondaController : ControllerBase
{
    private readonly IRelatorioRondaRepository _relatorioRondaRepository;
    private readonly IMapper _mapper;

    public RelatorioRondaController(IRelatorioRondaRepository relatorioRondaRepository, IMapper mapper)
    {
        _relatorioRondaRepository = relatorioRondaRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Busca todos os relatórios de ronda
    /// </summary>
    /// <returns>Todos os relatórios de ronda</returns>
    /// <response code="200">Em caso de sucesso</response>
    /// <response code="500">Em caso de erro</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<RelatorioRonda>>> GetAll()
    {
        try
        {
            var relatorios = await _relatorioRondaRepository.GetAllWithDetailsAsync();
            return Ok(relatorios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.", error = ex.Message });
        }
    }

    /// <summary>
    /// Busca relatório de ronda por id
    /// </summary>
    /// <param name="id">Id do relatório</param>
    /// <returns>Relatório de ronda</returns>
    /// <response code="200">Em caso de sucesso</response>
    /// <response code="404">Em caso de não achar relatório</response>
    /// <response code="500">Em caso de erro</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RelatorioRonda>> GetById(int id)
    {
        try
        {
            var relatorio = await _relatorioRondaRepository.GetByIdWithDetailsAsync(id);
            if (relatorio is null) 
                return NotFound(new { message = "Relatório de ronda não encontrado" });

            return Ok(relatorio);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.", error = ex.Message });
        }
    }

    /// <summary>
    /// Busca relatórios por empresa
    /// </summary>
    /// <param name="empresaId">Id da empresa</param>
    /// <returns>Relatórios da empresa</returns>
    /// <response code="200">Em caso de sucesso</response>
    /// <response code="500">Em caso de erro</response>
    [HttpGet("empresa/{empresaId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<RelatorioRonda>>> GetByEmpresa(int empresaId)
    {
        try
        {
            var relatorios = await _relatorioRondaRepository.GetByEmpresaAsync(empresaId);
            return Ok(relatorios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.", error = ex.Message });
        }
    }

    /// <summary>
    /// Busca relatórios por vigilante
    /// </summary>
    /// <param name="vigilanteId">Id do vigilante</param>
    /// <returns>Relatórios do vigilante</returns>
    /// <response code="200">Em caso de sucesso</response>
    /// <response code="500">Em caso de erro</response>
    [HttpGet("vigilante/{vigilanteId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<RelatorioRonda>>> GetByVigilante(int vigilanteId)
    {
        try
        {
            var relatorios = await _relatorioRondaRepository.GetByVigilanteAsync(vigilanteId);
            return Ok(relatorios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.", error = ex.Message });
        }
    }

    /// <summary>
    /// Busca relatórios por data
    /// </summary>
    /// <param name="data">Data dos relatórios</param>
    /// <returns>Relatórios da data</returns>
    /// <response code="200">Em caso de sucesso</response>
    /// <response code="500">Em caso de erro</response>
    [HttpGet("data/{data:datetime}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<RelatorioRonda>>> GetByData(DateTime data)
    {
        try
        {
            var relatorios = await _relatorioRondaRepository.GetByDataAsync(data);
            return Ok(relatorios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.", error = ex.Message });
        }
    }

    /// <summary>
    /// Adiciona relatório de ronda
    /// </summary>
    /// <param name="relatorioDto">Relatório de ronda</param>
    /// <returns>Relatório de ronda</returns>
    /// <response code="201">Em caso de sucesso</response>
    /// <response code="400">Em caso de dados inválidos</response>
    /// <response code="404">Em caso de empresa ou vigilante não encontrado</response>
    /// <response code="409">Em caso de relatório já existir para a data</response>
    /// <response code="500">Em caso de erro</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] RelatorioRondaDTO relatorioDto)
    {
        try
        {
            // Validação do modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            // Verificar se a empresa existe
            var empresaExiste = await _relatorioRondaRepository.EmpresaExisteAsync(relatorioDto.EmpresaId);
            if (!empresaExiste)
            {
                return NotFound(new { message = "Empresa não encontrada" });
            }

            // Verificar se o vigilante existe
            var vigilanteExiste = await _relatorioRondaRepository.VigilanteExisteAsync(relatorioDto.VigilanteId);
            if (!vigilanteExiste)
            {
                return NotFound(new { message = "Vigilante não encontrado" });
            }

            // Verificar se já existe relatório para a mesma data, empresa e vigilante
            var relatorioExiste = await _relatorioRondaRepository.RelatorioExisteParaDataAsync(
                relatorioDto.EmpresaId, relatorioDto.VigilanteId, relatorioDto.Data);
            if (relatorioExiste)
            {
                return Conflict(new { message = "Já existe um relatório para esta data, empresa e vigilante" });
            }

            var relatorio = _mapper.Map<RelatorioRonda>(relatorioDto);
            var relatorioCriado = await _relatorioRondaRepository.AddAsync(relatorio);
            
            // Buscar o relatório criado com detalhes para retornar
            var relatorioCompleto = await _relatorioRondaRepository.GetByIdWithDetailsAsync(relatorioCriado.Id);
            
            return CreatedAtAction(nameof(GetById), new { id = relatorioCriado.Id }, relatorioCompleto);
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
    /// Atualiza relatório de ronda
    /// </summary>
    /// <param name="id">Id do relatório</param>
    /// <param name="relatorioDto">Relatório de ronda</param>
    /// <returns></returns>
    /// <response code="204">Em caso de sucesso</response>
    /// <response code="400">Em caso de dados inválidos</response>
    /// <response code="404">Em caso de não achar relatório</response>
    /// <response code="409">Em caso de relatório já existir para a data</response>
    /// <response code="500">Em caso de erro</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put(int id, [FromBody] RelatorioRondaDTO relatorioDto)
    {
        try
        {
            // Validação do modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var relatorio = await _relatorioRondaRepository.GetByIdWithDetailsAsync(id);
            if (relatorio is null) 
                return NotFound(new { message = "Relatório de ronda não encontrado" });

            // Verificar se a empresa existe
            var empresaExiste = await _relatorioRondaRepository.EmpresaExisteAsync(relatorioDto.EmpresaId);
            if (!empresaExiste)
            {
                return NotFound(new { message = "Empresa não encontrada" });
            }

            // Verificar se o vigilante existe
            var vigilanteExiste = await _relatorioRondaRepository.VigilanteExisteAsync(relatorioDto.VigilanteId);
            if (!vigilanteExiste)
            {
                return NotFound(new { message = "Vigilante não encontrado" });
            }

            // Verificar se já existe outro relatório para a mesma data, empresa e vigilante (excluindo o atual)
            var relatorioExiste = await _relatorioRondaRepository.GetByAsync(r => 
                r.EmpresaId == relatorioDto.EmpresaId && 
                r.VigilanteId == relatorioDto.VigilanteId && 
                r.Data.Date == relatorioDto.Data.Date && 
                r.Id != id && 
                r.Ativo);
            
            if (relatorioExiste != null)
            {
                return Conflict(new { message = "Já existe outro relatório para esta data, empresa e vigilante" });
            }

            // Atualizar apenas os campos necessários
            relatorio.EmpresaId = relatorioDto.EmpresaId;
            relatorio.VigilanteId = relatorioDto.VigilanteId;
            relatorio.Data = relatorioDto.Data;
            relatorio.KmSaida = relatorioDto.KmSaida;
            relatorio.KmChegada = relatorioDto.KmChegada;
            relatorio.TestemunhaSaida = relatorioDto.TestemunhaSaida;
            relatorio.TestemunhaChegada = relatorioDto.TestemunhaChegada;

            await _relatorioRondaRepository.UpdateAsync(relatorio);

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
    /// Deleta relatório de ronda
    /// </summary>
    /// <param name="id">Id do relatório</param>
    /// <returns></returns>
    /// <response code="204">Em caso de sucesso</response>
    /// <response code="404">Em caso de não achar relatório</response>
    /// <response code="500">Em caso de erro</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var relatorio = await _relatorioRondaRepository.GetByIdWithDetailsAsync(id);
            if (relatorio is null) 
                return NotFound(new { message = "Relatório de ronda não encontrado" });

            await _relatorioRondaRepository.DeleteAsync(relatorio);

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
    /// Desativa relatório de ronda
    /// </summary>
    /// <param name="id">Id do relatório</param>
    /// <returns></returns>
    /// <response code="204">Em caso de sucesso</response>
    /// <response code="404">Em caso de não achar relatório</response>
    /// <response code="500">Em caso de erro</response>
    [HttpDelete("desativar/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Desativar(int id)
    {
        try
        {
            var relatorio = await _relatorioRondaRepository.GetByIdWithDetailsAsync(id);
            if (relatorio is null) 
                return NotFound(new { message = "Relatório de ronda não encontrado" });

            await _relatorioRondaRepository.DesativarAsync(relatorio);

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
