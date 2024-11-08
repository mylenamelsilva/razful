using API.Controllers;
using API.DTOs.AlunoTurma;
using API.Services.AlunoTurma;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Gestao.Teste.AlunoTurma
{
    public class AlunoTurmaControllerTeste
    {
        private readonly AlunoTurmasController _alunoTurmaController;
        private readonly Mock<IAlunoTurmaService> _alunoTurmaServiceMock;

        public AlunoTurmaControllerTeste()
        {
            _alunoTurmaServiceMock = new Mock<IAlunoTurmaService>();
            _alunoTurmaController = new AlunoTurmasController(_alunoTurmaServiceMock.Object);
        }

        [Fact]
        public async Task Teste_AlunoJaCadastradoNaTurma_DeveRetornarBadRequest()
        {
            var model = new CriacaoAtualizacaoAlunoTurmaDto
            {
                Aluno = "aluno1",
                Turma = "turma1"
            };

            _alunoTurmaServiceMock.Setup(s => s.CriarAssociacao(It.IsAny<CriacaoAtualizacaoAlunoTurmaDto>()))
                                  .ReturnsAsync(new RetornoAlunoTurmaDto { Sucesso = -2 });

            var result = await _alunoTurmaController.CriarAssociacao(model);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Aluno já cadastrado na turma.", badRequestResult.Value);
        }
    }
}
