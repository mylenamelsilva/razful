using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class AlunoModel
    {
        public int Id { get; private set; }
        [Column(TypeName = "VARCHAR(255)")]
        public string Nome { get; private set; }
        [Column(TypeName = "VARCHAR(45)")]
        public string Usuario { get; private set; }
        [Column(TypeName = "CHAR(60)")]
        public string Senha { get; private set; }

        public virtual List<AlunoTurmaModel> AlunoTurmas { get; } = new();
    }
}
