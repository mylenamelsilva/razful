using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Turma
{
    public class CriacaoAtualizacaoTurmaDto
    {
        public int CursoId { get; set; }
        [RegularExpression(@"^\d{4}$", ErrorMessage = "O ano deve ter 4 dígitos.")]
        public int Ano { get; set; }
        [MaxLength(45)]
        public string Turma { get; set; }
    }
}
