using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Turma_Turma",
                table: "Turma",
                column: "Turma",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Aluno_Usuario",
                table: "Aluno",
                column: "Usuario",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Turma_Turma",
                table: "Turma");

            migrationBuilder.DropIndex(
                name: "IX_Aluno_Usuario",
                table: "Aluno");
        }
    }
}
