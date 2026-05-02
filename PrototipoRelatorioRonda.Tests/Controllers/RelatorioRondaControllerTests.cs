using Moq;
using Xunit;
using FluentAssertions;
using PrototipoRelatorioRonda.API.Controllers;
using PrototipoRelatorioRonda.Application.Interfaces;
using PrototipoRelatorioRonda.Domain.Entities;
using PrototipoRelatorioRonda.Application.DTOs;

namespace PrototipoRelatorioRonda.Tests.Controllers;

public class RelatorioRondaControllerTests
{
    private readonly Mock<IRelatorioRondaService> _serviceMock;
    private readonly RelatorioRondaController _controller;

    public RelatorioRondaControllerTests()
    {
        _serviceMock = new Mock<IRelatorioRondaService>();
        _controller = new RelatorioRondaController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var relatorios = new List<RelatorioRonda>
        {
            new RelatorioRonda { Id = 1, Data = DateTime.Today },
            new RelatorioRonda { Id = 2, Data = DateTime.Today }
        };
        _serviceMock.Setup(s => s.GetAllWithDetailsAsync()).ReturnsAsync(relatorios);

        var result = await _controller.GetAll();

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_WhenExists_ReturnsOk()
    {
        var relatorio = new RelatorioRonda { Id = 1, Data = DateTime.Today };
        _serviceMock.Setup(s => s.GetByIdWithDetailsAsync(1)).ReturnsAsync(relatorio);

        var result = await _controller.GetById(1);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_WhenNotExists_ReturnsNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdWithDetailsAsync(999)).ReturnsAsync((RelatorioRonda?)null);

        var result = await _controller.GetById(999);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Post_WhenValid_ReturnsCreated()
    {
        var dto = new RelatorioRondaDTO { Data = DateTime.Today, EmpresaId = 1, VigilanteId = 1 };
        var relatorio = new RelatorioRonda { Id = 1, Data = DateTime.Today };
        _serviceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(relatorio);

        var result = await _controller.Post(dto);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Put_WhenValid_ReturnsNoContent()
    {
        var dto = new RelatorioRondaDTO { Data = DateTime.Today, EmpresaId = 1, VigilanteId = 1 };
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
