using Microsoft.AspNetCore.Mvc.Rendering;

namespace Front.Models.AlunoTurma
{
    public class DropdownAlunoTurmaViewModel
    {
        public string Aluno { get; set; }
        public string Turma { get; set; }
        public IEnumerable<SelectListItem> Alunos { get; set; } = [];
        public IEnumerable<SelectListItem> Turmas { get; set; } = [];
    }
}
