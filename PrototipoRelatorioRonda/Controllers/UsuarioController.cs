using Microsoft.AspNetCore.Mvc;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;
using PrototipoRelatorioRonda.Services.Interface;

namespace PrototipoRelatorioRonda.Controllers;

[Route("[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Busca todos os usuários
    /// </summary>
    /// <returns>Todos os usuários</returns>
    /// <response code="200">Em caso de sucesso</response>
    /// <response code="500">Em caso de erro</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
    {
        var usuarios = await _usuarioService.GetAllWithEmpresaAsync();
        return Ok(usuarios);
    }

    /// <summary>
    /// Busca usuário por id
    /// </summary>
    /// <param name="id">Id do usuário</param>
    /// <returns>Usuário</returns>
    /// <response code="200">Em caso de sucesso</response>
    /// <response code="404">Em caso de não achar usuário</response>
    /// <response code="500">Em caso de erro</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Usuario>> GetById(int id)
    {
        var usuario = await _usuarioService.GetByIdWithEmpresaAsync(id);
        if (usuario is null)
            return NotFound(new { message = "Usuário não encontrado" });

        return Ok(usuario);
    }

    /// <summary>
    /// Adiciona usuário
    /// </summary>
    /// <param name="usuarioDto">Usuário</param>
    /// <returns>Usuário</returns>
    /// <response code="201">Em caso de sucesso</response>
    /// <response code="400">Em caso de dados inválidos</response>
    /// <response code="409">Em caso de conflito (email/nome já existe)</response>
    /// <response code="404">Em caso de empresa não encontrada</response>
    /// <response code="500">Em caso de erro</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] UsuarioDTO usuarioDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Dados inválidos", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        var usuarioCriado = await _usuarioService.CreateAsync(usuarioDto);
        return CreatedAtAction(nameof(GetById), new { id = usuarioCriado.Id }, usuarioCriado);
    }

    /// <summary>
    /// Atualiza usuário
    /// </summary>
    /// <param name="id">Id do usuário</param>
    /// <param name="usuarioDto">Usuário</param>
    /// <returns></returns>
    /// <response code="204">Em caso de sucesso</response>
    /// <response code="400">Em caso de dados inválidos</response>
    /// <response code="404">Em caso de não achar usuário</response>
    /// <response code="409">Em caso de conflito (email/nome já existe)</response>
    /// <response code="500">Em caso de erro</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put(int id, [FromBody] UsuarioDTO usuarioDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Dados inválidos", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        await _usuarioService.UpdateAsync(id, usuarioDto);
        return NoContent();
    }

    /// <summary>
    /// Deleta usuário
    /// </summary>
    /// <param name="id">Id do usuário</param>
    /// <returns></returns>
    /// <response code="204">Em caso de sucesso</response>
    /// <response code="404">Em caso de não achar usuário</response>
    /// <response code="500">Em caso de erro</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        await _usuarioService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Desativa usuário
    /// </summary>
    /// <param name="id">Id do usuário</param>
    /// <returns></returns>
    /// <response code="204">Em caso de sucesso</response>
    /// <response code="404">Em caso de não achar usuário</response>
    /// <response code="500">Em caso de erro</response>
    [HttpDelete("desativar/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Desativar(int id)
    {
        await _usuarioService.DesativarAsync(id);
        return NoContent();
    }
}
