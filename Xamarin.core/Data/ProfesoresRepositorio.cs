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
    public class ProfesoresRepositorio
    {

        private readonly HttpClient client;

        public ProfesoresRepositorio()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri($"{ValuesServices.url}/")
            };
        }

        public async Task<bool> PostAsync(Profesores profesor)
        {
            var serializedProfesor = JsonConvert.SerializeObject(profesor);

            var response = await client.PostAsync($"Profesores", new StringContent(serializedProfesor, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<List<Profesores>> GetAsync()
        {
            var json = await client.GetStringAsync($"Profesores");
            return JsonConvert.DeserializeObject<List<Profesores>>(json);
        }

        public async Task<Profesores> GetProfesorAsync(int id)
        {
            var json = await client.GetStringAsync($"api/item/{id}");
            return await Task.Run(() => JsonConvert.DeserializeObject<Profesores>(json));
        }
    }
}