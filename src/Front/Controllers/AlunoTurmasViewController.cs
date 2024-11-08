using API.DTOs.Aluno;
using API.DTOs.AlunoTurma;
using Front.Models.AlunoTurma;
using Front.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Front.Controllers
{
    public class AlunoTurmasViewController : Controller
    {
        private readonly AlunoTurmaViewService _alunoTurmaService;
        private readonly AlunoHttpService _alunoService;
        private readonly TurmaHttpService _turmaService;

        public AlunoTurmasViewController(AlunoTurmaViewService alunoTurmaService, AlunoHttpService alunoService, TurmaHttpService turmaService)
        {
            _alunoTurmaService = alunoTurmaService;
            _alunoService = alunoService;
            _turmaService = turmaService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pagina = 1, int registrosPorPagina = 10)
        {
            var listaTurmas = await _alunoTurmaService.ListarTodasAssociacoesAsync(pagina, registrosPorPagina);
            return View(listaTurmas);
        }

        [HttpGet]
        public async Task<IActionResult> CriarAssociacao()
        {
            var alunos = await _alunoService.ListarTodosAlunos(1, 500);
            var turmas = await _turmaService.ListarTodasTurmas(1, 500);

            var alunoTurmaDropdown = new DropdownAlunoTurmaViewModel()
            {
                Alunos = alunos?.Alunos.Select(a => new SelectListItem { Value = a.Usuario.ToString(), Text = a.Usuario }),
                Turmas = turmas?.Turmas.Select(t => new SelectListItem { Value = t.Turma.ToString(), Text = t.Turma })
            };

            return View(alunoTurmaDropdown);
        }

        [HttpGet("AtualizarAssociacao/{turma}")]
        public async Task<IActionResult> AtualizarAssociacao([FromRoute] string turma)
        {
            var alunos = await _alunoService.ListarTodosAlunos(1, 500);
            var turmas = await _turmaService.ListarTodasTurmas(1, 500);

            var alunoTurmaDropdown = new DropdownAlunoTurmaViewModel()
            {
                Alunos = alunos?.Alunos.Select(a => new SelectListItem { Value = a.Usuario.ToString(), Text = a.Usuario }),
                Turmas = turmas?.Turmas.Select(t => new SelectListItem { Value = t.Turma.ToString(), Text = t.Turma }),
                Turma = turma
            };

            return View(alunoTurmaDropdown);
        }

        [HttpGet]
        public async Task<IActionResult> ListarAlunosPorTurma(string turma, int pagina = 1, int registrosPorPagina = 10)
        {
            var associacoes = await _alunoTurmaService.ListarAlunosPorTurmaAsync(turma, pagina, registrosPorPagina);

            var associacoesView = new AlunosAssociadosViewModel()
            {
                Pagina = associacoes.Pagina,
                TotalParaPaginacao = associacoes.TotalParaPaginacao,
                Turma = turma,
                Alunos = associacoes.Alunos
            };

            return View(associacoesView);
        }

        [HttpPost]
        public async Task<IActionResult> CriarAssociacao(CriacaoAtualizacaoAlunoTurmaDto req)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["Erros"] = erros;
                return RedirectToAction("CriarAssociacao");
            }

            try
            {
                var aluno = await _alunoTurmaService.CriarAssociacaoAsync(req);

                if (!string.IsNullOrEmpty(aluno.Erro))
                {
                    TempData["Mensagem"] = aluno.Erro;
                    return RedirectToAction("CriarAssociacao");
                }

                TempData["Mensagem"] = "Associação cadastrada.";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Mensagem"] = "Houve um erro ao cadastrar a associação.";
                return RedirectToAction("CriarAssociacao");
            }
        }

        [HttpPost("AtualizarAssociacao/{turma}")]
        public async Task<IActionResult> AtualizarAssociacao(CriacaoAtualizacaoAlunoTurmaDto req, [FromRoute] string turma)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                TempData["Erros"] = erros;
                return RedirectToAction("AtualizarAssociacao");
            }

            try
            {
                var aluno = await _alunoTurmaService.AtualizarAssociacaoAsync(req, turma);

                if (!string.IsNullOrEmpty(aluno.Erro))
                {
                    TempData["Mensagem"] = aluno.Erro;
                    return RedirectToAction("AtualizarAssociacao");
                }

                TempData["Mensagem"] = "Associação atualizada.";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Mensagem"] = "Houve um erro ao atualizar a associação.";
                return RedirectToAction("AtualizarAssociacao");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoverAssociacao(string aluno, string turma)
        {
            try
            {
                var associacao = await _alunoTurmaService.RemoverAssociacaoAsync(aluno, turma);

                if (!string.IsNullOrEmpty(associacao.Erro))
                {
                    ViewData["Mensagem"] = associacao.Erro;
                    return View();
                }

                TempData["Mensagem"] = "Associação removida com sucesso.";
                return RedirectToAction("Index");
            }
            catch
            {
                ViewData["Mensagem"] = "Houve um erro ao remover a associação.";
                return View();
            }
        }

        [HttpPost("RemoverTodaAssociacao/{turma}")]
        public async Task<IActionResult> RemoverTodaAssociacao([FromRoute] string turma)
        {
            try
            {
                var associacao = await _alunoTurmaService.RemoverTodaAssociacaoAsync(turma);

                if (!string.IsNullOrEmpty(associacao.Erro))
                {
                    ViewData["Mensagem"] = associacao.Erro;
                    return View();
                }

                TempData["Mensagem"] = "Associação removida com sucesso.";
                return RedirectToAction("Index");
            }
            catch
            {
                ViewData["Mensagem"] = "Houve um erro ao remover a associação.";
                return View();
            }
        }
    }
}
