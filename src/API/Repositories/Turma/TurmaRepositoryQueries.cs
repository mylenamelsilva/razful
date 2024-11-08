namespace API.Repositories.Aluno
{
    public class TurmaRepositoryQueries
    {
        public static string AdicionarTurma
           = $@"
                INSERT INTO [dbo].[Turma]
                   (Turma
                   ,Ano
                   ,Curso_Id)
                VALUES (
                    @TURMA
                    ,@ANO
                    ,@CURSO
                )

                SELECT SCOPE_IDENTITY()
            ";

        private static string _ListarTurmas
            = $@"
                SELECT
                    Id
                    ,Turma
                    ,Ano
                    ,Curso_Id AS CursoId
                FROM [dbo].[Turma]
            ";

        public static string ListarTurmasPorPagina
            = $@"
                {_ListarTurmas}
                ORDER BY Id DESC 
                OFFSET (@REGISTROS * (@PAGINA - 1)) ROWS FETCH NEXT @REGISTROS ROWS ONLY
            ";

        public static string ListarTurmaPorId
            = $@"
                {_ListarTurmas}
                WHERE ID = @ID
            ";

        public static string ListarTurmaPorTurma
            = $@"
                {_ListarTurmas}
                WHERE Turma = @TURMA
            ";

        public static string ContagemTurmas
            = $@"
                SELECT
                    COUNT(*)
                FROM [dbo].[Turma]
            ";

        public static string AtualizarTurma
            = $@"
                UPDATE [dbo].[Turma]
                  SET Turma = @TURMA
                   ,Curso_Id = @CURSO
                   ,Ano = @ANO
                WHERE Id = @ID
            ";

        public static string RemoverTurma
            = $@"
                DELETE FROM [dbo].[Turma] WHERE Id = @ID
            ";
    }
}
