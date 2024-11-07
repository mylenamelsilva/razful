using API.DTOs.Aluno;
using API.Services.Aluno;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoService _alunoService;

        public AlunosController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RetornoAlunoDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public IActionResult CriarAluno(CriacaoAtualizacaoAlunoDto model)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

                return BadRequest(erros);
            }

            var aluno = _alunoService.CriarAluno(model);

            if (aluno.Id == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new List<string> { "Houve um erro ao inserir o aluno." });
            }

            if (aluno.Id == -1)
            {
                return BadRequest("Usuário já em uso.");
            }

            return CreatedAtAction(nameof(CriarAluno), new { id = aluno.Id }, aluno);
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RetornoTodosAlunosDto))]
        public IActionResult ListarAlunos([FromQuery] int pagina = 1, [FromQuery] int registrosPorPagina = 10)
        {
            return Ok(_alunoService.ListarTodosAlunos(pagina,   registrosPorPagina));
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RetornoAlunoDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ListarAlunoPorUsuario([FromQuery] string usuario)
        {
            var aluno = _alunoService.ListarAlunoPorUsuario(usuario);

            if (aluno == null)
            {
                return NotFound();
            }

            return Ok(aluno);
        }

        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CriacaoAtualizacaoAlunoDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public IActionResult AtualizarAluno(CriacaoAtualizacaoAlunoDto model, [FromQuery] string usuario)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

                return BadRequest(erros);
            }

            var aluno = _alunoService.AtualizarAluno(model, usuario);

            if (aluno == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new List<string> { "Houve um erro ao atualizar o aluno." });
            }

            if (aluno == -1)
            {
                return BadRequest("Usuário não existente.");
            }

            return Ok(model);
        }

        [HttpDelete]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CriacaoAtualizacaoAlunoDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public IActionResult RemoverAluno([FromQuery] string usuario)
        {
            var aluno = _alunoService.RemoverAluno(usuario);

            if (aluno == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new List<string> { "Houve um erro ao remover o aluno." });
            }

            if (aluno == -1)
            {
                return BadRequest("Usuário não existente.");
            }

            return Ok();
        }
    }
}
