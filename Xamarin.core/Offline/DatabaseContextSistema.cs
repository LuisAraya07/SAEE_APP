using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.EntityFrameworkCore;
using Xamarin.core.Models;
using Environment = System.Environment;

namespace Xamarin.core.Offline
{
    public partial class DatabaseContextSistema : DbContext
    {
        public virtual DbSet<Grupos> Grupos { get; set; }
        public virtual DbSet<Estudiantes> Estudiantes { get; set; }
        public virtual DbSet<Profesores> Profesores { get; set; }
        public virtual DbSet<Cursos> Cursos { get; set; }
        public virtual DbSet<CursosGrupos> CursosGrupos { get; set; }
        public virtual DbSet<EstudiantesXgrupos> EG { get; set; }
        public virtual DbSet<Asignaciones> Asignaciones{ get; set; }
    //    public virtual DbSet<Evaluaciones> Evaluaciones { get; set; }

       // public DbSet<Asignaciones> Asignaciones{ get; set; }
        public DbSet<Evaluaciones> Evaluaciones { get; set; }
        private string _DatabasePath { get; set; } = "dbSistema.db";


        public DatabaseContextSistema()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), _DatabasePath);
            optionsBuilder.UseSqlite($"Data Source={_DatabasePath}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cursos>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.IdProfesorNavigation)
                    .WithMany(p => p.Cursos)
                    .HasForeignKey(d => d.IdProfesor)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Cursos__IdProfes__07C12930");
            });

            modelBuilder.Entity<CursosGrupos>(entity =>
            {
                entity.HasOne(d => d.IdCursoNavigation)
                    .WithMany(p => p.CursosGrupos)
                    .HasForeignKey(d => d.IdCurso)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK__CursosGru__IdCur__0A9D95DB");

                entity.HasOne(d => d.IdGrupoNavigation)
                    .WithMany(p => p.CursosGrupos)
                    .HasForeignKey(d => d.IdGrupo)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK__CursosGru__IdGru__0B91BA14");
            });
            modelBuilder.Entity<Estudiantes>(entity =>
            {
                entity.Property(e => e.Cedula)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Pin)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.PrimerApellido)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SegundoApellido)
                    //.IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.IdProfesorNavigation)
                    .WithMany(p => p.Estudiantes)
                    .HasForeignKey(d => d.IdProfesor)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK__Estudiant__IdPro__73BA3083");
            });

            modelBuilder.Entity<EstudiantesXgrupos>(entity =>
            {
                entity.ToTable("EstudiantesXGrupos");

                entity.HasOne(d => d.IdEstudianteNavigation)
                    .WithMany(p => p.EstudiantesXgrupos)
                    .HasForeignKey(d => d.IdEstudiante)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK__Estudiant__IdEst__787EE5A0");

                entity.HasOne(d => d.IdGrupoNavigation)
                    .WithMany(p => p.EstudiantesXgrupos)
                    .HasForeignKey(d => d.IdGrupo)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK__Estudiant__IdGru__778AC167");
            });

            modelBuilder.Entity<Grupos>(entity =>
            {
                entity.Property(e => e.Grupo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.IdProfesorNavigation)
                    .WithMany(p => p.Grupos)
                    .HasForeignKey(d => d.IdProfesor)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Grupos__IdProfes__70DDC3D8");
            });
            modelBuilder.Entity<Profesores>(entity =>
            {
                entity.Property(e => e.Cedula)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Contrasenia)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Correo).HasMaxLength(100);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PrimerApellido)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SegundoApellido)
                    .IsRequired()
                    .HasMaxLength(100);
            });
            modelBuilder.Entity<Asignaciones>(entity =>
            {
                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Profesor)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Curso)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Grupo)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Fecha)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Puntos)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Porcentaje)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}