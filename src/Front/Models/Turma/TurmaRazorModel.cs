namespace Front.Models.Turma
{
    public class TurmaRazorModel
    {
        public int Id { get; set; }
        public int CursoId { get; set; }
        public int Ano { get; set; }
        public string Turma { get; set; }
        public string Erro { get; set; } = string.Empty;
    }
}
