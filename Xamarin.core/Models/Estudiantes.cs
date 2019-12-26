using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public partial class Estudiantes
    {
        public Estudiantes()
        {
            EstudiantesXgrupos = new HashSet<EstudiantesXgrupos>();
        }


        public int Id { get; set; }
        public int IdProfesor { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Pin { get; set; }

        public virtual Profesores IdProfesorNavigation { get; set; }
        public virtual ICollection<EstudiantesXgrupos> EstudiantesXgrupos { get; set; }
    }
}