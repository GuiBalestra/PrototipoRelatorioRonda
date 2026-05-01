using Moq;
using Xunit;
using FluentAssertions;
using PrototipoRelatorioRonda.Services;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;
using PrototipoRelatorioRonda.Models.Enums;
using AutoMapper;

namespace PrototipoRelatorioRonda.Tests.Services;

public class UsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UsuarioService _service;

    public UsuarioServiceTests()
    {
        _repoMock = new Mock<IUsuarioRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new UsuarioService(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllWithEmpresaAsync_ReturnsAll()
    {
        var list = new List<Usuario> { new Usuario { Id = 1, Nome = "João" }, new Usuario { Id = 2, Nome = "Maria" } };
        _repoMock.Setup(r => r.GetAllWithEmpresaAsync()).ReturnsAsync(list);

        var result = await _service.GetAllWithEmpresaAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdWithEmpresaAsync_WhenExists_ReturnsUsuario()
    {
        var usuario = new Usuario { Id = 1, Nome = "João" };
        _repoMock.Setup(r => r.GetByIdWithEmpresaAsync(1)).ReturnsAsync(usuario);

        var result = await _service.GetByIdWithEmpresaAsync(1);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAsync_WhenEmailAndNomeUnique_Creates()
    {
        var dto = new UsuarioDTO { Nome = "Novo", Email = "novo@email.com", EmpresaId = 1, Funcao = Funcao.Vigilante, Senha = "123456" };
        var usuario = new Usuario { Id = 1, Nome = "Novo", Email = "novo@email.com" };

        _repoMock.Setup(r => r.EmailExisteAsync(dto.Email, null)).ReturnsAsync(false);
        _repoMock.Setup(r => r.NomeExisteAsync(dto.Nome, null)).ReturnsAsync(false);
        _repoMock.Setup(r => r.EmpresaExisteAsync(dto.EmpresaId)).ReturnsAsync(true);
        _mapperMock.Setup(m => m.Map<Usuario>(dto)).Returns(usuario);
        _repoMock.Setup(r => r.AddAsync(usuario)).ReturnsAsync(usuario);

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Nome.Should().Be("Novo");
    }

    [Fact]
    public async Task CreateAsync_WhenEmailExists_Throws()
    {
        var dto = new UsuarioDTO { Nome = "A", Email = "existe@email.com", EmpresaId = 1 };
        _repoMock.Setup(r => r.EmailExisteAsync(dto.Email, null)).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_WhenEmpresaNotExists_Throws()
    {
        var dto = new UsuarioDTO { Nome = "A", Email = "a@email.com", EmpresaId = 999 };
        _repoMock.Setup(r => r.EmailExisteAsync(It.IsAny<string>(), It.IsAny<int?>())).ReturnsAsync(false);
        _repoMock.Setup(r => r.NomeExisteAsync(It.IsAny<string>(), It.IsAny<int?>())).ReturnsAsync(false);
        _repoMock.Setup(r => r.EmpresaExisteAsync(It.IsAny<int>())).ReturnsAsync(false);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task DeleteAsync_WhenNotExists_Throws()
    {
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Usuario)null!);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(999));
    }
}
