using API.DTOs.Turma;

namespace API.Services.Turma
{
    public interface ITurmaService
    {
        public Task<RetornoTurmaDto> CriarTurma(CriacaoAtualizacaoTurmaDto model);
        public Task<int> AtualizarTurma(CriacaoAtualizacaoTurmaDto model, int idTurma);
        public Task<RetornoTurmaDto?> ListarTurmaPorId(int idTurma);
        public Task<RetornoTodasTurmasDto> ListarTodasTurmas(int pagina, int registrosPorPagina);
        public Task<int> RemoverTurma(int idTurma);
    }
}
