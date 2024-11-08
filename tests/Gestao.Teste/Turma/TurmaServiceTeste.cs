using API.DTOs.Turma;
using API.Repositories.Turma;
using API.Services.Turma;
using Moq;

namespace Gestao.Teste.Turma
{
    public class TurmaServiceTeste
    {
        private readonly ITurmaService _TurmaService;
        private readonly Mock<ITurmaRepository> _TurmaRepositoryMock;

        public TurmaServiceTeste()
        {
            _TurmaRepositoryMock = new Mock<ITurmaRepository>();
            _TurmaService = new TurmaService(_TurmaRepositoryMock.Object);
        }

        [Fact]
        public async Task Teste_NaoPodeCadastrarMesmaTurma_DeveRetornarIdNegativo()
        {
            var turma = new CriacaoAtualizacaoTurmaDto()
            {
                Turma = "Turma1",
                Ano = 2024,
                CursoId = 2
            };

            _TurmaRepositoryMock.Setup(r => r.CriarTurma(turma))
                                        .ReturnsAsync(new RetornoTurmaDto { Id = -1 });

            var resultado = await _TurmaService.CriarTurma(turma);

            Assert.Equal(-1, resultado.Id);
        }
    }
}
