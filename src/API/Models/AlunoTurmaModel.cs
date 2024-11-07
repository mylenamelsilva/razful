using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class AlunoTurmaModel
    {
        public int Aluno_Id { get; private set; }
        public int Turma_Id { get; private set; }

        [ForeignKey(nameof(Aluno_Id))]
        public virtual AlunoModel Aluno { get; set; } = null!;

        [ForeignKey(nameof(Turma_Id))]
        public virtual TurmaModel Turma { get; set; } = null!;
    }
}
