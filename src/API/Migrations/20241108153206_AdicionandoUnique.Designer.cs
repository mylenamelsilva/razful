﻿// <auto-generated />
using API.Repositories._Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(GestaoDbContext))]
    [Migration("20241108153206_AdicionandoUnique")]
    partial class AdicionandoUnique
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API.Models.AlunoModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("VARCHAR(255)");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("CHAR(60)");

                    b.Property<string>("Usuario")
                        .IsRequired()
                        .HasColumnType("VARCHAR(45)");

                    b.HasKey("Id");

                    b.HasIndex("Usuario")
                        .IsUnique();

                    b.ToTable("Aluno");
                });

            modelBuilder.Entity("API.Models.AlunoTurmaModel", b =>
                {
                    b.Property<int>("Aluno_Id")
                        .HasColumnType("int");

                    b.Property<int>("Turma_Id")
                        .HasColumnType("int");

                    b.HasKey("Aluno_Id", "Turma_Id");

                    b.HasIndex("Turma_Id");

                    b.ToTable("Aluno_Turma");
                });

            modelBuilder.Entity("API.Models.TurmaModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Ano")
                        .HasMaxLength(4)
                        .HasColumnType("int");

                    b.Property<int>("Curso_Id")
                        .HasColumnType("int");

                    b.Property<string>("Turma")
                        .IsRequired()
                        .HasColumnType("VARCHAR(45)");

                    b.HasKey("Id");

                    b.HasIndex("Turma")
                        .IsUnique();

                    b.ToTable("Turma");
                });

            modelBuilder.Entity("API.Models.AlunoTurmaModel", b =>
                {
                    b.HasOne("API.Models.AlunoModel", "Aluno")
                        .WithMany("AlunoTurmas")
                        .HasForeignKey("Aluno_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Models.TurmaModel", "Turma")
                        .WithMany("AlunoTurmas")
                        .HasForeignKey("Turma_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aluno");

                    b.Navigation("Turma");
                });

            modelBuilder.Entity("API.Models.AlunoModel", b =>
                {
                    b.Navigation("AlunoTurmas");
                });

            modelBuilder.Entity("API.Models.TurmaModel", b =>
                {
                    b.Navigation("AlunoTurmas");
                });
#pragma warning restore 612, 618
        }
    }
}
