using System.ComponentModel.DataAnnotations;

namespace PulsoAcademia.Models;

/// <summary>Um aluno matriculado na academia.</summary>
public class Aluno
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome completo do aluno.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    [Display(Name = "Nome completo")]
    public string NomeCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o e-mail do aluno.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [StringLength(120, ErrorMessage = "O e-mail deve ter no máximo 120 caracteres.")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a data de nascimento.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de nascimento")]
    public DateTime DataNascimento { get; set; } = DateTime.Today.AddYears(-25);

    [Required(ErrorMessage = "Informe a data da matrícula.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data da matrícula")]
    public DateTime DataMatricula { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Escolha o plano.")]
    [Display(Name = "Plano")]
    public Plano Plano { get; set; }

    [Required(ErrorMessage = "Escolha a modalidade.")]
    [Display(Name = "Modalidade principal")]
    public string Modalidade { get; set; } = string.Empty;

    [Range(0.01, 9999.99, ErrorMessage = "A mensalidade deve estar entre R$ 0,01 e R$ 9.999,99.")]
    [Display(Name = "Mensalidade (R$)")]
    public decimal Mensalidade { get; set; }

    [Range(30, 300, ErrorMessage = "O peso deve estar entre 30 e 300 kg.")]
    [Display(Name = "Peso atual (kg)")]
    public decimal? PesoKg { get; set; }

    [Display(Name = "Matrícula ativa")]
    public bool Ativo { get; set; } = true;

    [StringLength(300, ErrorMessage = "O objetivo deve ter no máximo 300 caracteres.")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Objetivo / observações")]
    public string? Objetivo { get; set; }

    /// <summary>Idade calculada a partir da data de nascimento (não é preenchida no formulário).</summary>
    public int Idade
    {
        get
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - DataNascimento.Year;
            if (DataNascimento.Date > hoje.AddYears(-idade)) idade--;
            return idade;
        }
    }
}
