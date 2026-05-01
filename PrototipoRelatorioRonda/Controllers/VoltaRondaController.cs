using Microsoft.AspNetCore.Mvc;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;
using PrototipoRelatorioRonda.Services.Interface;

namespace PrototipoRelatorioRonda.Controllers;

[Route("[controller]")]
[ApiController]
public class VoltaRondaController : ControllerBase
{
    private readonly IVoltaRondaService _voltaRondaService;

    public VoltaRondaController(IVoltaRondaService voltaRondaService)
    {
        _voltaRondaService = voltaRondaService;
    }

    /// <summary>
    /// Busca todas as voltas de ronda
    /// </summary>
    /// <returns>Todas as voltas de ronda</returns>
    /// <response code="200">Em caso de sucesso</response>
    /// <response code="500">Em caso de erro</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<VoltaRonda>>> GetAll()
    {
        var voltas = await _voltaRondaService.GetAllWithRelatorioAsync();
        return Ok(voltas);
    }

    /// <summary>
    /// Busca volta de ronda por id
    /// </summary>
    /// <param name="id">Id da volta</param>
    /// <returns>Volta de ronda</returns>
    /// <response code="200">Em caso de sucesso</response>
    /// <response code="404">Em caso de não achar volta</response>
    /// <response code="500">Em caso de erro</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VoltaRonda>> GetById(int id)
    {
        var volta = await _voltaRondaService.GetByIdWithRelatorioAsync(id);
        if (volta is null)
            return NotFound(new { message = "Volta de ronda não encontrada" });

        return Ok(volta);
    }

    /// <summary>
    /// Busca voltas por relatório de ronda
    /// </summary>
    /// <param name="relatorioRondaId">Id do relatório de ronda</param>
    /// <returns>Voltas do relatório</returns>
    /// <response code="200">Em caso de sucesso</response>
    /// <response code="500">Em caso de erro</response>
    [HttpGet("relatorio/{relatorioRondaId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<VoltaRonda>>> GetByRelatorioRonda(int relatorioRondaId)
    {
        var voltas = await _voltaRondaService.GetByRelatorioRondaAsync(relatorioRondaId);
        return Ok(voltas);
    }

    /// <summary>
    /// Adiciona volta de ronda
    /// </summary>
    /// <param name="voltaDto">Volta de ronda</param>
    /// <returns>Volta de ronda</returns>
    /// <response code="201">Em caso de sucesso</response>
    /// <response code="400">Em caso de dados inválidos</response>
    /// <response code="404">Em caso de relatório não encontrado</response>
    /// <response code="409">Em caso de número de volta já existir</response>
    /// <response code="500">Em caso de erro</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] VoltaRondaDTO voltaDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Dados inválidos", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        var voltaCriada = await _voltaRondaService.CreateAsync(voltaDto);
        return CreatedAtAction(nameof(GetById), new { id = voltaCriada.Id }, voltaCriada);
    }

    /// <summary>
    /// Atualiza volta de ronda
    /// </summary>
    /// <param name="id">Id da volta</param>
    /// <param name="voltaDto">Volta de ronda</param>
    /// <returns></returns>
    /// <response code="204">Em caso de sucesso</response>
    /// <response code="400">Em caso de dados inválidos</response>
    /// <response code="404">Em caso de não achar volta</response>
    /// <response code="409">Em caso de número de volta já existir</response>
    /// <response code="500">Em caso de erro</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put(int id, [FromBody] VoltaRondaDTO voltaDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Dados inválidos", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        await _voltaRondaService.UpdateAsync(id, voltaDto);
        return NoContent();
    }

    /// <summary>
    /// Deleta volta de ronda
    /// </summary>
    /// <param name="id">Id da volta</param>
    /// <returns></returns>
    /// <response code="204">Em caso de sucesso</response>
    /// <response code="404">Em caso de não achar volta</response>
    /// <response code="500">Em caso de erro</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        await _voltaRondaService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Desativa volta de ronda
    /// </summary>
    /// <param name="id">Id da volta</param>
    /// <returns></returns>
    /// <response code="204">Em caso de sucesso</response>
    /// <response code="404">Em caso de não achar volta</response>
    /// <response code="500">Em caso de erro</response>
    [HttpDelete("desativar/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Desativar(int id)
    {
        await _voltaRondaService.DesativarAsync(id);
        return NoContent();
    }
}
