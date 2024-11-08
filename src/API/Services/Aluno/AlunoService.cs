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

        public int AtualizarAluno(CriacaoAtualizacaoAlunoDto model, string usuario)
        {

            model.Senha = model.Senha.Length != 60 ? _segurancaService.ConverterStringEmHash(model.Senha) : model.Senha;
            var alunoAtualizado = _alunoRepository.AtualizarAluno(model, usuario);
            return alunoAtualizado;
        }

        public RetornoAlunoDto CriarAluno(CriacaoAtualizacaoAlunoDto model)
        {
            model.Senha = _segurancaService.ConverterStringEmHash(model.Senha);
            var aluno = _alunoRepository.CriarAluno(model);

            return aluno;
        }

        public RetornoAlunoDto? ListarAlunoPorUsuario(string usuario)
        {
            var aluno = _alunoRepository.ListarAlunoPorUsuario(usuario);
            return aluno;
        }

        public RetornoTodosAlunosDto ListarTodosAlunos(int pagina, int registrosPorPagina)
        {
            var listaAlunos = _alunoRepository.ListarTodosAlunos(pagina, registrosPorPagina);
            return listaAlunos;
        }

        public int RemoverAluno(string usuario)
        {
            var alunoRemovido = _alunoRepository.RemoverAluno(usuario);
            return alunoRemovido;
        }
    }
}
