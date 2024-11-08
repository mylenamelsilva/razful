using API.DTOs.Turma;

namespace API.Repositories.Turma
{
    public interface ITurmaRepository
    {
        public Task<RetornoTurmaDto> CriarTurma(CriacaoAtualizacaoTurmaDto model);
        public Task<int> AtualizarTurma(CriacaoAtualizacaoTurmaDto model, int idTurma);
        public Task<RetornoTurmaDto?> ListarTurmaPorId(int idTurma);
        public Task<RetornoTodasTurmasDto> ListarTodasTurmas(int pagina, int registrosPorPagina);
        public Task<int> RemoverTurma(int idTurma);
    }
}
