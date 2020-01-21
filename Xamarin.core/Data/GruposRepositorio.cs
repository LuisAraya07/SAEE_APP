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
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace Xamarin.core.Data
{
    public class GruposRepositorio
    {
        private readonly HttpClient client;

        public GruposRepositorio() {
            client = ClienteHttp.ObtenerHttpClient();
        }

        public async Task<List<Grupos>> GetAsync()
        {
           

            var json = await client.GetStringAsync($"Grupos/GetGrupos");
            return JsonConvert.DeserializeObject<List<Grupos>>(json);
        }
      

        public async Task<List<Estudiantes>> GetGrupoAsync(int id)
        {
            var json = await client.GetStringAsync($"Grupos/GetEstudiantes?id="+id);
            return await Task.Run(() => JsonConvert.DeserializeObject<List<Estudiantes>>(json));
        }

        public async Task<List<EstudiantesXgrupos>> GetEGAsync(int id)
        {
            var json = await client.GetStringAsync($"Grupos/GetEG?id=" + id);
            return await Task.Run(() => JsonConvert.DeserializeObject<List<EstudiantesXgrupos>>(json));
        }

        public async Task<List<EstudiantesXgrupos>> GetAllEGAsync()
        {
            var json = await client.GetStringAsync($"Grupos/GetEGOffline");
            return await Task.Run(() => JsonConvert.DeserializeObject<List<EstudiantesXgrupos>>(json));
        }

        //Agregar Grupo
        public async Task<HttpResponseMessage> PostAsync(Grupos grupo)
        {
            var serializedGrupo = JsonConvert.SerializeObject(grupo);
            var response = await client.PostAsync($"Grupos/PostGrupos", new StringContent(serializedGrupo, Encoding.UTF8, "application/json"));
            return response;
        }

        //Modificar grupo
        public async Task<bool> PutAsync(Grupos grupo)
        {
            var serializedGrupo = JsonConvert.SerializeObject(grupo);
            var response = await client.PutAsync($"Grupos/PutGrupos", new StringContent(serializedGrupo, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteGrupoAsync(Grupos grupo)
        {
            var serializedGrupo = JsonConvert.SerializeObject(grupo);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{ValuesServices.url}/Grupos/DeleteGrupos"),
                Content = new StringContent(serializedGrupo, Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<Boolean> DeleteEGAsync(EstudiantesXgrupos EG)
        {
            var serializedEG = JsonConvert.SerializeObject(EG);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{ValuesServices.url}/Grupos/DeleteEG"),
                Content = new StringContent(serializedEG, Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<EstudiantesXgrupos> PostEGAsync(EstudiantesXgrupos EG)
        {
            var serializedEG = JsonConvert.SerializeObject(EG);
            var response = await client.PostAsync($"Grupos/PostEG", new StringContent(serializedEG, Encoding.UTF8, "application/json"));
            string resString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<EstudiantesXgrupos>(resString);
        }



    }
}