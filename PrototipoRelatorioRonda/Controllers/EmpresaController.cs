using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    public ActionResult<IEnumerable<Empresa>> GetAll()
    {
        try
        {
            return Ok(_empresaRepository.GetAll());
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro interno. Por favor, tente novamente mais tarde.");
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
    public ActionResult<Empresa> GetById(int id)
    {
        try
        {
            var empresa = _empresaRepository.GetById(id);
            if (empresa is null) return NotFound();

            return Ok(empresa);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro interno. Por favor, tente novamente mais tarde.");
        }
    }

    /// <summary>
    /// Adiciona empresa
    /// </summary>
    /// <param name="empresaDto">Empresa</param>
    /// <returns>Empresa</returns>
    /// <response code="204">Em caso de sucesso</response>
    /// <response code="500">Em caso de erro</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Post([FromBody] EmpresaDTO empresaDto)
    {
        try
        {
            var empresa = _empresaRepository.Add(_mapper.Map<Empresa>(empresaDto));
            return CreatedAtAction(nameof(GetById), new { id = empresa.Id }, empresa);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro interno. Por favor, tente novamente mais tarde.");
        }
    }

    /// <summary>
    /// Atualiza empresa
    /// </summary>
    /// <param name="id">Id da empresa</param>
    /// <param name="empresaDto">Empresa</param>
    /// <returns></returns>
    /// <response code="204">Em caso de sucesso</response>
    /// <response code="404">Em caso de não achar empresa</response>
    /// <response code="500">Em caso de erro</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Put(int id, [FromBody] EmpresaDTO empresaDto)
    {
        try
        {
            var empresa = _empresaRepository.GetById(id);
            if (empresa is null) return NotFound();

            _empresaRepository.Update(_mapper.Map(empresaDto, empresa));

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro interno. Por favor, tente novamente mais tarde.");
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
    public IActionResult Delete(int id)
    {
        try
        {
            var empresa = _empresaRepository.GetById(id);
            if (empresa is null) return NotFound();

            _empresaRepository.Delete(empresa);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro interno. Por favor, tente novamente mais tarde.");
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
    public IActionResult Desativar(int id)
    {
        try
        {
            var empresa = _empresaRepository.GetById(id);
            if (empresa is null) return NotFound();

            _empresaRepository.Desativar(empresa);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro interno. Por favor, tente novamente mais tarde.");
        }
    }
}
