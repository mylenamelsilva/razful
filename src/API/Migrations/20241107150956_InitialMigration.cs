using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aluno",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    Usuario = table.Column<string>(type: "VARCHAR(45)", nullable: false),
                    Senha = table.Column<string>(type: "CHAR(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aluno", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Turma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Curso_Id = table.Column<int>(type: "int", nullable: false),
                    Ano = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    Turma = table.Column<string>(type: "VARCHAR(45)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turma", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Aluno_Turma",
                columns: table => new
                {
                    Aluno_Id = table.Column<int>(type: "int", nullable: false),
                    Turma_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aluno_Turma", x => new { x.Aluno_Id, x.Turma_Id });
                    table.ForeignKey(
                        name: "FK_Aluno_Turma_Aluno_Aluno_Id",
                        column: x => x.Aluno_Id,
                        principalTable: "Aluno",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Aluno_Turma_Turma_Turma_Id",
                        column: x => x.Turma_Id,
                        principalTable: "Turma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aluno_Turma_Turma_Id",
                table: "Aluno_Turma",
                column: "Turma_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aluno_Turma");

            migrationBuilder.DropTable(
                name: "Aluno");

            migrationBuilder.DropTable(
                name: "Turma");
        }
    }
}
