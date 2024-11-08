using API.DTOs.Aluno;
using API.DTOs.AlunoTurma;
using API.Repositories.AlunoTurma;

namespace API.Services.AlunoTurma
{
    public class AlunoTurmaService : IAlunoTurmaService
    {
        private readonly IAlunoTurmaRepository _alunoTurmaRepository;

        public AlunoTurmaService(IAlunoTurmaRepository alunoTurmaRepository)
        {
            _alunoTurmaRepository = alunoTurmaRepository;
        }

        public async Task<int> AtualizarAssociacao(CriacaoAtualizacaoAlunoTurmaDto model, string turma)
        {
            return await _alunoTurmaRepository.AtualizarAssociacao(model, turma);
        }

        public async Task<RetornoAlunoTurmaDto> CriarAssociacao(CriacaoAtualizacaoAlunoTurmaDto model)
        {
            return await _alunoTurmaRepository.CriarAssociacao(model);
        }

        public async Task<RetornoTodosAlunosDto> ListarAlunosPorTurma(int pagina, int registrosPorPagina, string turma)
        {
            return await _alunoTurmaRepository.ListarAlunosPorTurma(pagina, registrosPorPagina, turma);
        }

        public async Task<RetornoTodasAssociacoesDto> ListarAssociacoes(int pagina, int registrosPorPagina)
        {
            return await _alunoTurmaRepository.ListarAssociacoes(pagina, registrosPorPagina);
        }

        public async Task<int> RemoverAssociacao(string aluno, string turma)
        {
            return await _alunoTurmaRepository.RemoverAssociacao(aluno, turma);
        }

        public async Task<int> RemoverTodaAssociacao(string turma)
        {
            return await _alunoTurmaRepository.RemoverTodaAssociacao(turma);
        }
    }
}
