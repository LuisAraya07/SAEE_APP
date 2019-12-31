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
    public class GruposRepositorio
    {
        private readonly HttpClient client;

        public GruposRepositorio() {
            client = new HttpClient
            {
                BaseAddress = new Uri($"{ValuesServices.url}/")
                
            };
        }

        public async Task<List<Grupos>> GetAsync(int id)
        {
            var json = await client.GetStringAsync($"Grupos/GetGrupos?id="+id);
            return JsonConvert.DeserializeObject<List<Grupos>>(json);
        }

        public async Task<List<Estudiantes>> GetGrupoAsync(int id)
        {
            var json = await client.GetStringAsync($"Grupos/GetEstudiantes?id="+id);
            return await Task.Run(() => JsonConvert.DeserializeObject<List<Estudiantes>>(json));
        }

        //Agregar Grupo
        public async Task<Grupos> PostAsync(Grupos grupo)
        {
            var serializedGrupo = JsonConvert.SerializeObject(grupo);
            var response = await client.PostAsync($"Grupos/PostGrupos", new StringContent(serializedGrupo, Encoding.UTF8, "application/json"));
            string resString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Grupos>(resString);
        }

        //Modificar grupo
        public async Task<bool> PutAsync(Grupos grupo)
        {
            var serializedGrupo = JsonConvert.SerializeObject(grupo);
            var buffer = Encoding.UTF8.GetBytes(serializedGrupo);
            var byteContent = new ByteArrayContent(buffer);
            var response = await client.PutAsync($"Grupos/PutGrupos", byteContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteGrupoAsync(Grupos grupo)
        {
            var serializedGrupo = JsonConvert.SerializeObject(grupo);
            var buffer = Encoding.UTF8.GetBytes(serializedGrupo);
            var byteContent = new ByteArrayContent(buffer);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{ValuesServices.url}/Grupos/DeleteGrupos"),
                Content = new StringContent(serializedGrupo, Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }



    }
}