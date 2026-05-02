using PrototipoRelatorioRonda.Domain.Enums;

namespace PrototipoRelatorioRonda.Domain.Entities;

public class Usuario : BaseModel
{
    #region Propriedades
    public string Nome { get; set; } = string.Empty;
    public string HashSenha { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int EmpresaId { get; set; }
    public Funcao Funcao { get; set; }


    #region Propriedades de navegação
    public virtual Empresa? Empresa { get; set; }
    #endregion

    #endregion

    #region Métodos
    #endregion
}
