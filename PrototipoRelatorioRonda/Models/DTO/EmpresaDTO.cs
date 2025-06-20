using System.ComponentModel.DataAnnotations;

namespace PrototipoRelatorioRonda.Models.DTO;

public class EmpresaDTO
{
    [Required(ErrorMessage = "O nome da empresa é obrigatório")]
    [StringLength(100, ErrorMessage = "O nome da empresa deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;
}
