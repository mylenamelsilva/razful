using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class TurmaModel
    {
        public int Id { get; private set; }
        public int Curso_Id { get; private set; }
        [MaxLength(4)]
        public int Ano { get; private set; }
        [Column(TypeName = "VARCHAR(45)")]
        public string Turma { get; private set; }

        public virtual List<AlunoTurmaModel> AlunoTurmas { get; } = new();
    }
}
