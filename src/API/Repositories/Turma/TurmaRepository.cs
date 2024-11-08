using API.DTOs.Aluno;
using API.DTOs.Turma;
using API.Repositories._Base;
using API.Repositories.Aluno;
using Dapper;
using System.Data;

namespace API.Repositories.Turma
{
    public class TurmaRepository : ITurmaRepository
    {
        private readonly GestaoDbContext _context;

        public TurmaRepository(GestaoDbContext context)
        {
            _context = context;
        }

        public async Task<int> AtualizarTurma(CriacaoAtualizacaoTurmaDto model, int idTurma)
        {
            using var _conexao = _context.ConexaoQuery();

            if (!await TurmaIdValida(_conexao, idTurma))
            {
                return -2;
            }

            if (await TurmaJaExiste(_conexao, model.Turma, idTurma))
            {
                return -1;
            }

            var query = TurmaRepositoryQueries.AtualizarTurma;
            using var _transacao = _conexao.BeginTransaction();

            var filtros = new Dictionary<string, object>()
            {
                { "TURMA", model.Turma },
                { "ANO", model.Ano },
                { "CURSO", model.CursoId },
                { "ID", idTurma },
            };

            var linhasAfetadas = await _conexao.ExecuteAsync(query, filtros, _transacao);

            if (linhasAfetadas == 0)
            {
                _transacao.Rollback();
                return linhasAfetadas;
            }

            _transacao.Commit();
            return linhasAfetadas;
        }

        public async Task<RetornoTurmaDto> CriarTurma(CriacaoAtualizacaoTurmaDto model)
        {
            using var _conexao = _context.ConexaoQuery();

            if (await TurmaJaExiste(_conexao, model.Turma))
            {
                return new RetornoTurmaDto { Id = -1 };
            }

            var query = TurmaRepositoryQueries.AdicionarTurma;
            using var _transacao = _conexao.BeginTransaction();

            var filtros = new Dictionary<string, object>()
            {
                { "TURMA", model.Turma },
                { "ANO", model.Ano },
                { "CURSO", model.CursoId },
            };

            var idTurma = await _conexao.ExecuteScalarAsync<int>(query, filtros, _transacao);

            if (idTurma <= 0)
            {
                _transacao.Rollback();
                return new RetornoTurmaDto();
            }

            _transacao.Commit();

            return new RetornoTurmaDto()
            {
                Id = idTurma,
                Turma = model.Turma,
                Ano = model.Ano,
                CursoId = model.CursoId,
            };
        }

        public async Task<RetornoTodasTurmasDto> ListarTodasTurmas(int pagina, int registrosPorPagina)
        {
            var query = TurmaRepositoryQueries.ListarTurmasPorPagina;
            using var _conexao = _context.ConexaoQuery();

            var filtros = new Dictionary<string, object>()
            {
                { "REGISTROS", registrosPorPagina},
                { "PAGINA", pagina }
            };

            var listaTurmas = (await _conexao.QueryAsync<RetornoTurmaDto>(query, filtros)).ToList();

            return new RetornoTodasTurmasDto()
            {
                Pagina = pagina,
                TotalParaPaginacao = await ContagemTotalTurmas(registrosPorPagina),
                Turmas = listaTurmas
            };
        }

        public async Task<RetornoTurmaDto?> ListarTurmaPorId(int id)
        {
            var query = TurmaRepositoryQueries.ListarTurmaPorId;
            using var _conexao = _context.ConexaoQuery();

            var turmaRetorno = await _conexao.QuerySingleOrDefaultAsync<RetornoTurmaDto>(query, new
            {
                ID = id
            });

            return turmaRetorno;
        }

        public async Task<int> RemoverTurma(int idTurma)
        {
            using var _conexao = _context.ConexaoQuery();

            if (!await TurmaIdValida(_conexao, idTurma))
            {
                return -1;
            }

            var query = TurmaRepositoryQueries.RemoverTurma;
            using var _transacao = _conexao.BeginTransaction();

            var linhasAfetadas = await _conexao.ExecuteAsync(query, new { ID = idTurma }, _transacao);

            if (linhasAfetadas == 0)
            {
                _transacao.Rollback();
                return linhasAfetadas;
            }

            _transacao.Commit();
            return linhasAfetadas;
        }

        private async Task<int> ContagemTotalTurmas(int registrosPorPagina)
        {
            var query = TurmaRepositoryQueries.ContagemTurmas;
            using var _conexao = _context.ConexaoQuery();

            var totalRegistros = await _conexao.ExecuteScalarAsync<int>(query);
            var totalDePaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

            return totalDePaginas;
        }

        private async Task<bool> TurmaJaExiste(IDbConnection conexao, string turma, int idTurma = 0)
        {
            var query = TurmaRepositoryQueries.ListarTurmaPorTurma;

            Dictionary<string, object> filtros = new()
            {
                { "TURMA", turma }
            };

            if (idTurma > 0)
            {
                query += "AND Id != @ID";
                filtros.Add("ID", idTurma);
            }
            
            var existeTurma = await conexao.QuerySingleOrDefaultAsync<RetornoTurmaDto>(query, filtros) != null;
            return existeTurma;
        }

        private async Task<bool> TurmaIdValida(IDbConnection conexao, int idTurma)
        {
            var query = TurmaRepositoryQueries.ListarTurmaPorId;
            var turmaIdValida = await conexao.QuerySingleOrDefaultAsync<RetornoTurmaDto>(query, new { ID = idTurma }) != null;
            return turmaIdValida;
        }
    }
}
