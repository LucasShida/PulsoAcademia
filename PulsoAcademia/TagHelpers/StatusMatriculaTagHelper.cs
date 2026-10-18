using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PulsoAcademia.TagHelpers;

/// <summary>
/// Uso: &lt;status-matricula ativo="aluno.Ativo"&gt;&lt;/status-matricula&gt;
/// Mostra "Ativa" (verde) ou "Trancada" (cinza) com uma bolinha indicadora.
/// </summary>
[HtmlTargetElement("status-matricula")]
public class StatusMatriculaTagHelper : TagHelper
{
    public bool Ativo { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("class", Ativo ? "status status-ativa" : "status status-trancada");
        output.Content.SetContent(Ativo ? "Ativa" : "Trancada");
    }
}
