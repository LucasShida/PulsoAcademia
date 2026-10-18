using System.Globalization;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PulsoAcademia.TagHelpers;

/// <summary>
/// Uso: &lt;data-br valor="aluno.DataMatricula"&gt;&lt;/data-br&gt;
/// Gera &lt;time&gt; com a data por extenso em português (ex.: 2 de setembro de 2026).
/// Atributo opcional: curta="true" mostra dd/MM/yyyy.
/// </summary>
[HtmlTargetElement("data-br")]
public class DataBrTagHelper : TagHelper
{
    private static readonly CultureInfo PtBr = new("pt-BR");

    public DateTime Valor { get; set; }

    public bool Curta { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "time";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("datetime", Valor.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

        var texto = Curta
            ? Valor.ToString("dd/MM/yyyy", PtBr)
            : Valor.ToString("d 'de' MMMM 'de' yyyy", PtBr);

        output.Content.SetContent(texto);
    }
}
