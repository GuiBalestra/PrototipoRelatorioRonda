using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;

namespace PrototipoRelatorioRonda.Controllers;

[Route("[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMapper _mapper;

    public UsuarioController(IUsuarioRepository usuarioRepository, IMapper mapper)
    {
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Busca todos os usuários
    /// </summary>
    /// <returns>Todos os usuários</returns>
    /// <response code="200">Em caso de sucesso</response>
    /// <response code="500">Em caso de erro</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<Usuario> GetAll()
    {
        try
        {
            return Ok(_usuarioRepository.GetAll());
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro interno. Por favor, tente novamente mais tarde.");
        }
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
    public ActionResult<Usuario> GetById(int id)
    {
        try
        {
            var usuario = _usuarioRepository.GetById(id);
            if (usuario is null) return NotFound();

            return Ok(usuario);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro interno. Por favor, tente novamente mais tarde.");
        }
    }

    /// <summary>
    /// Adiciona usuário
    /// </summary>
    /// <param name="usuarioDto">Usuário</param>
    /// <returns>Usuário</returns>
    /// <response code="204">Em caso de sucesso</response>
    /// <response code="500">Em caso de erro</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Post([FromBody] UsuarioDTO usuarioDto)
    {
        try
        {
            var usuario = _mapper.Map<Usuario>(usuarioDto);
            _usuarioRepository.Add(usuario);
            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro interno. Por favor, tente novamente mais tarde.");
        }
    }

    /// <summary>
    /// Atualiza usuário
    /// </summary>
    /// <param name="id">Id do usuário</param>
    /// <param name="usuarioDto">Usuário</param>
    /// <returns></returns>
    /// <response code="204">Em caso de sucesso</response>
    /// <response code="404">Em caso de não achar usuário</response>
    /// <response code="500">Em caso de erro</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Put(int id, [FromBody] UsuarioDTO usuarioDto)
    {
        try
        {
            var usuario = _usuarioRepository.GetById(id);
            if (usuario is null) return NotFound();

            _usuarioRepository.Update(_mapper.Map(usuarioDto, usuario));

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Ocorreu um erro interno. Por favor, tente novamente mais tarde.");
        }
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
    public IActionResult Delete(int id)
    {
        try
        {
            var usuario = _usuarioRepository.GetById(id);
            if (usuario is null) return NotFound();

            _usuarioRepository.Delete(usuario);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro interno. Por favor, tente novamente mais tarde.");
        }
        
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
    public IActionResult Desativar(int id)
    {
        try
        {
            var usuario = _usuarioRepository.GetById(id);
            if (usuario is null) return NotFound();

            _usuarioRepository.Desativar(usuario);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro interno. Por favor, tente novamente mais tarde.");
        }

    }
}
