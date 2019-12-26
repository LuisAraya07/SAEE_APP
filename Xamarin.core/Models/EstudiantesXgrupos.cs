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
    public partial class EstudiantesXgrupos
    {
        public int Id { get; set; }
        public int IdProfesor { get; set; }
        public int IdGrupo { get; set; }
        public int IdEstudiante { get; set; }

        public virtual Estudiantes IdEstudianteNavigation { get; set; }
        public virtual Grupos IdGrupoNavigation { get; set; }
        public virtual Profesores IdProfesorNavigation { get; set; }
    }
}