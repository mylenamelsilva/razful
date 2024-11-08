using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Turma
{
    public class RetornoTurmaDto
    {
        public int Id { get; set; }
        public int CursoId { get; set; }
        public int Ano { get; set; }
        public string Turma { get; set; }
    }
}
