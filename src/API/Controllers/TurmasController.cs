using API.DTOs.Aluno;
using API.DTOs.Turma;
using API.Services.Turma;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TurmasController : ControllerBase
    {
        private readonly ITurmaService _turmaService;

        public TurmasController(ITurmaService turmaService)
        {
            _turmaService = turmaService;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RetornoTurmaDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public IActionResult CriarTurma(CriacaoAtualizacaoTurmaDto model)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

                return BadRequest(erros);
            }

            var turma = _turmaService.CriarTurma(model);

            return turma.Id switch
            {
                0 => StatusCode(StatusCodes.Status500InternalServerError, new List<string> { "Houve um erro ao inserir a turma." }),
                -1 => BadRequest(new List<string> { "Turma já em uso." }),
                _ => CreatedAtAction(nameof(CriarTurma), new { id = turma.Id }, turma)
            };
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RetornoTodasTurmasDto))]
        public IActionResult ListarTurmas([FromQuery] int pagina = 1, [FromQuery] int registrosPorPagina = 10)
        {
            return Ok(_turmaService.ListarTodasTurmas(pagina, registrosPorPagina));
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RetornoTurmaDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ListarTurmaPorId([FromQuery] int idTurma)
        {
            if (idTurma <= 0)
            {
                return BadRequest(new List<string> { "ID de turma inválido." });
            }

            var turmaRetorno = _turmaService.ListarTurmaPorId(idTurma);

            return turmaRetorno != null ? Ok(turmaRetorno) : NotFound();
        }

        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CriacaoAtualizacaoTurmaDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public IActionResult AtualizarTurma(CriacaoAtualizacaoTurmaDto model, [FromQuery] int idTurma)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

                return BadRequest(erros);
            }

            if (idTurma <= 0)
            {
                return BadRequest(new List<string> { "ID de turma inválido." });
            }

            var turmaRetorno = _turmaService.AtualizarTurma(model, idTurma);

            return turmaRetorno switch
            {
                0 => StatusCode(StatusCodes.Status500InternalServerError, new List<string> { "Houve um erro ao atualizar a turma." }),
                -1 => BadRequest(new List<string> { "Turma já em uso." }),
                -2 => BadRequest(new List<string> { "Turma da rota '/turma' não existente." }),
                _ => Ok(model)
            };
        }

        [HttpDelete]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public IActionResult RemoverTurma([FromQuery] int idTurma)
        {
            var turmaRetorno = _turmaService.RemoverTurma(idTurma);

            return turmaRetorno switch
            {
                0 => StatusCode(StatusCodes.Status500InternalServerError, new List<string> { "Houve um erro ao remover a turma." }),
                -1 => BadRequest(new List<string> { "Turma não existente." }),
                _ => Ok()
            };
        }
    }
}
