using System.Diagnostics;
using PulsoAcademia.Models;
using PulsoAcademia.Services;
using Microsoft.AspNetCore.Mvc;

namespace PulsoAcademia.Controllers;

public class HomeController : Controller
{
    private readonly AlunoRepositorio _repositorio;

    public HomeController(AlunoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public IActionResult Index()
    {
        var alunos = _repositorio.ObterTodos();
        var ativos = alunos.Where(a => a.Ativo).ToList();

        ViewBag.Total = alunos.Count;
        ViewBag.Ativos = ativos.Count;
        ViewBag.Receita = ativos.Sum(a => a.Mensalidade);
        ViewBag.Recentes = alunos.Take(3).ToList();
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
