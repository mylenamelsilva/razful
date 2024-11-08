using API.DTOs.Aluno;
using API.Repositories.Aluno;
using API.Services.Seguranca;

namespace API.Services.Aluno
{
    public class AlunoService : IAlunoService
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly ISegurancaService _segurancaService;

        public AlunoService(IAlunoRepository alunoRepository, ISegurancaService segurancaService)
        {
            _alunoRepository = alunoRepository;
            _segurancaService = segurancaService;
        }

        public async Task<int> AtualizarAluno(CriacaoAtualizacaoAlunoDto model, string usuario)
        {

            model.Senha = model.Senha.Length != 60 ? await _segurancaService.ConverterStringEmHash(model.Senha) : model.Senha;
            var alunoAtualizado = await _alunoRepository.AtualizarAluno(model, usuario);
            return alunoAtualizado;
        }

        public async Task<RetornoAlunoDto> CriarAluno(CriacaoAtualizacaoAlunoDto model)
        {
            model.Senha = await _segurancaService.ConverterStringEmHash(model.Senha);
            var aluno = await _alunoRepository.CriarAluno(model);

            return aluno;
        }

        public async Task<RetornoAlunoDto?> ListarAlunoPorUsuario(string usuario)
        {
            var aluno = await _alunoRepository.ListarAlunoPorUsuario(usuario);
            return aluno;
        }

        public async Task<RetornoTodosAlunosDto> ListarTodosAlunos(int pagina, int registrosPorPagina)
        {
            var listaAlunos = await _alunoRepository.ListarTodosAlunos(pagina, registrosPorPagina);
            return listaAlunos;
        }

        public async Task<int> RemoverAluno(string usuario)
        {
            var alunoRemovido = await _alunoRepository.RemoverAluno(usuario);
            return alunoRemovido;
        }
    }
}
