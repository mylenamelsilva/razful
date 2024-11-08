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
        public async Task<IActionResult> CriarAluno(CriacaoAtualizacaoAlunoDto model)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

                return BadRequest(erros);
            }

            var aluno = await _alunoService.CriarAluno(model);

            return aluno.Id switch
            {
                > 0 => CreatedAtAction(nameof(CriarAluno), new { id = aluno.Id }, aluno),
                -1 => BadRequest("Usuário já em uso."),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Erro ao inserir o aluno.")
            };
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RetornoTodosAlunosDto))]
        public async Task<IActionResult> ListarAlunos([FromQuery] int pagina = 1, [FromQuery] int registrosPorPagina = 10)
        {
            return Ok(await _alunoService.ListarTodosAlunos(pagina,   registrosPorPagina));
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RetornoAlunoDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListarAlunoPorUsuario([FromQuery] string usuario)
        {
            var aluno = await _alunoService.ListarAlunoPorUsuario(usuario);

            return aluno != null ? Ok(aluno) : NotFound();
        }

        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CriacaoAtualizacaoAlunoDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public async Task<IActionResult> AtualizarAluno(CriacaoAtualizacaoAlunoDto model, [FromQuery] string usuario)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

                return BadRequest(erros);
            }

            var aluno = await _alunoService.AtualizarAluno(model, usuario);

            return aluno switch
            {
                > 0 => Ok(model),
                -1 => BadRequest("Usuário já em uso."),
                -2 => BadRequest("Usuário da rota ''/usuario='' não existente."),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar o aluno.")
            };
        }

        [HttpDelete]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public async Task<IActionResult> RemoverAluno([FromQuery] string usuario)
        {
            var aluno = await _alunoService.RemoverAluno(usuario);

            return aluno switch
            {
                0 => StatusCode(StatusCodes.Status500InternalServerError, new List<string> { "Houve um erro ao remover o aluno." }),
                -1 => BadRequest(new List<string> { "Usuário não existente." }),
                _ => Ok()
            };
        }
    }
}
