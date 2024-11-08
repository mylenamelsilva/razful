using API.DTOs.Aluno;
using API.DTOs.Turma;
using API.Repositories.Turma;

namespace API.Services.Turma
{
    public class TurmaService : ITurmaService
    {
        private readonly ITurmaRepository _turmaRepository;

        public TurmaService(ITurmaRepository turmaRepository)
        {
            _turmaRepository = turmaRepository;
        }

        public async Task<int> AtualizarTurma(CriacaoAtualizacaoTurmaDto model, int idTurma)
        {
            var turmaAtualizada = await _turmaRepository.AtualizarTurma(model, idTurma);
            return turmaAtualizada;
        }

        public async Task<RetornoTurmaDto> CriarTurma(CriacaoAtualizacaoTurmaDto model)
        {
            var turma = await _turmaRepository.CriarTurma(model);
            return turma;
        }

        public async Task<RetornoTodasTurmasDto> ListarTodasTurmas(int pagina, int registrosPorPagina)
        {
            var listaAlunos = await _turmaRepository.ListarTodasTurmas(pagina, registrosPorPagina);
            return listaAlunos;
        }

        public async Task<RetornoTurmaDto?> ListarTurmaPorId(int idTurma)
        {
            var aluno = await _turmaRepository.ListarTurmaPorId(idTurma);
            return aluno;
        }

        public async Task<int> RemoverTurma(int idTurma)
        {
            var turmaRemovida = await _turmaRepository.RemoverTurma(idTurma);
            return turmaRemovida;
        }
    }
}
