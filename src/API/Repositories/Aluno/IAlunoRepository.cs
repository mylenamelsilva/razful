using API.DTOs.Aluno;

namespace API.Repositories.Aluno
{
    public interface IAlunoRepository
    {
        public RetornoAlunoDto CriarAluno(CriacaoAtualizacaoAlunoDto model);
        public RetornoTodosAlunosDto ListarTodosAlunos(int pagina, int registrosPorPagina);
        public RetornoAlunoDto? ListarAlunoPorUsuario(string usuario);
        public int AtualizarAluno(CriacaoAtualizacaoAlunoDto model, string usuario);
        public int RemoverAluno(string usuario);
    }
}
