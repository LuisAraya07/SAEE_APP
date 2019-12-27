using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<Profesores> Get()
        {
            return _profesoresR.Get();
        }

        public async Task<List<Profesores>> GetAsync()
        {
            return await _profesoresR.GetAsync();
        }

        public Profesores GetProfesor(int id)
        {
            return _profesoresR.GetProfesor(id);
        }
    }
}