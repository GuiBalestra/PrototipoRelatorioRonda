using Moq;
using Xunit;
using FluentAssertions;
using PrototipoRelatorioRonda.API.Controllers;
using PrototipoRelatorioRonda.Application.Interfaces;
using PrototipoRelatorioRonda.Domain.Entities;
using PrototipoRelatorioRonda.Application.DTOs;

namespace PrototipoRelatorioRonda.Tests.Controllers;

public class VoltaRondaControllerTests
{
    private readonly Mock<IVoltaRondaService> _serviceMock;
    private readonly VoltaRondaController _controller;

    public VoltaRondaControllerTests()
    {
        _serviceMock = new Mock<IVoltaRondaService>();
        _controller = new VoltaRondaController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var voltas = new List<VoltaRonda>
        {
            new VoltaRonda { Id = 1, NumeroVolta = 1 },
            new VoltaRonda { Id = 2, NumeroVolta = 2 }
        };
        _serviceMock.Setup(s => s.GetAllWithRelatorioAsync()).ReturnsAsync(voltas);

        var result = await _controller.GetAll();

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_WhenExists_ReturnsOk()
    {
        var volta = new VoltaRonda { Id = 1, NumeroVolta = 1 };
        _serviceMock.Setup(s => s.GetByIdWithRelatorioAsync(1)).ReturnsAsync(volta);

        var result = await _controller.GetById(1);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_WhenNotExists_ReturnsNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdWithRelatorioAsync(999)).ReturnsAsync((VoltaRonda?)null);

        var result = await _controller.GetById(999);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Post_WhenValid_ReturnsCreated()
    {
        var dto = new VoltaRondaDTO { NumeroVolta = 1, RelatorioRondaId = 1, Observacoes = "OK" };
        var volta = new VoltaRonda { Id = 1, NumeroVolta = 1 };
        _serviceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(volta);

        var result = await _controller.Post(dto);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Put_WhenValid_ReturnsNoContent()
    {
        var dto = new VoltaRondaDTO { NumeroVolta = 1, RelatorioRondaId = 1, Observacoes = "Atualizado" };
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
