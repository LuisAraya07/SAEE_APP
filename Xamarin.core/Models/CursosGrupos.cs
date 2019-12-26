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
    public class CursosGrupos
    {

        public int Id { get; set; }
        public int IdCurso { get; set; }
        public int IdGrupo { get; set; }

        public virtual Cursos IdCursoNavigation { get; set; }
        public virtual Grupos IdGrupoNavigation { get; set; }
    }
}