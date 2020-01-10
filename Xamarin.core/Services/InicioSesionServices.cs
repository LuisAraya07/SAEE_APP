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
    public class InicioSesionServices
    {
        private readonly InicioSesionRepositorio _inicioSesionR;

        public InicioSesionServices()
        {
            _inicioSesionR = new InicioSesionRepositorio();
        }

        public async Task<HttpResponseMessage> IniciarSesion(string cedula, string contrasenia)
        {
            return await _inicioSesionR.IniciarSesion(cedula, contrasenia);
        }
    }
}