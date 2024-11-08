namespace Front.Models.Aluno
{
    public class AlunoRazorModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Erro { get; set; } = string.Empty;
    }
}
