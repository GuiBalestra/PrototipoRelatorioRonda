namespace PrototipoRelatorioRonda.Models;

public class VoltaRonda : BaseModel
{
    #region Propriedades
    public int RelatorioRondaId { get; set; }
    public int NumeroVolta { get; set; }
    public DateTime? HoraSaida { get; set; }
    public DateTime? HoraChegada { get; set; }
    public DateTime? HoraDescanso { get; set; }
    public string? Observacoes { get; set; } = string.Empty;

    #region Propriedades de navegação
    public virtual RelatorioRonda? RelatorioRonda { get; set; }
    #endregion

    #endregion
}
