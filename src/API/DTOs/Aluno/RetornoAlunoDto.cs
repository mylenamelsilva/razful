using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Aluno
{
    public class RetornoAlunoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }

    }
}
