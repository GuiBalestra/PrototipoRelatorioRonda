using Moq;
using Xunit;
using FluentAssertions;
using PrototipoRelatorioRonda.Services;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;
using AutoMapper;

namespace PrototipoRelatorioRonda.Tests.Services;

public class EmpresaServiceTests
{
    private readonly Mock<IEmpresaRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly EmpresaService _service;

    public EmpresaServiceTests()
    {
        _repoMock = new Mock<IEmpresaRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new EmpresaService(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEmpresas()
    {
        var list = new List<Empresa> { new Empresa { Id = 1, Nome = "A" }, new Empresa { Id = 2, Nome = "B" } };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ReturnsEmpresa()
    {
        var empresa = new Empresa { Id = 1, Nome = "A" };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(empresa);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Nome.Should().Be("A");
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Empresa)null!);

        var result = await _service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_WhenNomeUnique_Creates()
    {
        var dto = new EmpresaDTO { Nome = "Nova" };
        var empresa = new Empresa { Id = 1, Nome = "Nova" };

        _repoMock.Setup(r => r.NomeExisteAsync(dto.Nome, null)).ReturnsAsync(false);
        _mapperMock.Setup(m => m.Map<Empresa>(dto)).Returns(empresa);
        _repoMock.Setup(r => r.AddAsync(empresa)).ReturnsAsync(empresa);

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Nome.Should().Be("Nova");
    }

    [Fact]
    public async Task CreateAsync_WhenNomeExists_Throws()
    {
        var dto = new EmpresaDTO { Nome = "Existe" };
        _repoMock.Setup(r => r.NomeExisteAsync(dto.Nome, null)).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task UpdateAsync_WhenExists_Updates()
    {
        var empresa = new Empresa { Id = 1, Nome = "Antiga" };
        var dto = new EmpresaDTO { Nome = "Nova" };

        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(empresa);
        _repoMock.Setup(r => r.NomeExisteAsync(dto.Nome, 1)).ReturnsAsync(false);
        _mapperMock.Setup(m => m.Map(dto, empresa));

        await _service.UpdateAsync(1, dto);

        _repoMock.Verify(r => r.UpdateAsync(empresa), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotExists_Throws()
    {
        var dto = new EmpresaDTO { Nome = "Nova" };
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Empresa)null!);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(999, dto));
    }

    [Fact]
    public async Task DeleteAsync_WhenExists_Deletes()
    {
        var empresa = new Empresa { Id = 1, Nome = "A" };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(empresa);

        await _service.DeleteAsync(1);

        _repoMock.Verify(r => r.DeleteAsync(empresa), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenNotExists_Throws()
    {
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Empresa)null!);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(999));
    }

    [Fact]
    public async Task DesativarAsync_WhenExists_SetsAtivoFalse()
    {
        var empresa = new Empresa { Id = 1, Nome = "A", Ativo = true };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(empresa);
        _repoMock.Setup(r => r.DesativarAsync(It.IsAny<Empresa>()))
                .Callback<Empresa>(e => e.Ativo = false);

        await _service.DesativarAsync(1);

        empresa.Ativo.Should().BeFalse();
        _repoMock.Verify(r => r.DesativarAsync(empresa), Times.Once);
    }
}
