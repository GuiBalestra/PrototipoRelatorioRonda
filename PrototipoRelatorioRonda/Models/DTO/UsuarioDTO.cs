using PrototipoRelatorioRonda.Models.Enums;

namespace PrototipoRelatorioRonda.Models.DTO;

public class UsuarioDTO
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int EmpresaId { get; set; }
    public Funcao Funcao { get; set; }
}
