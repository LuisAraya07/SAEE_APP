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
    public class Usuario
    {
        public Profesores Profesor { get; set; }

        public string HttpHeader { get; set; }
    }
}