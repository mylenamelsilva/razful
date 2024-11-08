namespace API.DTOs.AlunoTurma
{
    public class RetornoTodasAssociacoesDto
    {
        public int Pagina { get; set; }
        public int TotalParaPaginacao { get; set; }
        public List<AssociacoesAgrupadasDto> Associacoes { get; set; } = [];
    }

    public class AssociacoesAgrupadasDto
    {
        public string Alunos { get; set; }
        public int CursoId { get; set; }
        public string Turma { get; set; }
    }
}
