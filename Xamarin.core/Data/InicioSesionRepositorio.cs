using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
    public class InicioSesionRepositorio
    {
        private readonly HttpClient client;

        public InicioSesionRepositorio()
        {
            client = ClienteHttp.ObtenerHttpClient();
        }

        public async Task<HttpResponseMessage> IniciarSesion(string cedula, string contrasenia)
        {
            Usuario usuario = new Usuario() { 
                Profesor = new Profesores() { 
                    Cedula = cedula, Contrasenia = contrasenia
                }
            };
            var serializedUsuario = JsonConvert.SerializeObject(usuario);
            var response = await client.PostAsync($"InicioSesion", new StringContent(serializedUsuario, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                ClienteHttp.EstablecerHeaders(response.Content);
            }

            return response;
        }
    }
}