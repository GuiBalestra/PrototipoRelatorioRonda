using Moq;
using Xunit;
using FluentAssertions;
using PrototipoRelatorioRonda.Services;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;
using AutoMapper;

namespace PrototipoRelatorioRonda.Tests.Services;

public class RelatorioRondaServiceTests
{
    private readonly Mock<IRelatorioRondaRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly RelatorioRondaService _service;

    public RelatorioRondaServiceTests()
    {
        _repoMock = new Mock<IRelatorioRondaRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new RelatorioRondaService(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllWithDetailsAsync_ReturnsAll()
    {
        var list = new List<RelatorioRonda> { new RelatorioRonda { Id = 1 }, new RelatorioRonda { Id = 2 } };
        _repoMock.Setup(r => r.GetAllWithDetailsAsync()).ReturnsAsync(list);

        var result = await _service.GetAllWithDetailsAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task CreateAsync_WhenUnique_Creates()
    {
        var dto = new RelatorioRondaDTO { Data = DateTime.Today, EmpresaId = 1, VigilanteId = 1 };
        var relatorio = new RelatorioRonda { Id = 1, Data = dto.Data };

        _repoMock.Setup(r => r.RelatorioExisteParaDataAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(false);
        _repoMock.Setup(r => r.EmpresaExisteAsync(It.IsAny<int>())).ReturnsAsync(true);
        _repoMock.Setup(r => r.VigilanteExisteAsync(It.IsAny<int>())).ReturnsAsync(true);
        _mapperMock.Setup(m => m.Map<RelatorioRonda>(It.IsAny<RelatorioRondaDTO>())).Returns(relatorio);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<RelatorioRonda>())).ReturnsAsync(relatorio);

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAsync_WhenDuplicate_Throws()
    {
        var dto = new RelatorioRondaDTO { Data = DateTime.Today, EmpresaId = 1, VigilanteId = 1 };
        _repoMock.Setup(r => r.EmpresaExisteAsync(It.IsAny<int>())).ReturnsAsync(true);
        _repoMock.Setup(r => r.VigilanteExisteAsync(It.IsAny<int>())).ReturnsAsync(true);
        _repoMock.Setup(r => r.RelatorioExisteParaDataAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_WhenEmpresaNotExists_Throws()
    {
        var dto = new RelatorioRondaDTO { Data = DateTime.Today, EmpresaId = 999, VigilanteId = 1 };
        _repoMock.Setup(r => r.RelatorioExisteParaDataAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(false);
        _repoMock.Setup(r => r.EmpresaExisteAsync(It.IsAny<int>())).ReturnsAsync(false);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CreateAsync(dto));
    }
}
