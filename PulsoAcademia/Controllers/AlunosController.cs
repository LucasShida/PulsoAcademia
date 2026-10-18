using PulsoAcademia.Models;
using PulsoAcademia.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PulsoAcademia.Controllers;

public class AlunosController : Controller
{
    private const int IdadeMinima = 14;

    private readonly AlunoRepositorio _repositorio;

    public AlunosController(AlunoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    // READ (lista) + PESQUISA: GET /Alunos?busca=maria&plano=Anual
    public IActionResult Index(string? busca, Plano? plano)
    {
        var alunos = _repositorio.Pesquisar(busca, plano);

        ViewBag.Busca = busca;
        ViewBag.PlanoSelecionado = plano;
        return View(alunos);
    }

    // READ (detalhes): GET /Alunos/Details/3
    public IActionResult Details(int id)
    {
        var aluno = _repositorio.ObterPorId(id);
        if (aluno is null) return NotFound();
        return View(aluno);
    }

    // CREATE (formulário): GET /Alunos/Create
    public IActionResult Create()
    {
        CarregarModalidades();
        return View(new Aluno());
    }

    // CREATE (salvar): POST /Alunos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Aluno aluno)
    {
        ValidarRegrasDeNegocio(aluno);

        if (!ModelState.IsValid)
        {
            CarregarModalidades(aluno.Modalidade);
            return View(aluno);
        }

        _repositorio.Adicionar(aluno);
        TempData["Mensagem"] = $"\"{aluno.NomeCompleto}\" foi matriculado(a) com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    // UPDATE (formulário): GET /Alunos/Edit/3
    public IActionResult Edit(int id)
    {
        var aluno = _repositorio.ObterPorId(id);
        if (aluno is null) return NotFound();

        CarregarModalidades(aluno.Modalidade);
        return View(aluno);
    }

    // UPDATE (salvar): POST /Alunos/Edit/3
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Aluno aluno)
    {
        if (id != aluno.Id) return BadRequest();

        ValidarRegrasDeNegocio(aluno);

        if (!ModelState.IsValid)
        {
            CarregarModalidades(aluno.Modalidade);
            return View(aluno);
        }

        if (!_repositorio.Atualizar(aluno)) return NotFound();

        TempData["Mensagem"] = $"Os dados de \"{aluno.NomeCompleto}\" foram atualizados.";
        return RedirectToAction(nameof(Index));
    }

    // DELETE (tela de confirmação): GET /Alunos/Delete/3
    public IActionResult Delete(int id)
    {
        var aluno = _repositorio.ObterPorId(id);
        if (aluno is null) return NotFound();
        return View(aluno);
    }

    // DELETE (executa depois de confirmar): POST /Alunos/Delete/3
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var aluno = _repositorio.ObterPorId(id);
        if (aluno is null) return NotFound();

        _repositorio.Remover(id);
        TempData["Mensagem"] = $"A matrícula de \"{aluno.NomeCompleto}\" foi excluída.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>Regras que as Data Annotations sozinhas não cobrem.</summary>
    private void ValidarRegrasDeNegocio(Aluno aluno)
    {
        if (aluno.DataNascimento.Date > DateTime.Today)
        {
            ModelState.AddModelError(nameof(Aluno.DataNascimento), "A data de nascimento não pode ser no futuro.");
        }
        else if (aluno.Idade < IdadeMinima)
        {
            ModelState.AddModelError(nameof(Aluno.DataNascimento), $"A idade mínima para matrícula é {IdadeMinima} anos.");
        }

        if (aluno.DataMatricula.Date > DateTime.Today.AddDays(30))
        {
            ModelState.AddModelError(nameof(Aluno.DataMatricula), "A matrícula não pode ser agendada para mais de 30 dias à frente.");
        }
    }

    /// <summary>Monta o dropdown de modalidades com dados vindos do Controller.</summary>
    private void CarregarModalidades(string? selecionada = null)
    {
        ViewBag.Modalidades = new SelectList(AlunoRepositorio.Modalidades, selecionada);
    }
}
