using Moq;
using Xunit;
using FluentAssertions;
using PrototipoRelatorioRonda.Services;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;
using AutoMapper;

namespace PrototipoRelatorioRonda.Tests.Services;

public class VoltaRondaServiceTests
{
    private readonly Mock<IVoltaRondaRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly VoltaRondaService _service;

    public VoltaRondaServiceTests()
    {
        _repoMock = new Mock<IVoltaRondaRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new VoltaRondaService(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllWithRelatorioAsync_ReturnsAll()
    {
        var list = new List<VoltaRonda> { new VoltaRonda { Id = 1, NumeroVolta = 1 }, new VoltaRonda { Id = 2, NumeroVolta = 2 } };
        _repoMock.Setup(r => r.GetAllWithRelatorioAsync()).ReturnsAsync(list);

        var result = await _service.GetAllWithRelatorioAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task CreateAsync_WhenNumeroVoltaUnique_Creates()
    {
        var dto = new VoltaRondaDTO { NumeroVolta = 1, RelatorioRondaId = 1, Observacoes = "OK" };
        var volta = new VoltaRonda { Id = 1, NumeroVolta = 1 };

        _repoMock.Setup(r => r.NumeroVoltaExisteAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>())).ReturnsAsync(false);
        _repoMock.Setup(r => r.RelatorioRondaExisteAsync(It.IsAny<int>())).ReturnsAsync(true);
        _mapperMock.Setup(m => m.Map<VoltaRonda>(It.IsAny<VoltaRondaDTO>())).Returns(volta);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<VoltaRonda>())).ReturnsAsync(volta);

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.NumeroVolta.Should().Be(1);
    }

    [Fact]
    public async Task CreateAsync_WhenNumeroVoltaExists_Throws()
    {
        var dto = new VoltaRondaDTO { NumeroVolta = 1, RelatorioRondaId = 1 };
        _repoMock.Setup(r => r.RelatorioRondaExisteAsync(It.IsAny<int>())).ReturnsAsync(true);
        _repoMock.Setup(r => r.NumeroVoltaExisteAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_WhenRelatorioNotExists_Throws()
    {
        var dto = new VoltaRondaDTO { NumeroVolta = 1, RelatorioRondaId = 999 };
        _repoMock.Setup(r => r.RelatorioRondaExisteAsync(It.IsAny<int>())).ReturnsAsync(false);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task DeleteAsync_WhenNotExists_Throws()
    {
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((VoltaRonda)null!);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(999));
    }
}
