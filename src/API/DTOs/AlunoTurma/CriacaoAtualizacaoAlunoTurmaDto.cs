using System.ComponentModel.DataAnnotations;

namespace API.DTOs.AlunoTurma
{
    public class CriacaoAtualizacaoAlunoTurmaDto
    {
        [Required]
        public string Aluno { get; set; }
        [Required]
        public string Turma { get; set; }
    }
}
