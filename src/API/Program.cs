
using API.Repositories._Base;
using API.Repositories.Aluno;
using API.Repositories.AlunoTurma;
using API.Repositories.Turma;
using API.Services.Aluno;
using API.Services.AlunoTurma;
using API.Services.Seguranca;
using API.Services.Turma;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<GestaoDbContext>();
            builder.Services.AddScoped<IAlunoService, AlunoService>();
            builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
            builder.Services.AddScoped<ISegurancaService, SegurancaService>();
            builder.Services.AddScoped<ITurmaRepository, TurmaRepository>();
            builder.Services.AddScoped<ITurmaService, TurmaService>();
            builder.Services.AddScoped<IAlunoTurmaService, AlunoTurmaService>();
            builder.Services.AddScoped<IAlunoTurmaRepository, AlunoTurmaRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
