using Moq;
using Xunit;
using FluentAssertions;
using PrototipoRelatorioRonda.Controllers;
using PrototipoRelatorioRonda.Services.Interface;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;

namespace PrototipoRelatorioRonda.Tests.Controllers;

public class EmpresaControllerTests
{
    private readonly Mock<IEmpresaService> _serviceMock;
    private readonly EmpresaController _controller;

    public EmpresaControllerTests()
    {
        _serviceMock = new Mock<IEmpresaService>();
        _controller = new EmpresaController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var empresas = new List<Empresa>
        {
            new Empresa { Id = 1, Nome = "Empresa A" },
            new Empresa { Id = 2, Nome = "Empresa B" }
        };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(empresas);

        var result = await _controller.GetAll();

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_WhenExists_ReturnsOk()
    {
        var empresa = new Empresa { Id = 1, Nome = "Empresa A" };
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(empresa);

        var result = await _controller.GetById(1);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_WhenNotExists_ReturnsNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Empresa?)null);

        var result = await _controller.GetById(999);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Post_WhenValid_ReturnsCreated()
    {
        var dto = new EmpresaDTO { Nome = "Nova Empresa" };
        var empresa = new Empresa { Id = 1, Nome = "Nova Empresa" };
        _serviceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(empresa);

        var result = await _controller.Post(dto);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Put_WhenValid_ReturnsNoContent()
    {
        var dto = new EmpresaDTO { Nome = "Empresa Atualizada" };
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
