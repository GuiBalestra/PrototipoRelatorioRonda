using AutoMapper;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;
using PrototipoRelatorioRonda.Services.Interface;
using System.Security.Cryptography;
using System.Text;

namespace PrototipoRelatorioRonda.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMapper _mapper;

    public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper)
    {
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _usuarioRepository.GetAllAsync();
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _usuarioRepository.GetByIdAsync(id);
    }

    public async Task<Usuario> CreateAsync(UsuarioDTO usuarioDto)
    {
        // Verificar se o email já existe
        var emailExiste = await _usuarioRepository.EmailExisteAsync(usuarioDto.Email);
        if (emailExiste)
        {
            throw new InvalidOperationException("Email já está em uso");
        }

        // Verificar se o nome já existe
        var nomeExiste = await _usuarioRepository.NomeExisteAsync(usuarioDto.Nome);
        if (nomeExiste)
        {
            throw new InvalidOperationException("Nome já está em uso");
        }

        // Verificar se a empresa existe
        var empresaExiste = await _usuarioRepository.EmpresaExisteAsync(usuarioDto.EmpresaId);
        if (!empresaExiste)
        {
            throw new KeyNotFoundException("Empresa não encontrada");
        }

        var usuario = _mapper.Map<Usuario>(usuarioDto);
        
        // Hash da senha
        usuario.HashSenha = HashPassword(usuarioDto.Senha);
        
        return await _usuarioRepository.AddAsync(usuario);
    }

    public async Task UpdateAsync(int id, UsuarioDTO usuarioDto)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario is null)
        {
            throw new KeyNotFoundException("Usuário não encontrado");
        }

        // Verificar se o email já existe (excluindo o usuário atual)
        var emailExiste = await _usuarioRepository.EmailExisteAsync(usuarioDto.Email, id);
        if (emailExiste)
        {
            throw new InvalidOperationException("Email já está em uso");
        }

        // Verificar se o nome já existe (excluindo o usuário atual)
        var nomeExiste = await _usuarioRepository.NomeExisteAsync(usuarioDto.Nome, id);
        if (nomeExiste)
        {
            throw new InvalidOperationException("Nome já está em uso");
        }

        // Verificar se a empresa existe
        var empresaExiste = await _usuarioRepository.EmpresaExisteAsync(usuarioDto.EmpresaId);
        if (!empresaExiste)
        {
            throw new KeyNotFoundException("Empresa não encontrada");
        }

        // Atualizar campos
        usuario.Nome = usuarioDto.Nome;
        usuario.Email = usuarioDto.Email;
        usuario.EmpresaId = usuarioDto.EmpresaId;
        usuario.Funcao = usuarioDto.Funcao;

        // Hash da senha apenas se foi fornecida
        if (!string.IsNullOrEmpty(usuarioDto.Senha))
        {
            usuario.HashSenha = HashPassword(usuarioDto.Senha);
        }

        await _usuarioRepository.UpdateAsync(usuario);
    }

    public async Task DeleteAsync(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario is null)
        {
            throw new KeyNotFoundException("Usuário não encontrado");
        }

        await _usuarioRepository.DeleteAsync(usuario);
    }

    public async Task DesativarAsync(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario is null)
        {
            throw new KeyNotFoundException("Usuário não encontrado");
        }

        await _usuarioRepository.DesativarAsync(usuario);
    }

    public async Task<IEnumerable<Usuario>> GetAllWithEmpresaAsync()
    {
        return await _usuarioRepository.GetAllWithEmpresaAsync();
    }

    public async Task<Usuario?> GetByIdWithEmpresaAsync(int id)
    {
        return await _usuarioRepository.GetByIdWithEmpresaAsync(id);
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
} 
