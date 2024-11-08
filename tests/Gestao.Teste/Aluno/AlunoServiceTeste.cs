using API.DTOs.Aluno;
using API.Repositories.Aluno;
using API.Services.Aluno;
using API.Services.Seguranca;
using Moq;

namespace Gestao.Teste.Aluno
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
        public async Task Teste_SenhaDeveSerHash_Retornar60Caracteres()
        {
            var aluno = new CriacaoAtualizacaoAlunoDto()
            {
                Nome = "Mylena",
                Usuario = "mylenamel",
                Senha = "Mylena@12#"
            };

            _segurancaServiceMock.Setup(s => s.ConverterStringEmHash(aluno.Senha))
                             .ReturnsAsync((string senha) => BCrypt.Net.BCrypt.HashPassword(senha));

            _alunoRepositoryMock.Setup(r => r.CriarAluno(It.IsAny<CriacaoAtualizacaoAlunoDto>()))
                            .ReturnsAsync((CriacaoAtualizacaoAlunoDto model) => new RetornoAlunoDto
                            {
                                Id = 1,
                                Nome = model.Nome,
                                Usuario = model.Usuario,
                                Senha = model.Senha
                            });

            var alunoRetornado = await _alunoService.CriarAluno(aluno);

            Assert.Equal(60, alunoRetornado.Senha.Length);
        }

        [Fact]
        public async Task Teste_NaoPodeCadastrarUsuarioQueJaExiste_RetornarIdNegativo()
        {
            var aluno = new CriacaoAtualizacaoAlunoDto()
            {
                Nome = "Mylena",
                Usuario = "mylenamel",
                Senha = "Mylena@12#"
            };


            _alunoRepositoryMock.Setup(r => r.CriarAluno(aluno))
                            .ReturnsAsync(new RetornoAlunoDto { Id = -1 });

            var resultado = await _alunoService.CriarAluno(aluno);

            Assert.Equal(-1, resultado.Id);
        }

        [Fact]
        public async Task Teste_NaoPodeAtualizarDeUsuarioQueNaoExiste_RetornarIdNegativo()
        {
            var aluno = new CriacaoAtualizacaoAlunoDto()
            {
                Nome = "Mylena",
                Usuario = "mylenamel",
                Senha = "Mylena@12#"
            };

            var usuarioOriginal = "jojo";

            _alunoRepositoryMock.Setup(r => r.AtualizarAluno(aluno, usuarioOriginal))
                                        .ReturnsAsync(-2);

            var resultado = await _alunoService.AtualizarAluno(aluno, usuarioOriginal);

            Assert.Equal(-2, resultado);
        }
    }
}