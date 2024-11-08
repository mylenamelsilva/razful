using API.DTOs.Aluno;

namespace API.Services.Aluno
{
    public interface IAlunoService
    {
        public Task<RetornoAlunoDto> CriarAluno(CriacaoAtualizacaoAlunoDto model);
        public Task<int> AtualizarAluno(CriacaoAtualizacaoAlunoDto model, string usuario);
        public Task <RetornoAlunoDto?> ListarAlunoPorUsuario(string usuario);
        public Task<RetornoTodosAlunosDto> ListarTodosAlunos(int pagina, int registrosPorPagina);
        public Task<int> RemoverAluno(string usuario);
    }
}
