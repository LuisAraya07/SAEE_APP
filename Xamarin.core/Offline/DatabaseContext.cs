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
    public class DatabaseContext : DbContext
    {

        public DbSet<Notificaciones> Notificaciones { get; set; }

        private string _DatabasePath { get; set; }


        public DatabaseContext(string DatabasPath)
        {
            _DatabasePath = DatabasPath;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), _DatabasePath);
            optionsBuilder.UseSqlite($"Data Source={_DatabasePath}");
        }
    }
}