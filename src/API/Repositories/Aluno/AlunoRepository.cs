using API.DTOs.Aluno;
using API.DTOs.Turma;
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
            using var _conexao = _context.ConexaoQuery();

            if (!UsuarioValido(_conexao, usuario))
            {
                return -2;
            }

            if (UsuarioJaExiste(_conexao, model.Usuario) && model.Usuario != usuario)
            {
                return -1;
            }

            var query = AlunoRepositoryQueries.AtualizarAluno;
            using var _transacao = _conexao.BeginTransaction();

            var filtros = new Dictionary<string, object>()
            {
                { "NOME", model.Nome },
                { "USUARIO", model.Usuario },
                { "SENHA", model.Senha },
                { "USUARIO_INICIAL", usuario },
            };

            var linhaAfetada = _conexao.Execute(query, filtros, _transacao);

            if (linhaAfetada == 0)
            {
                _transacao.Rollback();
                return linhaAfetada;
            }

            _transacao.Commit();
            return linhaAfetada;
        }

        public RetornoAlunoDto CriarAluno(CriacaoAtualizacaoAlunoDto model)
        {
            using var _conexao = _context.ConexaoQuery();

            if (UsuarioJaExiste(_conexao, model.Usuario))
            {
                return new RetornoAlunoDto { Id = -1 };
            }

            var query = AlunoRepositoryQueries.AdicionarAluno;
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

            var aluno = _conexao.QuerySingleOrDefault<RetornoAlunoDto>(query, new
            {
                USUARIO = usuario
            });

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
            using var _conexao = _context.ConexaoQuery();

            if (!UsuarioValido(_conexao, usuario))
            {
                return -1;
            }

            var query = AlunoRepositoryQueries.RemoverAluno;
            using var _transacao = _conexao.BeginTransaction();

            var linhaAfetada = _conexao.Execute(query, new { USUARIO = usuario}, _transacao);

            if (linhaAfetada == 0)
            {
                _transacao.Rollback();
                return linhaAfetada;
            }

            _transacao.Commit();
            return linhaAfetada;
        }

        private int ContagemTotalAlunos(int registrosPorPagina)
        {
            var query = AlunoRepositoryQueries.ContagemAlunos;
            using var _conexao = _context.ConexaoQuery();

            var totalRegistros = _conexao.ExecuteScalar<int>(query);
            var totalDePaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

            return totalDePaginas;
        }

        private bool UsuarioJaExiste(IDbConnection conexao, string usuario)
        {
            var query = AlunoRepositoryQueries.ListarAlunoPorUsuario;
            var existeUsuario = conexao.QuerySingleOrDefault<RetornoTurmaDto>(query, new { USUARIO = usuario }) != null;
            return existeUsuario;
        }

        private bool UsuarioValido(IDbConnection conexao, string usuario)
        {
            var query = AlunoRepositoryQueries.ListarAlunoPorUsuario;
            var usuarioValido = conexao.QuerySingleOrDefault<RetornoTurmaDto>(query, new { USUARIO = usuario }) != null;
            return usuarioValido;
        }
    }
}
