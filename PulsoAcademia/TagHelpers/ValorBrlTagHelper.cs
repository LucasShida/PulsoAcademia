using System.Globalization;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PulsoAcademia.TagHelpers;

/// <summary>
/// Uso: &lt;valor-brl valor="aluno.Mensalidade"&gt;&lt;/valor-brl&gt;
/// Formata um decimal como moeda brasileira (ex.: R$ 149,90), independente da cultura do servidor.
/// </summary>
[HtmlTargetElement("valor-brl")]
public class ValorBrlTagHelper : TagHelper
{
    private static readonly CultureInfo PtBr = new("pt-BR");

    public decimal Valor { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("class", "valor-brl");
        output.Content.SetContent(Valor.ToString("C2", PtBr));
    }
}
