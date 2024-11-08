using API.DTOs.Aluno;
using Front.Services;
using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers
{
    public class AlunosHttpController : Controller
    {
        private readonly AlunoHttpService _alunoHttpService;

        public AlunosHttpController(AlunoHttpService alunoHttpService)
        {
            _alunoHttpService = alunoHttpService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pagina = 1, int registrosPorPagina = 10)
        {
            var listaAlunos = await _alunoHttpService.ListarTodosAlunos(pagina, registrosPorPagina);
            return View(listaAlunos);
        }

        [HttpGet]
        public IActionResult CriarAluno()
        {
            return View();
        }

        [HttpGet("AtualizarAluno/{usuario}")]
        public async Task<IActionResult> AtualizarAluno([FromRoute] string usuario)
        {
            var aluno = await _alunoHttpService.ListarAlunoPorUsuarioAsync(usuario);
            var alunoReq = new CriacaoAtualizacaoAlunoDto()
            {
                Nome = aluno.Nome,
                Usuario = aluno.Usuario,
                Senha = aluno.Senha
            };
            return View(alunoReq);
        }

        [HttpGet("ListarInformacaoAluno/{usuario}")]
        public async Task<IActionResult> ListarInformacaoAluno([FromRoute] string usuario)
        {
            var aluno = await _alunoHttpService.ListarAlunoPorUsuarioAsync(usuario);
            return View(aluno);
        }

        [HttpPost]
        public async Task<IActionResult> CriarAluno(CriacaoAtualizacaoAlunoDto req)
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
                var aluno = await _alunoHttpService.CriarAlunoAsync(req);

                if (!string.IsNullOrEmpty(aluno.Erro))
                {
                    ViewData["Mensagem"] = aluno.Erro;
                    return View(req);
                }

                TempData["Mensagem"] = "Aluno cadastrado.";
                return RedirectToAction("Index");
            }
            catch
            {
                ViewData["Mensagem"] = "Houve um erro ao cadastrar o aluno.";
                return View(req);
            }
        }

        [HttpPost("AtualizarAluno/{usuario}")]
        public async Task<IActionResult> AtualizarAluno(CriacaoAtualizacaoAlunoDto req, [FromRoute] string usuario)
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
                var aluno = await _alunoHttpService.AtualizarAlunoAsync(req, usuario);

                if (!string.IsNullOrEmpty(aluno.Erro))
                {
                    ViewData["Mensagem"] = aluno.Erro;
                    return View(req);
                }

                TempData["Mensagem"] = "Aluno atualizado.";
                return RedirectToAction("Index");
            }
            catch
            {
                ViewData["Mensagem"] = "Houve um erro ao atualizar o aluno.";
                return View(req);
            }
        }

        [HttpPost("RemoverAluno/{usuario}")]
        public async Task<IActionResult> RemoverAluno([FromRoute] string usuario)
        {

            try
            {
                var aluno = await _alunoHttpService.RemoverAlunoAsync(usuario);

                if (!string.IsNullOrEmpty(aluno.Erro))
                {
                    ViewData["Mensagem"] = aluno.Erro;
                    return View();
                }

                TempData["Mensagem"] = "Aluno removido.";
                return RedirectToAction("Index");
            }
            catch
            {
                ViewData["Mensagem"] = "Houve um erro ao remover o aluno.";
                return View();
            }
        }
    }
}
