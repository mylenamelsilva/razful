namespace API.DTOs.Aluno
{
    public class RetornoTodosAlunosDto
    {
        public int Pagina { get; set; }
        public int TotalParaPaginacao { get; set; }
        public List<RetornoAlunoDto> Alunos { get; set; } = [];
    }
}
