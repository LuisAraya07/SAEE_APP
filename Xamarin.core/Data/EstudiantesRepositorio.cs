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
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace Xamarin.core.Data
{
    public class EstudiantesRepositorio
    {
        private List<Estudiantes> estudiantes;
        private WebServices ws;
        public EstudiantesRepositorio()
        {
            ws = new WebServices();
        }


        public List<Estudiantes> Get(int id)
        {
            var response = ws.Get(ValuesServices.url + "Estudiantes/GetEstudiantes?id=" + id);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Estudiantes>>(response.Content);

        }

        public Estudiantes GetEstudiante(int id)
        {
            var response = ws.Get(ValuesServices.url + "Estudiantes/GetEstudiante?id=" + id);
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException("Estudiante no encontrado");
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Estudiantes>(response.Content);

        }
    }
}