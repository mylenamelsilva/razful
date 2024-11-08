using API.DTOs.Aluno;

namespace Front.Models.AlunoTurma
{
    public class AlunosAssociadosViewModel
    {
        public string Turma { get; set; }
        public int Pagina { get; set; }
        public int TotalParaPaginacao { get; set; }
        public List<RetornoAlunoDto> Alunos { get; set; } = [];
    }
}
