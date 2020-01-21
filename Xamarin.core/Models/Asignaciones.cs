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
    public partial class Asignaciones
    {
        [Key("Id")]
        public int Id { get; set; }
        public string Tipo { get; set; }
        public int Profesor { get; set; }
        public int Curso { get; set; }
        public int Grupo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public Decimal Puntos { get; set; }
        public Decimal Porcentaje { get; set; }
    }

}