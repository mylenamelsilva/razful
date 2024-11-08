using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace API.DTOs.Aluno
{
    public class CriacaoAtualizacaoAlunoDto : IValidatableObject
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [MaxLength(255, ErrorMessage = "O campo Nome deve ter no máximo 255 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo Usuario é obrigatório.")]
        [MaxLength(45, ErrorMessage = "O campo Usuario deve ter no máximo 45 caracteres.")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [MinLength(8, ErrorMessage = "O campo Senha deve ter no mínimo 8 caracteres.")]
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
