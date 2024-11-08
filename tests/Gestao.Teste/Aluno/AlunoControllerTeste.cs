using API.Controllers;
using API.DTOs.Aluno;
using API.Services.Aluno;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Gestao.Teste.Aluno
{
    public class AlunoControllerTeste
    {
        private readonly AlunosController _alunoController;
        private readonly Mock<IAlunoService> _alunoServiceMock;

        public AlunoControllerTeste()
        {
            _alunoServiceMock = new Mock<IAlunoService>();
            _alunoController = new AlunosController(_alunoServiceMock.Object);
        }

        [Fact]
        public async Task Teste_SenhaInvalida_DeveDarErro()
        {
            var aluno = new CriacaoAtualizacaoAlunoDto()
            {
                Nome = "Mylena",
                Usuario = "mylenamel",
                Senha = "Mylena2"
            };
            var mensagemErro = "A senha precisa conter, no mínimo, 8 caracteres com uma letra maiúscula e um caractere especial";
            _alunoController.ModelState.AddModelError("Senha", mensagemErro);

            var resultado = await _alunoController.CriarAluno(aluno);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado);

            var erros = Assert.IsType<List<string>>(badRequestResult.Value);

            Assert.Contains(mensagemErro, erros);
        }

        [Fact]
        public async Task Teste_SenhaValida_DevePassar()
        {
            var aluno = new CriacaoAtualizacaoAlunoDto()
            {
                Nome = "Mylena",
                Usuario = "mylenamel",
                Senha = "Mylena!#42"
            };
            var mensagemErro = "A senha precisa conter, no mínimo, 8 caracteres com uma letra maiúscula e um caractere especial";
            _alunoServiceMock.Setup(s => s.CriarAluno(It.IsAny<CriacaoAtualizacaoAlunoDto>()))
                                    .ReturnsAsync(new RetornoAlunoDto { Id = 1 });

            var resultado = await _alunoController.CriarAluno(aluno);

            var okObjectResult = Assert.IsType<CreatedAtActionResult>(resultado);

            var erros = okObjectResult.Value as List<string> ?? new List<string>();

            Assert.DoesNotContain(mensagemErro, erros);
        }

        [Fact]
        public async Task Teste_NaoPodeCadastrarUsuarioQueJaExiste_RetornarMensagemErro()
        {
            var aluno = new CriacaoAtualizacaoAlunoDto()
            {
                Nome = "Mylena",
                Usuario = "mylenamel",
                Senha = "Mylena@12#"
            };

            _alunoServiceMock.Setup(s => s.CriarAluno(It.IsAny<CriacaoAtualizacaoAlunoDto>()))
                             .ReturnsAsync(new RetornoAlunoDto { Id = -1 });

            string mensagemErro = "Usuário já em uso.";

            var resultado = await _alunoController.CriarAluno(aluno);

            var objectResult = Assert.IsType<BadRequestObjectResult>(resultado);
            Assert.Equal(400, objectResult.StatusCode);
            var erros = Assert.IsType<string>(objectResult.Value);
            Assert.Contains(mensagemErro, erros);
        }

        [Fact]
        public async Task Teste_NaoPodeAtualizarDeUsuarioQueNaoExiste_RetornarMensagemErro()
        {
            var aluno = new CriacaoAtualizacaoAlunoDto()
            {
                Nome = "Mylena",
                Usuario = "mylenamel",
                Senha = "Mylena@12#"
            };

            var usuarioOriginal = "jojo";

            _alunoServiceMock.Setup(s => s.AtualizarAluno(It.IsAny<CriacaoAtualizacaoAlunoDto>(), usuarioOriginal))
                             .ReturnsAsync(-2);

            string mensagemErro = "Usuário da rota ''/usuario='' não existente.";

            var resultado = await _alunoController.AtualizarAluno(aluno, usuarioOriginal);

            var objectResult = Assert.IsType<BadRequestObjectResult>(resultado);
            Assert.Equal(400, objectResult.StatusCode);
            var erros = Assert.IsType<string>(objectResult.Value);
            Assert.Contains(mensagemErro, erros);
        }
    }
}
