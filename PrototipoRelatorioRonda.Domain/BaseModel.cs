using PrototipoRelatorioRonda.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PrototipoRelatorioRonda.Domain;

public abstract class BaseModel : IAtivavel
{
    #region Propriedades
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public bool Ativo { get; set; } = true;
    [Required]
    public DateTime CriadoEm { get; set; } = DateTime.Now;
    #endregion
}
