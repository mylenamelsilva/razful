using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Repositories._Base
{
    public class GestaoDbContext : DbContext
    {
        public DbSet<AlunoModel> Aluno { get; set; }
        public DbSet<TurmaModel> Turma { get; set; }
        public DbSet<AlunoTurmaModel> Aluno_Turma { get; set; }

        private IConfiguration _configuration;

        public GestaoDbContext(IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DatabaseConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlunoTurmaModel>()
                       .HasKey(at => new { at.Aluno_Id, at.Turma_Id });

            modelBuilder.Entity<AlunoModel>()
                .HasMany(e => e.AlunoTurmas);

            modelBuilder.Entity<TurmaModel>()
               .HasMany(e => e.AlunoTurmas);
        }

        public IDbConnection ConexaoQuery()
        {
            var conexao = Database.GetDbConnection();

            if (conexao.State != ConnectionState.Open)
            {
                conexao.Open();
            }
            return conexao;
        }
    }
}
