namespace Front.Models.AlunoTurma
{
    public class AlunoTurmaViewModel
    {
        public int Sucesso { get; set; }
        public string Aluno { get; set; }
        public string Turma { get; set; }
        public string Erro { get; set; } = string.Empty;
    }
}
