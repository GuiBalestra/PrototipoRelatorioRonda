using System.ComponentModel.DataAnnotations.Schema;

namespace PrototipoRelatorioRonda.Models;

public class RelatorioRonda : BaseModel
{
    #region Propriedades
    [ForeignKey("Empresa")]
    public int EmpresaId { get; set; }
    [ForeignKey("Vigilante")]
    public int VigilanteId { get; set; }
    public DateTime Data { get; set; }
    public decimal? KmSaida { get; set; }
    public decimal? KmChegada { get; set; }
    public string? TestemunhaSaida { get; set; } = string.Empty;
    public string? TestemunhaChegada { get; set; } = string.Empty;

    #region Propriedades de navegação
    public virtual Empresa Empresa { get; set; } = new();
    public virtual Usuario Vigilante { get; set; } = new();
    public virtual ICollection<VoltaRonda> Voltas { get; set; } = new List<VoltaRonda>();
    #endregion

    #endregion

    #region Métodos
    #endregion
}
