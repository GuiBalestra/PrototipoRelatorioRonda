using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrototipoRelatorioRonda.Domain.Entities;
using PrototipoRelatorioRonda.Application.DTOs;
using PrototipoRelatorioRonda.Application.Interfaces;

namespace PrototipoRelatorioRonda.API.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class RelatorioRondaController : ControllerBase
{
    private readonly IRelatorioRondaService _relatorioRondaService;

    public RelatorioRondaController(IRelatorioRondaService relatorioRondaService)
    {
        _relatorioRondaService = relatorioRondaService;
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
        var relatorios = await _relatorioRondaService.GetAllWithDetailsAsync();
        return Ok(relatorios);
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
        var relatorio = await _relatorioRondaService.GetByIdWithDetailsAsync(id);
        if (relatorio is null)
            return NotFound(new { message = "Relatório de ronda não encontrado" });

        return Ok(relatorio);
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
        var relatorios = await _relatorioRondaService.GetByEmpresaAsync(empresaId);
        return Ok(relatorios);
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
        var relatorios = await _relatorioRondaService.GetByVigilanteAsync(vigilanteId);
        return Ok(relatorios);
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
        var relatorios = await _relatorioRondaService.GetByDataAsync(data);
        return Ok(relatorios);
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
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Dados inválidos", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        var relatorioCriado = await _relatorioRondaService.CreateAsync(relatorioDto);
        return CreatedAtAction(nameof(GetById), new { id = relatorioCriado.Id }, relatorioCriado);
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
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Dados inválidos", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        await _relatorioRondaService.UpdateAsync(id, relatorioDto);
        return NoContent();
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
        await _relatorioRondaService.DeleteAsync(id);
        return NoContent();
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
        await _relatorioRondaService.DesativarAsync(id);
        return NoContent();
    }
}
