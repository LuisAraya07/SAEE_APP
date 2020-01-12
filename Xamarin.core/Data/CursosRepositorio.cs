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
    public class CursosRepositorio
    {
        private readonly HttpClient client;

        public CursosRepositorio()
        {
            client = ClienteHttp.ObtenerHttpClient();
        }

        public async Task<HttpResponseMessage> PostAsync(Cursos curso)
        {
            var serializedCurso = JsonConvert.SerializeObject(curso);

            var response = await client.PostAsync($"Cursos", new StringContent(serializedCurso, Encoding.UTF8, "application/json"));

            return response;
        }

        public async Task<List<Cursos>> GetAsync()
        {
            var json = await client.GetStringAsync($"Cursos");
            return JsonConvert.DeserializeObject<List<Cursos>>(json);
        }

        public async Task<Cursos> GetCursoAsync(int id)
        {
            var json = await client.GetStringAsync($"Cursos/{id}");
            return JsonConvert.DeserializeObject<Cursos>(json);
        }

        public async Task<bool> UpdateCursoAsync(Cursos curso)
        {
            var serializedCurso = JsonConvert.SerializeObject(curso);
            var response = await client.PutAsync($"Cursos/{curso.Id}", new StringContent(serializedCurso, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AgregarCursosGruposAsync(List<CursosGrupos> cursosGrupos)
        {
            var serializedCursosGrupos = JsonConvert.SerializeObject(cursosGrupos);
            var response = await client.PutAsync($"CursosGrupos/PostCursosGrupos", new StringContent(serializedCursosGrupos, Encoding.UTF8, "application/json"));
            
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> BorrarCursosGruposAsync(List<CursosGrupos> cursosGrupos)
        {
            var serializedCursosGrupos = JsonConvert.SerializeObject(cursosGrupos);
            var response = await client.PutAsync($"CursosGrupos/DeleteCursosGrupos", new StringContent(serializedCursosGrupos, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<List<CursosGrupos>> GetCursosGruposAsync(int id)
        {
            var json = await client.GetStringAsync($"CursosGrupos/{id}");
            return JsonConvert.DeserializeObject<List<CursosGrupos>>(json);
        }

        public async Task<bool> DeleteCursoAsync(int id)
        {
            var response = await client.DeleteAsync($"Cursos/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}