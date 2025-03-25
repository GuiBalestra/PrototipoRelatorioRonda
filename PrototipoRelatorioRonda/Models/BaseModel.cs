using PrototipoRelatorioRonda.Models.Interface;
using System.ComponentModel.DataAnnotations;

namespace PrototipoRelatorioRonda.Models;

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
