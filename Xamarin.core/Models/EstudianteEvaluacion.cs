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
    public partial class EstudianteEvaluacion
    {
        public Evaluaciones evaluacion { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public Decimal Puntos { get; set; }
        public Decimal Porcentaje { get; set; }
        public int Nota { get; set; }
        public string Estado { get; set; }
    }

}