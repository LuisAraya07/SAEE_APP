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
        public int Id { get; set; }
        public int IdProfesor { get; set; }
        public string Grupo { get; set; }
        public int Anio { get; set; }
    }
}