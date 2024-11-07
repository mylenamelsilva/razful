using API.DTOs.Aluno;
using API.Repositories._Base;
using Dapper;
using System.Data;

namespace API.Repositories.Aluno
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly GestaoDbContext _context;

        public AlunoRepository(GestaoDbContext context)
        {
            _context = context;
        }

        public int AtualizarAluno(CriacaoAtualizacaoAlunoDto model, string usuario)
        {
            var query = AlunoRepositoryQueries.AtualizarAluno;
            using var _conexao = _context.ConexaoQuery();

            var (existeAluno, usuarioRotaValida) = VerificarSeJaExisteUsuario(_conexao, model.Usuario, usuario, "atualizar");


            if (usuarioRotaValida != null && usuarioRotaValida is false)
            {
                return -2;
            }

            if (existeAluno)
            {
                return -1;
            }

            using var _transacao = _conexao.BeginTransaction();

            var filtros = new Dictionary<string, object>()
            {
                { "NOME", model.Nome },
                { "USUARIO", model.Usuario },
                { "SENHA", model.Senha },
                { "USUARIO_INICIAL", usuario },
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

        public RetornoAlunoDto CriarAluno(CriacaoAtualizacaoAlunoDto model)
        {
            var query = AlunoRepositoryQueries.AdicionarAluno;
            using var _conexao = _context.ConexaoQuery();

            var (existeAluno, _) = VerificarSeJaExisteUsuario(_conexao, model.Usuario, "", "criar");

            if (existeAluno)
            {
                return new RetornoAlunoDto()
                {
                    Id = -1
                };
            }

            using var _transacao = _conexao.BeginTransaction();

            var filtros = new Dictionary<string, object>()
            {
                { "NOME", model.Nome },
                { "USUARIO", model.Usuario },
                { "SENHA", model.Senha }
            };

            var idAluno = _conexao.ExecuteScalar<int>(query, filtros, _transacao);

            if (idAluno <= 0)
            {
                _transacao.Rollback();
                return new RetornoAlunoDto();
            }

            _transacao.Commit();

            return new RetornoAlunoDto()
            {
                Id = idAluno,
                Nome = model.Nome,
                Usuario = model.Usuario,
                Senha = model.Senha,
            };
        }

        public RetornoAlunoDto? ListarAlunoPorUsuario(string usuario)
        {
            var query = AlunoRepositoryQueries.ListarAlunoPorUsuario;
            using var _conexao = _context.ConexaoQuery();

            var aluno = _conexao.Query<RetornoAlunoDto>(query, new
            {
                USUARIO = usuario
            }).FirstOrDefault();

            return aluno;
        }

        public RetornoTodosAlunosDto ListarTodosAlunos(int pagina, int registrosPorPagina)
        {
            var query = AlunoRepositoryQueries.ListarAlunosPorPagina;
            using var _conexao = _context.ConexaoQuery();

            var filtros = new Dictionary<string, object>()
            {
                { "REGISTROS", registrosPorPagina},
                { "PAGINA", pagina }
            };

            var listaAlunos = _conexao.Query<RetornoAlunoDto>(query, filtros).ToList();

            return new RetornoTodosAlunosDto()
            {
                Pagina = pagina,
                TotalParaPaginacao = ContagemTotalAlunos(registrosPorPagina),
                Alunos = listaAlunos
            };
        }

        public int RemoverAluno(string usuario)
        {
            var query = AlunoRepositoryQueries.RemoverAluno;
            using var _conexao = _context.ConexaoQuery();

            var (_, usuarioRotaValida) = VerificarSeJaExisteUsuario(_conexao, "", usuario, "atualizar");

            if (usuarioRotaValida != null && usuarioRotaValida is false)
            {
                return -1;
            }

            using var _transacao = _conexao.BeginTransaction();

            var alunoRemovido = _conexao.Execute(query, new { USUARIO = usuario}, _transacao);

            if (alunoRemovido == 0)
            {
                _transacao.Rollback();
                return alunoRemovido;
            }

            _transacao.Commit();
            return alunoRemovido;
        }

        private int ContagemTotalAlunos(int registrosPorPagina)
        {
            var query = AlunoRepositoryQueries.ContagemAlunos;
            using var _conexao = _context.ConexaoQuery();

            var totalRegistros = _conexao.ExecuteScalar<int>(query);
            var totalDePaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

            return totalDePaginas;
        }

        private (bool, bool?) VerificarSeJaExisteUsuario(IDbConnection conexao, string usuarioAAtualizar, string usuarioRota, string operacao)
        {
            var queryExisteAluno = AlunoRepositoryQueries.ListarAlunoPorUsuario;

            var existeUsuario = conexao.Query<RetornoAlunoDto>(queryExisteAluno, new
            {
                USUARIO = usuarioAAtualizar
            }).FirstOrDefault();

            if (operacao == "atualizar")
            {
                var usuarioValido = conexao.Query<RetornoAlunoDto>(queryExisteAluno, new
                {
                    USUARIO = usuarioRota
                }).FirstOrDefault();

                if (existeUsuario?.Usuario == usuarioValido?.Usuario)
                {
                    existeUsuario = null;
                }

                return (existeUsuario != null, usuarioValido != null);
            }

            return (existeUsuario != null, null);
        }
    }
}
