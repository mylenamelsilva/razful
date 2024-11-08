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

        public async Task<int> AtualizarAluno(CriacaoAtualizacaoAlunoDto model, string usuario)
        {
            using var _conexao = _context.ConexaoQuery();

            if (!await UsuarioValido(_conexao, usuario))
            {
                return -2;
            }

            if (await UsuarioJaExiste(_conexao, model.Usuario) && model.Usuario != usuario)
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

            var linhaAfetada = await _conexao.ExecuteAsync(query, filtros, _transacao);

            if (linhaAfetada == 0)
            {
                _transacao.Rollback();
                return linhaAfetada;
            }

            _transacao.Commit();
            return linhaAfetada;
        }

        public async Task<RetornoAlunoDto> CriarAluno(CriacaoAtualizacaoAlunoDto model)
        {
            using var _conexao = _context.ConexaoQuery();

            if (await UsuarioJaExiste(_conexao, model.Usuario))
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

            var idAluno = await _conexao.ExecuteScalarAsync<int>(query, filtros, _transacao);

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

        public async Task<RetornoAlunoDto?> ListarAlunoPorUsuario(string usuario)
        {
            var query = AlunoRepositoryQueries.ListarAlunoPorUsuario;
            using var _conexao = _context.ConexaoQuery();

            var aluno = await _conexao.QuerySingleOrDefaultAsync<RetornoAlunoDto>(query, new
            {
                USUARIO = usuario
            });

            return aluno;
        }

        public async Task<RetornoTodosAlunosDto> ListarTodosAlunos(int pagina, int registrosPorPagina)
        {
            var query = AlunoRepositoryQueries.ListarAlunosPorPagina;
            using var _conexao = _context.ConexaoQuery();

            var filtros = new Dictionary<string, object>()
            {
                { "REGISTROS", registrosPorPagina},
                { "PAGINA", pagina }
            };

            var listaAlunos = (await _conexao.QueryAsync<RetornoAlunoDto>(query, filtros)).ToList();

            return new RetornoTodosAlunosDto()
            {
                Pagina = pagina,
                TotalParaPaginacao = await ContagemTotalAlunos(registrosPorPagina),
                Alunos = listaAlunos
            };
        }

        public async Task<int> RemoverAluno(string usuario)
        {
            using var _conexao = _context.ConexaoQuery();

            if (!await UsuarioValido(_conexao, usuario))
            {
                return -1;
            }

            var query = AlunoRepositoryQueries.RemoverAluno;
            using var _transacao = _conexao.BeginTransaction();

            var linhaAfetada = await _conexao.ExecuteAsync(query, new { USUARIO = usuario}, _transacao);

            if (linhaAfetada == 0)
            {
                _transacao.Rollback();
                return linhaAfetada;
            }

            _transacao.Commit();
            return linhaAfetada;
        }

        private async Task<int> ContagemTotalAlunos(int registrosPorPagina)
        {
            var query = AlunoRepositoryQueries.ContagemAlunos;
            using var _conexao = _context.ConexaoQuery();

            var totalRegistros = await _conexao.ExecuteScalarAsync<int>(query);
            var totalDePaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

            return totalDePaginas;
        }

        private async Task<bool> UsuarioJaExiste(IDbConnection conexao, string usuario)
        {
            var query = AlunoRepositoryQueries.ListarAlunoPorUsuario;
            var existeUsuario = await conexao.QuerySingleOrDefaultAsync<RetornoTurmaDto>(query, new { USUARIO = usuario }) != null;
            return existeUsuario;
        }

        private async Task<bool> UsuarioValido(IDbConnection conexao, string usuario)
        {
            var query = AlunoRepositoryQueries.ListarAlunoPorUsuario;
            var usuarioValido = await conexao.QuerySingleOrDefaultAsync<RetornoTurmaDto>(query, new { USUARIO = usuario }) != null;
            return usuarioValido;
        }
    }
}
