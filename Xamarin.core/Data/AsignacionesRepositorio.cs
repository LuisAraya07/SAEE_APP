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
    public class AsignacionesRepositorio
    {
        private readonly HttpClient client;
        public AsignacionesRepositorio()
        {
            client = ClienteHttp.ObtenerHttpClient();
        }
        public async Task<HttpResponseMessage> PostAsync(Asignaciones asignacion)
        {
            var serializedAsignacion = JsonConvert.SerializeObject(asignacion);

            var response = await client.PostAsync($"Asignaciones", new StringContent(serializedAsignacion, Encoding.UTF8, "application/json"));

            return response;
        }

        public async Task<List<Asignaciones>> GetAsync()
        {
            var json = await client.GetStringAsync($"Asignaciones");
            return JsonConvert.DeserializeObject<List<Asignaciones>>(json);
        }

        public async Task<Asignaciones> GetAsignacionesAsync(int id)
        {
            var json = await client.GetStringAsync($"Asignaciones/{id}");
            return JsonConvert.DeserializeObject<Asignaciones>(json);
        }

        public async Task<bool> UpdateAsignacionesAsync(Asignaciones asignacion)
        {
            var serializedAsignacion = JsonConvert.SerializeObject(asignacion);
            var response = await client.PutAsync($"Asignaciones/{asignacion.Id}", new StringContent(serializedAsignacion, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteAsignacionAsync(int id)
        {
            var response = await client.DeleteAsync($"Asignaciones/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<Boolean> DeleteAllAsignacionesAsync()
        {
            var request = new HttpRequestMessage
            {

                RequestUri = new Uri($"{ValuesServices.url}/Asignaciones/DeleteAllAsignaciones")
            };
            var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}