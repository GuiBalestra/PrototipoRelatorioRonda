using System.ComponentModel.DataAnnotations;

namespace PrototipoRelatorioRonda.Models;

public class Empresa : BaseModel
{
    #region Propriedades
    [Required]
    public string Nome { get; set; } = string.Empty;

    #region Propriedades de navegação
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    #endregion

    #endregion

    #region Métodos
    #endregion
}
