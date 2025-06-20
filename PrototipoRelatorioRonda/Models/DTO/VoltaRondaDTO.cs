using System.ComponentModel.DataAnnotations;

namespace PrototipoRelatorioRonda.Models.DTO;

public class VoltaRondaDTO
{
    [Required(ErrorMessage = "O ID do relatório de ronda é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "ID do relatório deve ser maior que 0")]
    public int RelatorioRondaId { get; set; }

    [Required(ErrorMessage = "O número da volta é obrigatório")]
    [Range(1, 99, ErrorMessage = "Número da volta deve estar entre 1 e 99")]
    public int NumeroVolta { get; set; }

    public DateTime? HoraSaida { get; set; }

    public DateTime? HoraChegada { get; set; }

    public DateTime? HoraDescanso { get; set; }

    [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
    public string? Observacoes { get; set; }
} 
