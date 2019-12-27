using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Xamarin.core.Models
{
    public class Grupos
    {
        public Grupos()
        {
            CursosGrupos = new HashSet<CursosGrupos>();
            EstudiantesXgrupos = new HashSet<EstudiantesXgrupos>();
        }

        public int Id { get; set; }
        public int IdProfesor { get; set; }
        public string Grupo { get; set; }
        public int Anio { get; set; }

        public virtual Profesores IdProfesorNavigation { get; set; }
        public virtual ICollection<CursosGrupos> CursosGrupos { get; set; }
        public virtual ICollection<EstudiantesXgrupos> EstudiantesXgrupos { get; set; }
    }
}