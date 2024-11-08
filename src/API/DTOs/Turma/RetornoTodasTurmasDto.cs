namespace API.DTOs.Turma
{
    public class RetornoTodasTurmasDto
    {
        public int Pagina { get; set; }
        public int TotalParaPaginacao { get; set; }
        public List<RetornoTurmaDto> Turmas { get; set; } = [];
    }
}
