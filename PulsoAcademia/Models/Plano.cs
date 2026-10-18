using System.ComponentModel.DataAnnotations;

namespace PulsoAcademia.Models;

/// <summary>Tipo de plano contratado pelo aluno (enum exibido como dropdown).</summary>
public enum Plano
{
    [Display(Name = "Mensal")]
    Mensal,

    [Display(Name = "Trimestral")]
    Trimestral,

    [Display(Name = "Semestral")]
    Semestral,

    [Display(Name = "Anual")]
    Anual
}
