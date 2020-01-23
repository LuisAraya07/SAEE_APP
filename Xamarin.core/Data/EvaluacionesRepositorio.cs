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
    public class EvaluacionesRepositorio
    {
        private readonly HttpClient client;
        public EvaluacionesRepositorio()
        {
            client = ClienteHttp.ObtenerHttpClient();
        }
        public async Task<HttpResponseMessage> PostAsync(Evaluaciones evaluacion)
        {
            var serializedAsignacion = JsonConvert.SerializeObject(evaluacion);

            var response = await client.PostAsync($"Evaluaciones", new StringContent(serializedAsignacion, Encoding.UTF8, "application/json"));

            return response;
        }

        public async Task<List<Evaluaciones>> GetEvaluacionesAsync()
        {
            var json = await client.GetStringAsync($"Evaluaciones");
            return JsonConvert.DeserializeObject<List<Evaluaciones>>(json);
        }

        public async Task<List<Evaluaciones>> GetEvaluacionesxAsignacionAsync(int asignacion)
        {
            var json = await client.GetStringAsync($"Evaluaciones/{asignacion}");
            return JsonConvert.DeserializeObject<List<Evaluaciones>>(json);
        }

        public async Task<Evaluaciones> GetEvaluacionAsync(int id)
        {
            var json = await client.GetStringAsync($"Evaluaciones/{id}");
            return JsonConvert.DeserializeObject<Evaluaciones>(json);
        }

        public async Task<bool> UpdateEvaluacionAsync(Evaluaciones evaluacion)
        {
            var serializedAsignacion = JsonConvert.SerializeObject(evaluacion);
            var response = await client.PutAsync($"Evaluaciones/{evaluacion.Id}", new StringContent(serializedAsignacion, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteEvaluacionAsync(int id)
        {
            var response = await client.DeleteAsync($"Evaluaciones/{id}");

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteAllEvaluacionAsync()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{ValuesServices.url}/Evaluaciones/DeleteAllEvaluaciones")
            };
            var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}