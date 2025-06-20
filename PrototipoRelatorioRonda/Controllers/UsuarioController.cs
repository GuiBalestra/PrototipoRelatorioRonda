using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
    {
        try
        {
            var usuarios = await _usuarioRepository.GetAllWithEmpresaAsync();
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.", error = ex.Message });
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Usuario>> GetById(int id)
    {
        try
        {
            var usuario = await _usuarioRepository.GetByIdWithEmpresaAsync(id);
            if (usuario is null) 
                return NotFound(new { message = "Usuário não encontrado" });

            return Ok(usuario);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.", error = ex.Message });
        }
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
        try
        {
            // Validação do modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            // Verificar se a empresa existe
            var empresaExiste = await _usuarioRepository.EmpresaExisteAsync(usuarioDto.EmpresaId);
            if (!empresaExiste)
            {
                return NotFound(new { message = "Empresa não encontrada" });
            }

            // Verificar se o email já existe
            var emailExiste = await _usuarioRepository.EmailExisteAsync(usuarioDto.Email);
            if (emailExiste)
            {
                return Conflict(new { message = "Email já está em uso" });
            }

            // Verificar se o nome já existe
            var nomeExiste = await _usuarioRepository.NomeExisteAsync(usuarioDto.Nome);
            if (nomeExiste)
            {
                return Conflict(new { message = "Nome já está em uso" });
            }

            var usuario = _mapper.Map<Usuario>(usuarioDto);
            var usuarioCriado = await _usuarioRepository.AddAsync(usuario);
            
            // Buscar o usuário criado com a empresa para retornar
            var usuarioCompleto = await _usuarioRepository.GetByIdWithEmpresaAsync(usuarioCriado.Id);
            
            return CreatedAtAction(nameof(GetById), new { id = usuarioCriado.Id }, usuarioCompleto);
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
        try
        {
            // Validação do modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var usuario = await _usuarioRepository.GetByIdWithEmpresaAsync(id);
            if (usuario is null) 
                return NotFound(new { message = "Usuário não encontrado" });

            // Verificar se a empresa existe
            var empresaExiste = await _usuarioRepository.EmpresaExisteAsync(usuarioDto.EmpresaId);
            if (!empresaExiste)
            {
                return NotFound(new { message = "Empresa não encontrada" });
            }

            // Verificar se o email já existe (excluindo o usuário atual)
            var emailExiste = await _usuarioRepository.EmailExisteAsync(usuarioDto.Email, id);
            if (emailExiste)
            {
                return Conflict(new { message = "Email já está em uso" });
            }

            // Verificar se o nome já existe (excluindo o usuário atual)
            var nomeExiste = await _usuarioRepository.NomeExisteAsync(usuarioDto.Nome, id);
            if (nomeExiste)
            {
                return Conflict(new { message = "Nome já está em uso" });
            }

            // Atualizar apenas os campos necessários
            usuario.Nome = usuarioDto.Nome;
            usuario.Email = usuarioDto.Email;
            usuario.EmpresaId = usuarioDto.EmpresaId;
            usuario.Funcao = usuarioDto.Funcao;

            // Se uma nova senha foi fornecida, atualizar o hash
            if (!string.IsNullOrEmpty(usuarioDto.Senha))
            {
                using var sha256 = System.Security.Cryptography.SHA256.Create();
                var bytes = System.Text.Encoding.UTF8.GetBytes(usuarioDto.Senha);
                var hash = sha256.ComputeHash(bytes);
                usuario.HashSenha = Convert.ToBase64String(hash);
            }

            await _usuarioRepository.UpdateAsync(usuario);

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
        try
        {
            var usuario = await _usuarioRepository.GetByIdWithEmpresaAsync(id);
            if (usuario is null) 
                return NotFound(new { message = "Usuário não encontrado" });

            await _usuarioRepository.DeleteAsync(usuario);

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
        try
        {
            var usuario = await _usuarioRepository.GetByIdWithEmpresaAsync(id);
            if (usuario is null) 
                return NotFound(new { message = "Usuário não encontrado" });

            await _usuarioRepository.DesativarAsync(usuario);

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
