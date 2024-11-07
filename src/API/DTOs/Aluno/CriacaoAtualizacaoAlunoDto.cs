using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace API.DTOs.Aluno
{
    public class CriacaoAtualizacaoAlunoDto : IValidatableObject
    {
        [Required]
        [MaxLength(255)]
        public string Nome { get; set; }
        [Required]
        [MaxLength(45)]
        public string Usuario { get; set; }
        [Required]
        [MinLength(8)]
        public string Senha { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SenhaValida(Senha))
            {
                yield return new ValidationResult("A senha precisa conter, no mínimo, 8 caracteres com uma letra maiúscula e um caractere especial", new[] { nameof(Senha) });
            }
        }

        private bool SenhaValida(string senha)
        {
            string regexPattern = "^(?=.*[A-Z])(?=.*[\\W_]).{8,}$";

            var senhaValida = Regex.IsMatch(senha, regexPattern);

            return senhaValida;
        }
    }
}
