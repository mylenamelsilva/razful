using API.DTOs.Turma;
using Front.Services;
using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers
{
    public class TurmasHttpController : Controller
    {
        private readonly TurmaHttpService _turmaHttpService;

        public TurmasHttpController(TurmaHttpService turmaHttpService)
        {
            _turmaHttpService = turmaHttpService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pagina = 1, int registrosPorPagina = 10)
        {
            var listaTurmas = await _turmaHttpService.ListarTodasTurmas(pagina, registrosPorPagina);
            return View(listaTurmas);
        }

        [HttpGet]
        public IActionResult CriarTurma()
        {
            return View();
        }

        [HttpGet("AtualizarTurma/{idTurma}")]
        public async Task<IActionResult> AtualizarTurma([FromRoute] int idTurma)
        {
            var turma = await _turmaHttpService.ListarTurmaPorIdAsync(idTurma);
            var turmaReq = new CriacaoAtualizacaoTurmaDto()
            {
                Turma = turma.Turma,
                CursoId = turma.CursoId,
                Ano = turma.Ano,
            };
            return View(turmaReq);
        }

        [HttpGet("ListarInformacaoTurma/{idTurma}")]
        public async Task<IActionResult> ListarInformacaoTurma([FromRoute] int idTurma)
        {
            var turma = await _turmaHttpService.ListarTurmaPorIdAsync(idTurma);
            return View(turma);
        }

        [HttpPost]
        public async Task<IActionResult> CriarTurma(CriacaoAtualizacaoTurmaDto req)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                ViewData["Erros"] = erros;
                return View(req);
            }

            try
            {
                var turma = await _turmaHttpService.CriarTurmaAsync(req);

                if (!string.IsNullOrEmpty(turma.Erro))
                {
                    ViewData["Mensagem"] = turma.Erro;
                    return View(req);
                }

                TempData["Mensagem"] = "Turma cadastrada.";
                return RedirectToAction("Index");
            }
            catch
            {
                ViewData["Mensagem"] = "Houve um erro ao cadastrar a turma.";
                return View(req);
            }
        }

        [HttpPost("AtualizarTurma/{idTurma}")]
        public async Task<IActionResult> AtualizarTurma(CriacaoAtualizacaoTurmaDto req, [FromRoute] int idTurma)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                ViewData["Erros"] = erros;
                return View(req);
            }

            try
            {
                var turma = await _turmaHttpService.AtualizarTurmaAsync(req, idTurma);

                if (!string.IsNullOrEmpty(turma.Erro))
                {
                    ViewData["Mensagem"] = turma.Erro;
                    return View(req);
                }

                TempData["Mensagem"] = "Turma atualizada.";
                return RedirectToAction("Index");
            }
            catch
            {
                ViewData["Mensagem"] = "Houve um erro ao atualizar a turma.";
                return View(req);
            }
        }

        [HttpPost("RemoverTurma/{idTurma}")]
        public async Task<IActionResult> RemoverTurma([FromRoute] int idTurma)
        {
            try
            {
                var turma = await _turmaHttpService.RemoverTurmaAsync(idTurma);

                if (!string.IsNullOrEmpty(turma.Erro))
                {
                    ViewData["Mensagem"] = turma.Erro;
                    return View();
                }

                TempData["Mensagem"] = "Turma removida.";
                return RedirectToAction("Index");
            }
            catch
            {
                ViewData["Mensagem"] = "Houve um erro ao remover a turma.";
                return View();
            }
        }
    }
}
