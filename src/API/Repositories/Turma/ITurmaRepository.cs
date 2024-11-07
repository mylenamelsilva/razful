using API.DTOs.Turma;

namespace API.Repositories.Turma
{
    public interface ITurmaRepository
    {
        public RetornoTurmaDto CriarTurma(CriacaoAtualizacaoTurmaDto model);
        public RetornoTodasTurmasDto ListarTodasTurmas(int pagina, int registrosPorPagina);
        public RetornoTurmaDto? ListarTurmaPorId(int idTurma);
        public int AtualizarTurma(CriacaoAtualizacaoTurmaDto model, int idTurma);
        public int RemoverTurma(int idTurma);
    }
}
