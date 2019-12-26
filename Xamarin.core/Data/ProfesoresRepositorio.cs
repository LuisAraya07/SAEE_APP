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
    public class ProfesoresRepositorio
    {
        private readonly WebServices ws;
        public ProfesoresRepositorio()
        {
            ws = new WebServices();
        }

        public List<Profesores> Get()
        {
            var response = ws.Get(ValuesServices.url + "Profesores");

            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Profesores>>(response.Content);
        }

        public Profesores GetGrupo(int id)
        {
            var response = ws.Get(ValuesServices.url + "Profesores/" + id);
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException("Profesor no encontrado");
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Profesores>(response.Content);
        }
    }
}