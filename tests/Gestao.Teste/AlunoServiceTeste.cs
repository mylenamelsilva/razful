using API.DTOs.Aluno;
using API.Repositories.Aluno;
using API.Services.Aluno;
using API.Services.Seguranca;
using Moq;

namespace Gestao.Teste
{
    public class AlunoServiceTeste
    {
        private readonly IAlunoService _alunoService;
        private readonly Mock<ISegurancaService> _segurancaServiceMock;
        private readonly Mock<IAlunoRepository> _alunoRepositoryMock;

        public AlunoServiceTeste()
        {
            _segurancaServiceMock = new Mock<ISegurancaService>();
            _alunoRepositoryMock = new Mock<IAlunoRepository>();
            _alunoService = new AlunoService(_alunoRepositoryMock.Object, _segurancaServiceMock.Object);
        }

        [Fact]
        public void Teste_SenhaDeveSerHash_Retornar60Caracteres()
        {
            var aluno = new CriacaoAtualizacaoAlunoDto()
            {
                Nome = "Mylena",
                Usuario = "mylenamel",
                Senha = "Mylena@12#"
            };

            _segurancaServiceMock.Setup(s => s.ConverterStringEmHash(aluno.Senha))
                             .Returns((string senha) => BCrypt.Net.BCrypt.HashPassword(senha));

            _alunoRepositoryMock.Setup(r => r.CriarAluno(It.IsAny<CriacaoAtualizacaoAlunoDto>()))
                            .Returns((CriacaoAtualizacaoAlunoDto model) => new RetornoAlunoDto
                            {
                                Id = 1,
                                Nome = model.Nome,
                                Usuario = model.Usuario,
                                Senha = model.Senha
                            });

            var alunoRetornado = _alunoService.CriarAluno(aluno);

            Assert.Equal(60, alunoRetornado.Senha.Length);
        }

        [Fact]
        public void Teste_NaoPodeCadastrarUsuarioQueJaExiste_RetornarIdNegativo()
        {
            var aluno = new CriacaoAtualizacaoAlunoDto()
            {
                Nome = "Mylena",
                Usuario = "mylenamel",
                Senha = "Mylena@12#"
            };

            _alunoRepositoryMock.Setup(r => r.ListarAlunoPorUsuario(aluno.Usuario))
                    .Returns(new RetornoAlunoDto { Usuario = aluno.Usuario });

            var resultado = _alunoService.CriarAluno(aluno);

            Assert.Equal(-1, resultado.Id);
        }

        [Fact]
        public void Teste_NaoPodeAtualizarDeUsuarioQueNaoExiste_RetornarIdNegativo()
        {
            var aluno = new CriacaoAtualizacaoAlunoDto()
            {
                Nome = "Mylena",
                Usuario = "mylenamel",
                Senha = "Mylena@12#"
            };

            var usuarioOriginal = "jojo";

            _alunoRepositoryMock.Setup(r => r.ListarAlunoPorUsuario(usuarioOriginal))
                    .Returns(new RetornoAlunoDto());

            var resultado = _alunoService.AtualizarAluno(aluno, usuarioOriginal);

            Assert.Equal(-1, resultado);
        }
    }
}