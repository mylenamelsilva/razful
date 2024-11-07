using API.DTOs.Aluno;

namespace API.Services.Aluno
{
    public interface IAlunoService
    {
        public RetornoAlunoDto CriarAluno(CriacaoAtualizacaoAlunoDto model);
        public int AtualizarAluno(CriacaoAtualizacaoAlunoDto model, string usuario);
        public RetornoAlunoDto? ListarAlunoPorUsuario(string usuario);
        public RetornoTodosAlunosDto ListarTodosAlunos(int pagina, int registrosPorPagina);
        public int RemoverAluno(string usuario);
    }
}
