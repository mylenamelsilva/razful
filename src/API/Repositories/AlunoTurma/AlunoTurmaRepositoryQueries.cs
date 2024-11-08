namespace API.Repositories.AlunoTurma
{
    public class AlunoTurmaRepositoryQueries
    {
        public static string CriarAssociacao
            = $@"
                INSERT INTO [dbo].[Aluno_Turma]
                (
                    Aluno_Id
                    ,Turma_Id
                )
                VALUES(
                    (SELECT Id FROM [dbo].[Aluno] WHERE Usuario = @USUARIO),
                    (SELECT Id FROM [dbo].[Turma] WHERE Turma = @TURMA)
                )
            ";

        public static string AtualizarAssociacao
            = $@"
                INSERT INTO [dbo].[Aluno_Turma]
                (
                    Aluno_Id
                    ,Turma_Id
                )
                VALUES(
                    (SELECT Id FROM [dbo].[Aluno] WHERE Usuario = @USUARIO),
                    (SELECT Id FROM [dbo].[Turma] WHERE Turma = @TURMA)
                )
            ";

        public static string ListarAssociacoesPorPagina
            = $@"
                 SELECT
                     TURMA.Turma 
	                 ,STRING_AGG(ALUNO.Nome, ', ') AS Alunos
                     ,TURMA.Curso_Id AS CursoId
                 FROM [dbo].[Aluno_Turma] ALUNO_TURMA
                     INNER JOIN [dbo].[Aluno] ALUNO ON ALUNO.Id = ALUNO_TURMA.Aluno_Id
                     INNER JOIN [dbo].[Turma] TURMA ON TURMA.Id = ALUNO_TURMA.Turma_Id
                 GROUP BY TURMA.Turma,
		                  TURMA.Curso_Id
                 ORDER BY 1 DESC 
                 OFFSET (@REGISTROS * (@PAGINA - 1)) ROWS FETCH NEXT @REGISTROS ROWS ONLY
            ";


        public static string ListarAlunosPorTurma
            = $@"
                SELECT
                    ALUNO.Id
                    ,ALUNO.Nome
	                ,ALUNO.Usuario
                 FROM [dbo].[Aluno_Turma] ALUNO_TURMA
                     INNER JOIN [dbo].[Aluno] ALUNO ON ALUNO.Id = ALUNO_TURMA.Aluno_Id
                     INNER JOIN [dbo].[Turma] TURMA ON TURMA.Id = ALUNO_TURMA.Turma_Id
                WHERE ALUNO_TURMA.Turma_Id = (SELECT Id FROM [dbo].[Turma] WHERE Turma = @TURMA)
            ";


        public static string _ContagemAssociacoes
            = $@"
                 SELECT TOP 1
	                 COUNT(*) OVER() AS TOTAL
                 FROM [dbo].[Aluno_Turma] ALUNO_TURMA
            ";

        public static string ContagemAlunosAssociados
            = $@"
                {_ContagemAssociacoes}
                WHERE ALUNO_TURMA.Turma_Id = (SELECT Id FROM [dbo].[Turma] WHERE Turma = @TURMA)
            ";

        public static string ContagemTodasAssociacoes
           = $@"
                {_ContagemAssociacoes}
                GROUP BY ALUNO_TURMA.Turma_Id
            ";

        public static string _RemoverAssociacao
            = $@"
                DELETE FROM [dbo].[Aluno_Turma] 
            ";

        public static string RemoverAlunoAssociado
            = $@"
                {_RemoverAssociacao}
                WHERE Aluno_Id = (SELECT Id FROM [dbo].[Aluno] WHERE Usuario = @ALUNO)
                    AND Turma_Id = (SELECT Id FROM [dbo].[Turma] WHERE Turma = @TURMA)
            ";

        public static string RemoverTodaAssociacao
            = $@"
                {_RemoverAssociacao}
                WHERE Turma_Id = (SELECT Id FROM [dbo].[Turma] WHERE Turma = @TURMA)
            ";

    }
}
