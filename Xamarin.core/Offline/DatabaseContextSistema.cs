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
    public class DatabaseContextSistema : DbContext
    {
        public DbSet<Grupos> Grupos { get; set; }
        public DbSet<Estudiantes> Estudiantes { get; set; }
        public DbSet<Profesores> Profesores { get; set; }
        public DbSet<Cursos> Cursos { get; set; }
        public DbSet<CursosGrupos> CursosGrupos { get; set; }
        public DbSet<EstudiantesXgrupos> EG { get; set; }

        public DbSet<Asignaciones> Asignaciones{ get; set; }

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
    }
}