using System.ComponentModel.DataAnnotations;
using System.Reflection;
using PulsoAcademia.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PulsoAcademia.TagHelpers;

/// <summary>
/// Uso: &lt;plano-badge valor="@aluno.Plano"&gt;&lt;/plano-badge&gt;
/// Gera um selo colorido com o nome do plano (lido do atributo [Display] do enum).
/// </summary>
[HtmlTargetElement("plano-badge")]
public class PlanoBadgeTagHelper : TagHelper
{
    public Plano Valor { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("class", $"plano-badge plano-{Valor.ToString().ToLowerInvariant()}");
        output.Content.SetContent(ObterNomeAmigavel(Valor));
    }

    public static string ObterNomeAmigavel(Plano plano)
    {
        var membro = typeof(Plano).GetMember(plano.ToString()).FirstOrDefault();
        var display = membro?.GetCustomAttribute<DisplayAttribute>();
        return display?.Name ?? plano.ToString();
    }
}
