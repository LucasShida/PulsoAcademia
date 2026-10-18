using System.Globalization;
using Microsoft.AspNetCore.Localization;
using PulsoAcademia.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Sem banco de dados: o repositório guarda tudo em memória (Singleton = vive enquanto a aplicação roda).
builder.Services.AddSingleton<AlunoRepositorio>();

var app = builder.Build();

// Cultura invariante no servidor: campos numéricos (<input type="number">) sempre enviam
// decimais com ponto, e isso evita erro de conversão em computadores configurados em pt-BR.
// A exibição em português (datas e R$) é feita explicitamente pelos Tag Helpers.
var cultura = CultureInfo.InvariantCulture;
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultura),
    SupportedCultures = new List<CultureInfo> { cultura },
    SupportedUICultures = new List<CultureInfo> { cultura }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
