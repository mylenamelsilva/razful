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

            if (turma.Id == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new List<string> { "Houve um erro ao inserir a turma." });
            }

            if (turma.Id == -1)
            {
                return BadRequest("Turma já em uso.");
            }

            return CreatedAtAction(nameof(CriarTurma), new { id = turma.Id }, turma);
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
            var turmaRetorno = _turmaService.ListarTurmaPorId(idTurma);

            if (turmaRetorno == null)
            {
                return NotFound();
            }

            return Ok(turmaRetorno);
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

            var turmaRetorno = _turmaService.AtualizarTurma(model, idTurma);

            if (turmaRetorno == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new List<string> { "Houve um erro ao atualizar a turma." });
            }

            if (turmaRetorno == -1)
            {
                return BadRequest("Turma já em uso.");
            }

            if (turmaRetorno == -2)
            {
                return BadRequest("Turma da rota ''/turma='' não existente.");
            }

            return Ok(model);
        }

        [HttpDelete]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public IActionResult RemoverTurma([FromQuery] int idTurma)
        {
            var turmaRetorno = _turmaService.RemoverTurma(idTurma);

            if (turmaRetorno == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new List<string> { "Houve um erro ao remover a turma." });
            }

            if (turmaRetorno == -1)
            {
                return BadRequest("Turma não existente.");
            }

            return Ok();
        }
    }
}
