using API.DTOs.Turma;

namespace API.Services.Turma
{
    public interface ITurmaService
    {
        public RetornoTurmaDto CriarTurma(CriacaoAtualizacaoTurmaDto model);
        public int AtualizarTurma(CriacaoAtualizacaoTurmaDto model, int idTurma);
        public RetornoTurmaDto? ListarTurmaPorId(int idTurma);
        public RetornoTodasTurmasDto ListarTodasTurmas(int pagina, int registrosPorPagina);
        public int RemoverTurma(int idTurma);
    }
}
