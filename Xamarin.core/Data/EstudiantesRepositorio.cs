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
    public class EstudiantesRepositorio
    {
        private readonly HttpClient client;

        public EstudiantesRepositorio()
        {
            client = ClienteHttp.ObtenerHttpClient();
        }


      
        public async Task<List<Estudiantes>> GetAsync()
        {
            var json = await client.GetStringAsync($"Estudiantes/GetEstudiantes");
            return JsonConvert.DeserializeObject<List<Estudiantes>>(json);
        }

        public async Task<Estudiantes> GetEstudianteAsync()
        {
            var json = await client.GetStringAsync($"Estudiantes/GetEstudiante");
            return await Task.Run(() => JsonConvert.DeserializeObject<Estudiantes>(json));

        }
        //Agregar estudiante
        public async Task<HttpResponseMessage> PostAsync(Estudiantes estudiante)
        {
            var serializedEstudiante = JsonConvert.SerializeObject(estudiante);
            var response = await client.PostAsync($"Estudiantes/PostEstudiantes", new StringContent(serializedEstudiante, Encoding.UTF8, "application/json"));
            return response;
        }

        //Modificar estudiante
        public async Task<bool> PutAsync(Estudiantes estudiante)
        {
            var serializedEstudiante = JsonConvert.SerializeObject(estudiante);
            var response = await client.PutAsync($"Estudiantes/PutEstudiantes", new StringContent(serializedEstudiante, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEstudiantesAsync(Estudiantes estudiante)
        {
            var serializedEstudiante = JsonConvert.SerializeObject(estudiante);
           // var buffer = Encoding.UTF8.GetBytes(serializedEstudiante);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{ValuesServices.url}/Estudiantes/DeleteEstudiantes"),
                Content = new StringContent(serializedEstudiante, Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

    }
}