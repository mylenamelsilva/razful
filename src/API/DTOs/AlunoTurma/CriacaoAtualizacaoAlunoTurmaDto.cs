using System.ComponentModel.DataAnnotations;

namespace API.DTOs.AlunoTurma
{
    public class CriacaoAtualizacaoAlunoTurmaDto
    {
        [Required(ErrorMessage = "O campo Aluno é obrigatório.")]
        public string Aluno { get; set; }
        [Required(ErrorMessage = "O campo Turma é obrigatório.")]
        public string Turma { get; set; }
    }
}
