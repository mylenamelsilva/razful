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

        public int AtualizarTurma(CriacaoAtualizacaoTurmaDto model, int idTurma)
        {
            using var _conexao = _context.ConexaoQuery();

            if (!TurmaIdValida(_conexao, idTurma))
            {
                return -2;
            }

            if (TurmaJaExiste(_conexao, model.Turma, idTurma))
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

            var linhasAfetadas = _conexao.Execute(query, filtros, _transacao);

            if (linhasAfetadas == 0)
            {
                _transacao.Rollback();
                return linhasAfetadas;
            }

            _transacao.Commit();
            return linhasAfetadas;
        }

        public RetornoTurmaDto CriarTurma(CriacaoAtualizacaoTurmaDto model)
        {
            using var _conexao = _context.ConexaoQuery();

            if (TurmaJaExiste(_conexao, model.Turma))
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

            var idTurma = _conexao.ExecuteScalar<int>(query, filtros, _transacao);

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

        public RetornoTodasTurmasDto ListarTodasTurmas(int pagina, int registrosPorPagina)
        {
            var query = TurmaRepositoryQueries.ListarTurmasPorPagina;
            using var _conexao = _context.ConexaoQuery();

            var filtros = new Dictionary<string, object>()
            {
                { "REGISTROS", registrosPorPagina},
                { "PAGINA", pagina }
            };

            var listaTurmas = _conexao.Query<RetornoTurmaDto>(query, filtros).ToList();

            return new RetornoTodasTurmasDto()
            {
                Pagina = pagina,
                TotalParaPaginacao = ContagemTotalTurmas(registrosPorPagina),
                Turmas = listaTurmas
            };
        }

        public RetornoTurmaDto? ListarTurmaPorId(int id)
        {
            var query = TurmaRepositoryQueries.ListarTurmaPorId;
            using var _conexao = _context.ConexaoQuery();

            var turmaRetorno = _conexao.QuerySingleOrDefault<RetornoTurmaDto>(query, new
            {
                ID = id
            });

            return turmaRetorno;
        }

        public int RemoverTurma(int idTurma)
        {
            using var _conexao = _context.ConexaoQuery();

            if (!TurmaIdValida(_conexao, idTurma))
            {
                return -1;
            }

            var query = TurmaRepositoryQueries.RemoverTurma;
            using var _transacao = _conexao.BeginTransaction();

            var linhasAfetadas = _conexao.Execute(query, new { ID = idTurma }, _transacao);

            if (linhasAfetadas == 0)
            {
                _transacao.Rollback();
                return linhasAfetadas;
            }

            _transacao.Commit();
            return linhasAfetadas;
        }

        private int ContagemTotalTurmas(int registrosPorPagina)
        {
            var query = TurmaRepositoryQueries.ContagemTurmas;
            using var _conexao = _context.ConexaoQuery();

            var totalRegistros = _conexao.ExecuteScalar<int>(query);
            var totalDePaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

            return totalDePaginas;
        }

        private bool TurmaJaExiste(IDbConnection conexao, string turma, int idTurma = 0)
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
            
            var existeTurma = conexao.QuerySingleOrDefault<RetornoTurmaDto>(query, filtros) != null;
            return existeTurma;
        }

        private bool TurmaIdValida(IDbConnection conexao, int idTurma)
        {
            var query = TurmaRepositoryQueries.ListarTurmaPorId;
            var turmaIdValida = conexao.QuerySingleOrDefault<RetornoTurmaDto>(query, new { ID = idTurma }) != null;
            return turmaIdValida;
        }
    }
}
