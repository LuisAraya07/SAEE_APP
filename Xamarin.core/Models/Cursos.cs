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
using Castle.Components.DictionaryAdapter;

namespace Xamarin.core.Models
{

    public partial class Cursos
    {
        public Cursos()
        {
            CursosGrupos = new HashSet<CursosGrupos>();
        }
        [Key("Id")]
        public int Id { get; set; }
        public int IdProfesor { get; set; }
        public string Nombre { get; set; }
        public int CantidadPeriodos { get; set; }

        public virtual Profesores IdProfesorNavigation { get; set; }
        public virtual ICollection<CursosGrupos> CursosGrupos { get; set; }
    }
}