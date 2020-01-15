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
using Xamarin.core.Data;
using Xamarin.core.Models;

namespace Xamarin.core.Services
{
    public class ProfesoresServices
    {
        private readonly ProfesoresRepositorio _profesoresR;

        public ProfesoresServices()
        {
            _profesoresR = new ProfesoresRepositorio();
        }

        public async Task<HttpResponseMessage> PostAsync(Profesores profesor)
        {
            return await _profesoresR.PostAsync(profesor);
        }

        public async Task<List<Profesores>> GetAsync()
        {
            return await _profesoresR.GetAsync();
        }

        public async Task<Profesores> GetProfesorAsync(int id)
        {
            return await _profesoresR.GetProfesorAsync(id);
        }

        public async Task<bool> UpdateProfesorAsync(Profesores profesor)
        {
            return await _profesoresR.UpdateProfesorAsync(profesor);
        }

        public async Task<bool> UpdatePerfilAsync(Profesores profesor)
        {
            return await _profesoresR.UpdatePerfilAsync(profesor);
        }

        public async Task<bool> DeleteProfesorAsync(int id)
        {
            return await _profesoresR.DeleteProfesorAsync(id);
        }
    }
}