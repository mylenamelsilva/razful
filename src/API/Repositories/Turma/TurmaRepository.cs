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
            var query = TurmaRepositoryQueries.AtualizarTurma;
            using var _conexao = _context.ConexaoQuery();

            var (existeTurma, turmaRotaValida) = VerificarSeJaExisteTurma(_conexao, model.Turma, idTurma, "atualizar");

            if (existeTurma)
            {
                return -1;
            }

            if (turmaRotaValida != null && turmaRotaValida is false)
            {
                return -2;
            }

            using var _transacao = _conexao.BeginTransaction();

            var filtros = new Dictionary<string, object>()
            {
                { "TURMA", model.Turma },
                { "ANO", model.Ano },
                { "CURSO", model.CursoId },
                { "ID", idTurma },
            };

            var alteracaoRealizada = _conexao.Execute(query, filtros, _transacao);

            if (alteracaoRealizada == 0)
            {
                _transacao.Rollback();
                return alteracaoRealizada;
            }

            _transacao.Commit();
            return alteracaoRealizada;
        }

        public RetornoTurmaDto CriarTurma(CriacaoAtualizacaoTurmaDto model)
        {
            var query = TurmaRepositoryQueries.AdicionarTurma;
            using var _conexao = _context.ConexaoQuery();

            var (existeTurma, _) = VerificarSeJaExisteTurma(_conexao, model.Turma, 0, "criar");

            if (existeTurma)
            {
                return new RetornoTurmaDto()
                {
                    Id = -1
                };
            }

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

            var turmaRetorno = _conexao.Query<RetornoTurmaDto>(query, new
            {
                ID = id
            }).FirstOrDefault();

            return turmaRetorno;
        }

        public int RemoverTurma(int idTurma)
        {
            var query = TurmaRepositoryQueries.RemoverTurma;
            using var _conexao = _context.ConexaoQuery();

            var (_, turmaRotaValida) = VerificarSeJaExisteTurma(_conexao, "", idTurma, "atualizar");

            if (turmaRotaValida != null && turmaRotaValida is false)
            {
                return -1;
            }

            using var _transacao = _conexao.BeginTransaction();

            var turmaRemovida = _conexao.Execute(query, new { ID = idTurma }, _transacao);

            if (turmaRemovida == 0)
            {
                _transacao.Rollback();
                return turmaRemovida;
            }

            _transacao.Commit();
            return turmaRemovida;
        }

        private int ContagemTotalTurmas(int registrosPorPagina)
        {
            var query = TurmaRepositoryQueries.ContagemTurmas;
            using var _conexao = _context.ConexaoQuery();

            var totalRegistros = _conexao.ExecuteScalar<int>(query);
            var totalDePaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

            return totalDePaginas;
        }

        private (bool, bool?) VerificarSeJaExisteTurma(IDbConnection conexao, string turmaAAtualizar, int idTurma, string operacao)
        {
            var queryExisteTurma = TurmaRepositoryQueries.ListarTurmaPorTurma;

            var existeTurma = conexao.Query<RetornoTurmaDto>(queryExisteTurma, new
            {
                TURMA = turmaAAtualizar
            }).FirstOrDefault();

            if (operacao == "atualizar")
            {
                var queryIdTurma = TurmaRepositoryQueries.ListarTurmaPorId;

                var idTurmaValida = conexao.Query<RetornoTurmaDto>(queryIdTurma, new
                {
                    ID = idTurma
                }).FirstOrDefault();

                return (existeTurma != null, idTurmaValida != null);
            }

            return (existeTurma != null, null);
        }
    }
}
