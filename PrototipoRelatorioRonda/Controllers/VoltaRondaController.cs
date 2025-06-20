using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;

namespace PrototipoRelatorioRonda.Controllers;

[Route("[controller]")]
[ApiController]
public class VoltaRondaController : ControllerBase
{
    private readonly IVoltaRondaRepository _voltaRondaRepository;
    private readonly IMapper _mapper;

    public VoltaRondaController(IVoltaRondaRepository voltaRondaRepository, IMapper mapper)
    {
        _voltaRondaRepository = voltaRondaRepository;
        _mapper = mapper;
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
        try
        {
            var voltas = await _voltaRondaRepository.GetAllWithRelatorioAsync();
            return Ok(voltas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.", error = ex.Message });
        }
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
        try
        {
            var volta = await _voltaRondaRepository.GetByIdWithRelatorioAsync(id);
            if (volta is null) 
                return NotFound(new { message = "Volta de ronda não encontrada" });

            return Ok(volta);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.", error = ex.Message });
        }
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
        try
        {
            var voltas = await _voltaRondaRepository.GetByRelatorioRondaAsync(relatorioRondaId);
            return Ok(voltas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.", error = ex.Message });
        }
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
        try
        {
            // Validação do modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            // Verificar se o relatório de ronda existe
            var relatorioExiste = await _voltaRondaRepository.RelatorioRondaExisteAsync(voltaDto.RelatorioRondaId);
            if (!relatorioExiste)
            {
                return NotFound(new { message = "Relatório de ronda não encontrado" });
            }

            // Verificar se o número da volta já existe para este relatório
            var numeroVoltaExiste = await _voltaRondaRepository.NumeroVoltaExisteAsync(voltaDto.RelatorioRondaId, voltaDto.NumeroVolta);
            if (numeroVoltaExiste)
            {
                return Conflict(new { message = "Número da volta já existe para este relatório" });
            }

            var volta = _mapper.Map<VoltaRonda>(voltaDto);
            var voltaCriada = await _voltaRondaRepository.AddAsync(volta);
            
            // Buscar a volta criada com o relatório para retornar
            var voltaCompleta = await _voltaRondaRepository.GetByIdWithRelatorioAsync(voltaCriada.Id);
            
            return CreatedAtAction(nameof(GetById), new { id = voltaCriada.Id }, voltaCompleta);
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
        try
        {
            // Validação do modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var volta = await _voltaRondaRepository.GetByIdWithRelatorioAsync(id);
            if (volta is null) 
                return NotFound(new { message = "Volta de ronda não encontrada" });

            // Verificar se o relatório de ronda existe
            var relatorioExiste = await _voltaRondaRepository.RelatorioRondaExisteAsync(voltaDto.RelatorioRondaId);
            if (!relatorioExiste)
            {
                return NotFound(new { message = "Relatório de ronda não encontrado" });
            }

            // Verificar se o número da volta já existe para este relatório (excluindo a volta atual)
            var numeroVoltaExiste = await _voltaRondaRepository.NumeroVoltaExisteAsync(voltaDto.RelatorioRondaId, voltaDto.NumeroVolta, id);
            if (numeroVoltaExiste)
            {
                return Conflict(new { message = "Número da volta já existe para este relatório" });
            }

            // Atualizar apenas os campos necessários
            volta.RelatorioRondaId = voltaDto.RelatorioRondaId;
            volta.NumeroVolta = voltaDto.NumeroVolta;
            volta.HoraSaida = voltaDto.HoraSaida;
            volta.HoraChegada = voltaDto.HoraChegada;
            volta.HoraDescanso = voltaDto.HoraDescanso;
            volta.Observacoes = voltaDto.Observacoes;

            await _voltaRondaRepository.UpdateAsync(volta);

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
        try
        {
            var volta = await _voltaRondaRepository.GetByIdWithRelatorioAsync(id);
            if (volta is null) 
                return NotFound(new { message = "Volta de ronda não encontrada" });

            await _voltaRondaRepository.DeleteAsync(volta);

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
        try
        {
            var volta = await _voltaRondaRepository.GetByIdWithRelatorioAsync(id);
            if (volta is null) 
                return NotFound(new { message = "Volta de ronda não encontrada" });

            await _voltaRondaRepository.DesativarAsync(volta);

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
