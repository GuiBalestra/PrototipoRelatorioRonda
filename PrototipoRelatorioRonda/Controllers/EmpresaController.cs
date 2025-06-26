using Microsoft.AspNetCore.Mvc;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;
using PrototipoRelatorioRonda.Services.Interface;

namespace PrototipoRelatorioRonda.Controllers;

[Route("[controller]")]
[ApiController]
public class EmpresaController : ControllerBase
{
    private readonly IEmpresaService _empresaService;

    public EmpresaController(IEmpresaService empresaService)
    {
        _empresaService = empresaService;
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
        var empresas = await _empresaService.GetAllAsync();
        return Ok(empresas);
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
        var empresa = await _empresaService.GetByIdAsync(id);
        if (empresa is null) 
            return NotFound(new { message = "Empresa não encontrada" });

        return Ok(empresa);
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
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Dados inválidos", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        var empresaCriada = await _empresaService.CreateAsync(empresaDto);
        return CreatedAtAction(nameof(GetById), new { id = empresaCriada.Id }, empresaCriada);
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
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Dados inválidos", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        await _empresaService.UpdateAsync(id, empresaDto);
        return NoContent();
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
        await _empresaService.DeleteAsync(id);
        return NoContent();
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
        await _empresaService.DesativarAsync(id);
        return NoContent();
    }
}
