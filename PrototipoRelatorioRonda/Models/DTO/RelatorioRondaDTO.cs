using System.ComponentModel.DataAnnotations;

namespace PrototipoRelatorioRonda.Models.DTO;

public class RelatorioRondaDTO
{
    [Required(ErrorMessage = "O ID da empresa é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "ID da empresa deve ser maior que 0")]
    public int EmpresaId { get; set; }

    [Required(ErrorMessage = "O ID do vigilante é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "ID do vigilante deve ser maior que 0")]
    public int VigilanteId { get; set; }

    [Required(ErrorMessage = "A data é obrigatória")]
    public DateTime Data { get; set; }

    [Range(0, 999999.99, ErrorMessage = "KM de saída deve estar entre 0 e 999999.99")]
    public decimal? KmSaida { get; set; }

    [Range(0, 999999.99, ErrorMessage = "KM de chegada deve estar entre 0 e 999999.99")]
    public decimal? KmChegada { get; set; }

    [StringLength(100, ErrorMessage = "Testemunha de saída deve ter no máximo 100 caracteres")]
    public string? TestemunhaSaida { get; set; }

    [StringLength(100, ErrorMessage = "Testemunha de chegada deve ter no máximo 100 caracteres")]
    public string? TestemunhaChegada { get; set; }
} 
