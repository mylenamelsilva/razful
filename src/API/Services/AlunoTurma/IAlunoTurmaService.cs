using API.DTOs.Aluno;
using API.DTOs.AlunoTurma;

namespace API.Services.AlunoTurma
{
    public interface IAlunoTurmaService
    {
        public Task<RetornoAlunoTurmaDto> CriarAssociacao(CriacaoAtualizacaoAlunoTurmaDto model);
        public Task<int> AtualizarAssociacao(CriacaoAtualizacaoAlunoTurmaDto model, string turma);
        public Task<int> RemoverAssociacao(string aluno, string turma);
        public Task<int> RemoverTodaAssociacao(string turma);
        public Task<RetornoTodasAssociacoesDto> ListarAssociacoes(int pagina, int registrosPorPagina);
        public Task<RetornoTodosAlunosDto> ListarAlunosPorTurma(int pagina, int registrosPorPagina, string turma);
    }
}
