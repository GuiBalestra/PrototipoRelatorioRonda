using System.ComponentModel.DataAnnotations;
using PrototipoRelatorioRonda.Models.Enums;

namespace PrototipoRelatorioRonda.Models.DTO;

public class UsuarioDTO
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres")]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "O ID da empresa é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "ID da empresa deve ser maior que 0")]
    public int EmpresaId { get; set; }

    [Required(ErrorMessage = "A função é obrigatória")]
    public Funcao Funcao { get; set; }
}
