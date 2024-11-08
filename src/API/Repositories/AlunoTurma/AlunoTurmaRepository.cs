using API.DTOs.Aluno;
using API.DTOs.AlunoTurma;
using API.DTOs.Turma;
using API.Repositories._Base;
using API.Repositories.Aluno;
using Dapper;
using System.Data;
using System.Net.NetworkInformation;

namespace API.Repositories.AlunoTurma
{
    public class AlunoTurmaRepository : IAlunoTurmaRepository
    {
        private readonly GestaoDbContext _context;

        public AlunoTurmaRepository(GestaoDbContext context)
        {
            _context = context;
        }

        public async Task<int> AtualizarAssociacao(CriacaoAtualizacaoAlunoTurmaDto model, string turma)
        {
            using var _conexao = _context.ConexaoQuery();

            if (!await UsuarioValido(_conexao, model.Aluno) || model.Turma != turma)
            {
                return -1;
            }

            if (await UsuarioJaCadastradoNaTurma(_conexao, model.Aluno, model.Turma))
            {
                return -2;
            }

            var query = AlunoTurmaRepositoryQueries.CriarAssociacao;
            using var _transacao = _conexao.BeginTransaction();

            var linhasAfetadas = await _conexao.ExecuteAsync(query, new
            {
                USUARIO = model.Aluno,
                TURMA = model.Turma
            }, _transacao);

            if (linhasAfetadas == 0)
            {
                _transacao.Rollback();
                return linhasAfetadas;
            }

            _transacao.Commit();
            return linhasAfetadas;
        }

        public async Task<RetornoAlunoTurmaDto> CriarAssociacao(CriacaoAtualizacaoAlunoTurmaDto model)
        {
            using var _conexao = _context.ConexaoQuery();

            bool usuarioValido = await UsuarioValido(_conexao, model.Aluno);
            bool turmaValida = await TurmaValida(_conexao, model.Turma);

            if (!usuarioValido || !turmaValida)
            {
                return new RetornoAlunoTurmaDto()
                {
                    Sucesso = -1
                };
            }

            if (await UsuarioJaCadastradoNaTurma(_conexao, model.Aluno, model.Turma))
            {
                return new RetornoAlunoTurmaDto()
                {
                    Sucesso = -2
                };
            }

            using var _transacao = _conexao.BeginTransaction();
            var query = AlunoTurmaRepositoryQueries.CriarAssociacao;

            var linhasAfetadas = await _conexao.ExecuteAsync(query, new
            {
                USUARIO = model.Aluno,
                TURMA = model.Turma
            }, _transacao);

            if (linhasAfetadas == 0)
            {
                _transacao.Rollback();
                return new RetornoAlunoTurmaDto();
            }

            _transacao.Commit();
            return new RetornoAlunoTurmaDto()
            {
                Sucesso = 1,
                Aluno = model.Aluno,
                Turma = model.Turma
            };
        }

        public async Task<RetornoTodosAlunosDto> ListarAlunosPorTurma(int pagina, int registrosPorPagina, string turma)
        {
            using var _conexao = _context.ConexaoQuery();
            var query = AlunoTurmaRepositoryQueries.ListarAlunosPorTurma;

            var listaAlunosAssociados = (await _conexao.QueryAsync<RetornoAlunoDto>(query, new
            {
                PAGINA = pagina,
                REGISTROS = registrosPorPagina,
                TURMA = turma
            })).ToList();

            return new RetornoTodosAlunosDto()
            {
                Pagina = pagina,
                TotalParaPaginacao = await ContagemTotalAlunosAssociados(registrosPorPagina, turma),
                Alunos = listaAlunosAssociados
            };
        }

        public async Task<RetornoTodasAssociacoesDto> ListarAssociacoes(int pagina, int registrosPorPagina)
        {
            using var _conexao = _context.ConexaoQuery();
            var query = AlunoTurmaRepositoryQueries.ListarAssociacoesPorPagina;

            var listaAssociacoes = (await _conexao.QueryAsync<AssociacoesAgrupadasDto>(query, new
            {
                PAGINA = pagina,
                REGISTROS = registrosPorPagina
            })).ToList();

            return new RetornoTodasAssociacoesDto()
            {
                Pagina = pagina,
                TotalParaPaginacao = await ContagemTotalAssociacoes(registrosPorPagina),
                Associacoes = listaAssociacoes
            };
        }

        public async Task<int> RemoverAssociacao(string aluno, string turma)
        {
            using var _conexao = _context.ConexaoQuery();

            bool usuarioValido = await UsuarioValido(_conexao, aluno);
            bool turmaValida = await TurmaValida(_conexao, turma);

            if (!usuarioValido || !turmaValida)
            {
                return -1;
            }

            var query = AlunoTurmaRepositoryQueries.RemoverAlunoAssociado;
            using var _transacao = _conexao.BeginTransaction();

            var linhaAfetada = await _conexao.ExecuteAsync(query, new
            {
                TURMA = turma,
                ALUNO = aluno
            }, _transacao);

            if (linhaAfetada == 0)
            {
                _transacao.Rollback();
                return linhaAfetada;
            }

            _transacao.Commit();
            return linhaAfetada;
        }

        public async Task<int> RemoverTodaAssociacao(string turma)
        {
            using var _conexao = _context.ConexaoQuery();

            bool turmaValida = await TurmaValida(_conexao, turma);

            if (!turmaValida)
            {
                return -1;
            }

            var query = AlunoTurmaRepositoryQueries.RemoverTodaAssociacao;
            using var _transacao = _conexao.BeginTransaction();

            var linhaAfetada = await _conexao.ExecuteAsync(query, new
            {
                TURMA = turma
            }, _transacao);

            if (linhaAfetada == 0)
            {
                _transacao.Rollback();
                return linhaAfetada;
            }

            _transacao.Commit();
            return linhaAfetada;
        }

        private async Task<int> ContagemTotalAssociacoes(int registrosPorPagina)
        {
            var query = AlunoTurmaRepositoryQueries.ContagemTodasAssociacoes;
            using var _conexao = _context.ConexaoQuery();

            var totalRegistros = await _conexao.ExecuteScalarAsync<int>(query);
            var totalDePaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

            return totalDePaginas;
        }

        private async Task<int> ContagemTotalAlunosAssociados(int registrosPorPagina, string turma)
        {
            var query = AlunoTurmaRepositoryQueries.ContagemAlunosAssociados;
            using var _conexao = _context.ConexaoQuery();

            var totalRegistros = await _conexao.ExecuteScalarAsync<int>(query, new { TURMA = turma });
            var totalDePaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

            return totalDePaginas;
        }

        private async Task<bool> UsuarioValido(IDbConnection conexao, string usuario)
        {
            var query = AlunoRepositoryQueries.ListarAlunoPorUsuario;
            var usuarioValido = await conexao.QuerySingleOrDefaultAsync<RetornoTurmaDto>(query, new { USUARIO = usuario }) != null;
            return usuarioValido;
        }

        private async Task<bool> TurmaValida(IDbConnection conexao, string turma)
        {
            var query = TurmaRepositoryQueries.ListarTurmaPorTurma;
            var usuarioValido = await conexao.QuerySingleOrDefaultAsync<RetornoTurmaDto>(query, new { TURMA = turma }) != null;
            return usuarioValido;
        }

        private async Task<bool> UsuarioJaCadastradoNaTurma(IDbConnection conexao, string usuario, string turma)
        {
            var query = AlunoTurmaRepositoryQueries.ExisteUsuarioCadastradoNaTurma;
            var usuarioTurmaValido = await conexao.QuerySingleOrDefaultAsync<RetornoTurmaDto>(query, new { TURMA = turma, ALUNO = usuario }) != null;
            return usuarioTurmaValido;
        }
    }
}
