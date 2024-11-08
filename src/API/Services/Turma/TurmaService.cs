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

        public int AtualizarTurma(CriacaoAtualizacaoTurmaDto model, int idTurma)
        {
            var turmaAtualizada = _turmaRepository.AtualizarTurma(model, idTurma);
            return turmaAtualizada;
        }

        public RetornoTurmaDto CriarTurma(CriacaoAtualizacaoTurmaDto model)
        {
            var turma = _turmaRepository.CriarTurma(model);
            return turma;
        }

        public RetornoTodasTurmasDto ListarTodasTurmas(int pagina, int registrosPorPagina)
        {
            var listaAlunos = _turmaRepository.ListarTodasTurmas(pagina, registrosPorPagina);
            return listaAlunos;
        }

        public RetornoTurmaDto? ListarTurmaPorId(int idTurma)
        {
            var aluno = _turmaRepository.ListarTurmaPorId(idTurma);
            return aluno;
        }

        public int RemoverTurma(int idTurma)
        {
            var turmaRemovida = _turmaRepository.RemoverTurma(idTurma);
            return turmaRemovida;
        }
    }
}
