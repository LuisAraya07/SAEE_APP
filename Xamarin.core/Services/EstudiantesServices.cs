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
using Xamarin.core.Data;
using Xamarin.core.Models;

namespace Xamarin.core.Services
{
    public class EstudiantesServices
    {
        private EstudiantesRepositorio _estudiantesR;
        public EstudiantesServices()
        {
            _estudiantesR = new EstudiantesRepositorio();

        }

        public List<Estudiantes> Get(int id)
        {

            return _estudiantesR.Get(id);


        }

        public Estudiantes GetEstudiante(int id)
        {
            return _estudiantesR.GetEstudiante(id);

        }
    }
}