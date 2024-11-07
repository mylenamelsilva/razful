namespace API.Repositories.Aluno
{
    public class AlunoRepositoryQueries
    {
        public static string AdicionarAluno
           = $@"
                INSERT INTO [dbo].[Aluno]
                   (Nome
                   ,Usuario
                   ,Senha)
                VALUES (
                    @NOME
                    ,@USUARIO
                    ,@SENHA
                )

                SELECT SCOPE_IDENTITY()
            ";

        private static string _ListarAlunos
            = $@"
                SELECT
                    Id
                    ,Nome
                    ,Usuario
                    ,Senha
                FROM [dbo].[Aluno]
            ";

        public static string ListarAlunosPorPagina
            = $@"
                {_ListarAlunos}
                ORDER BY Id DESC 
                OFFSET (@REGISTROS * (@PAGINA - 1)) ROWS FETCH NEXT @REGISTROS ROWS ONLY
            ";        
        
        public static string ListarAlunoPorUsuario
            = $@"
                {_ListarAlunos}
                WHERE Usuario = @USUARIO
            ";

        public static string ContagemAlunos
            = $@"
                SELECT
                    COUNT(*)
                FROM [dbo].[Aluno]
            ";

        public static string AtualizarAluno
            = $@"
                UPDATE [dbo].[Aluno]
                  SET Nome = @NOME
                   ,Usuario = @USUARIO
                   ,Senha = @SENHA
                WHERE Usuario = @USUARIO_INICIAL
            ";

        public static string RemoverAluno
            = $@"
                DELETE FROM [dbo].[Aluno] WHERE Usuario = @USUARIO
            ";
    }
}
