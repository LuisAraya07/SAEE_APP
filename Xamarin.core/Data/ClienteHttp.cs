using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace Xamarin.core.Data
{
    public class ClienteHttp
    {
        private static HttpClient client = null;
        public static Usuario Usuario = null;

        public static HttpClient ObtenerHttpClient()
        {
            if (client == null)
            {
                client = new HttpClient
                {
                    BaseAddress = new Uri($"{ValuesServices.url}/")
                };
            }
            return client;
        }

        public static async void EstablecerHeaders(HttpContent content)
        {
            // Se obtiene el elemento
            string resultadoString = await content.ReadAsStringAsync();
            // Se establece el usuario
            Usuario = new Usuario
            {
                Profesor = JsonConvert.DeserializeObject<Profesores>(resultadoString)
            };
            ActualizarHeaders();
        }
        public static void EstablecerHeaders(string cedula, string contrasenia)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("cedula", cedula);
            client.DefaultRequestHeaders.Add("contrasenia", contrasenia);
        }

        public static void ActualizarHeaders()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("cedula", Usuario.Profesor.Cedula);
            client.DefaultRequestHeaders.Add("contrasenia", Usuario.Profesor.Contrasenia);
        }
    }
}