using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Turma
{
    public class CriacaoAtualizacaoTurmaDto
    {
        public int CursoId { get; set; }
        [RegularExpression(@"^\d{4}$", ErrorMessage = "O ano deve ter 4 dígitos.")]
        public int Ano { get; set; }
        [MaxLength(45, ErrorMessage = "O campo Turma deve ter no máximo 45 caracteres.")]
        public string Turma { get; set; }
    }
}
