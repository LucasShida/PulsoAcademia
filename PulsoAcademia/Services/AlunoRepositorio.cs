using PulsoAcademia.Models;

namespace PulsoAcademia.Services;

/// <summary>
/// "Banco de dados" em memória. Os dados somem quando a aplicação é reiniciada
/// (o enunciado do checkpoint pede para NÃO usar banco de dados).
/// </summary>
public class AlunoRepositorio
{
    private readonly List<Aluno> _alunos = new();
    private readonly object _trava = new();
    private int _proximoId = 1;

    /// <summary>Lista fixa de modalidades exibida no dropdown do formulário (vem do Controller).</summary>
    public static readonly IReadOnlyList<string> Modalidades = new[]
    {
        "Musculação",
        "Funcional",
        "Spinning",
        "Crossfit",
        "Pilates",
        "Yoga",
        "Natação",
        "Muay Thai"
    };

    public AlunoRepositorio()
    {
        // Dados de exemplo para a tela não começar vazia.
        Adicionar(new Aluno { NomeCompleto = "Mariana Souza Lima", Email = "mariana.lima@email.com", DataNascimento = new DateTime(1994, 3, 14), DataMatricula = new DateTime(2026, 9, 2), Plano = Plano.Anual, Modalidade = "Crossfit", Mensalidade = 129.90m, PesoKg = 63.5m, Ativo = true, Objetivo = "Ganho de condicionamento para uma prova de 10 km." });
        Adicionar(new Aluno { NomeCompleto = "Rafael Teixeira", Email = "rafael.teixeira@email.com", DataNascimento = new DateTime(1988, 11, 3), DataMatricula = new DateTime(2026, 6, 20), Plano = Plano.Semestral, Modalidade = "Musculação", Mensalidade = 149.90m, PesoKg = 86.2m, Ativo = true, Objetivo = "Hipertrofia, treino ABC cinco vezes por semana." });
        Adicionar(new Aluno { NomeCompleto = "Helena Prado", Email = "helena.prado@email.com", DataNascimento = new DateTime(1979, 7, 28), DataMatricula = new DateTime(2026, 3, 9), Plano = Plano.Trimestral, Modalidade = "Pilates", Mensalidade = 189.00m, PesoKg = 68.0m, Ativo = false, Objetivo = "Reabilitação de lombar. Matrícula trancada em agosto." });
        Adicionar(new Aluno { NomeCompleto = "Diego Almeida", Email = "diego.almeida@email.com", DataNascimento = new DateTime(2001, 1, 19), DataMatricula = new DateTime(2026, 10, 5), Plano = Plano.Mensal, Modalidade = "Spinning", Mensalidade = 169.90m, PesoKg = 74.8m, Ativo = true, Objetivo = "Emagrecimento e melhora do fôlego." });
    }

    public List<Aluno> ObterTodos()
    {
        lock (_trava)
        {
            return _alunos.OrderByDescending(a => a.DataMatricula).ToList();
        }
    }

    public Aluno? ObterPorId(int id)
    {
        lock (_trava)
        {
            return _alunos.FirstOrDefault(a => a.Id == id);
        }
    }

    /// <summary>Pesquisa pelo nome ou e-mail (ignorando maiúsculas) e, opcionalmente, por plano.</summary>
    public List<Aluno> Pesquisar(string? termo, Plano? plano)
    {
        lock (_trava)
        {
            IEnumerable<Aluno> consulta = _alunos;

            if (!string.IsNullOrWhiteSpace(termo))
            {
                var t = termo.Trim();
                consulta = consulta.Where(a =>
                    a.NomeCompleto.Contains(t, StringComparison.OrdinalIgnoreCase) ||
                    a.Email.Contains(t, StringComparison.OrdinalIgnoreCase));
            }

            if (plano.HasValue)
            {
                consulta = consulta.Where(a => a.Plano == plano.Value);
            }

            return consulta.OrderByDescending(a => a.DataMatricula).ToList();
        }
    }

    public void Adicionar(Aluno aluno)
    {
        lock (_trava)
        {
            aluno.Id = _proximoId++;
            _alunos.Add(aluno);
        }
    }

    public bool Atualizar(Aluno aluno)
    {
        lock (_trava)
        {
            var existente = _alunos.FirstOrDefault(a => a.Id == aluno.Id);
            if (existente is null) return false;

            existente.NomeCompleto = aluno.NomeCompleto;
            existente.Email = aluno.Email;
            existente.DataNascimento = aluno.DataNascimento;
            existente.DataMatricula = aluno.DataMatricula;
            existente.Plano = aluno.Plano;
            existente.Modalidade = aluno.Modalidade;
            existente.Mensalidade = aluno.Mensalidade;
            existente.PesoKg = aluno.PesoKg;
            existente.Ativo = aluno.Ativo;
            existente.Objetivo = aluno.Objetivo;
            return true;
        }
    }

    public bool Remover(int id)
    {
        lock (_trava)
        {
            var aluno = _alunos.FirstOrDefault(a => a.Id == id);
            if (aluno is null) return false;
            _alunos.Remove(aluno);
            return true;
        }
    }
}
