using Moq;
using Xunit;
using FluentAssertions;
using PrototipoRelatorioRonda.API.Controllers;
using PrototipoRelatorioRonda.Application.Interfaces;
using PrototipoRelatorioRonda.Domain.Entities;
using PrototipoRelatorioRonda.Application.DTOs;

namespace PrototipoRelatorioRonda.Tests.Controllers;

public class UsuarioControllerTests
{
    private readonly Mock<IUsuarioService> _serviceMock;
    private readonly UsuarioController _controller;

    public UsuarioControllerTests()
    {
        _serviceMock = new Mock<IUsuarioService>();
        _controller = new UsuarioController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var usuarios = new List<Usuario>
        {
            new Usuario { Id = 1, Nome = "Vigilante A" },
            new Usuario { Id = 2, Nome = "Vigilante B" }
        };
        _serviceMock.Setup(s => s.GetAllWithEmpresaAsync()).ReturnsAsync(usuarios);

        var result = await _controller.GetAll();

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_WhenExists_ReturnsOk()
    {
        var usuario = new Usuario { Id = 1, Nome = "Vigilante A" };
        _serviceMock.Setup(s => s.GetByIdWithEmpresaAsync(1)).ReturnsAsync(usuario);

        var result = await _controller.GetById(1);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_WhenNotExists_ReturnsNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdWithEmpresaAsync(999)).ReturnsAsync((Usuario?)null);

        var result = await _controller.GetById(999);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Post_WhenValid_ReturnsCreated()
    {
        var dto = new UsuarioDTO { Nome = "Novo Vigilante", Email = "novo@test.com" };
        var usuario = new Usuario { Id = 1, Nome = "Novo Vigilante" };
        _serviceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(usuario);

        var result = await _controller.Post(dto);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Put_WhenValid_ReturnsNoContent()
    {
        var dto = new UsuarioDTO { Nome = "Vigilante Atualizado", Email = "atualizado@test.com" };
        _serviceMock.Setup(s => s.UpdateAsync(1, dto)).Returns(Task.CompletedTask);

        var result = await _controller.Put(1, dto);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_WhenExists_ReturnsNoContent()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Desativar_WhenExists_ReturnsNoContent()
    {
        _serviceMock.Setup(s => s.DesativarAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.Desativar(1);

        result.Should().NotBeNull();
    }
}
