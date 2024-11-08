using API.DTOs.Aluno;
using API.DTOs.AlunoTurma;
using API.Services.AlunoTurma;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AlunoTurmasController : ControllerBase
    {
        private readonly IAlunoTurmaService _alunoTurmaService;

        public AlunoTurmasController(IAlunoTurmaService alunoTurmaService)
        {
            _alunoTurmaService = alunoTurmaService;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RetornoAlunoTurmaDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public async Task<IActionResult> CriarAssociacao(CriacaoAtualizacaoAlunoTurmaDto model)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

                return BadRequest(erros);
            }

            var turma = await _alunoTurmaService.CriarAssociacao(model);

            return turma.Sucesso switch
            {
                0 => StatusCode(StatusCodes.Status500InternalServerError, new List<string> { "Houve um erro ao inserir a associação." }),
                _ => CreatedAtAction(nameof(CriarAssociacao), new { id = turma.Sucesso }, turma)
            };
        }

        //[HttpPut]
        //public async Task<IActionResult> AlterarAssociacao(CriacaoAtualizacaoAlunoTurmaDto model)
        //{

        //}

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RetornoTodasAssociacoesDto))]
        public async Task<IActionResult> ListarAssociacoes([FromQuery] int pagina = 1, [FromQuery] int registrosPorPagina = 10)
        {
            return Ok(await _alunoTurmaService.ListarAssociacoes(pagina, registrosPorPagina));
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RetornoTodosAlunosDto))]
        public async Task<IActionResult> ListarAlunosPorTurma([FromQuery] string turma, [FromQuery] int pagina = 1, [FromQuery] int registrosPorPagina = 10)
        {
            return Ok(await _alunoTurmaService.ListarAlunosPorTurma(pagina, registrosPorPagina, turma));
        }

        [HttpDelete]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public async Task<IActionResult> RemoverAssociacao([FromQuery] string aluno, [FromQuery] string turma)
        {
            var associacao = await _alunoTurmaService.RemoverAssociacao(aluno, turma);

            return associacao switch
            {
                0 => StatusCode(StatusCodes.Status500InternalServerError, new List<string> { "Houve um erro ao remover a associação." }),
                -1 => BadRequest(new List<string> { "Usuário/Turma não existente." }),
                _ => Ok()
            };
        }

        [HttpDelete]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public async Task<IActionResult> RemoverTodaAssociacao([FromQuery] string turma)
        {
            var associacao = await _alunoTurmaService.RemoverTodaAssociacao(turma);

            return associacao switch
            {
                0 => StatusCode(StatusCodes.Status500InternalServerError, new List<string> { "Houve um erro ao remover a associação." }),
                -1 => BadRequest(new List<string> { "Turma não existente." }),
                _ => Ok()
            };
        }
    }
}
