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
    public class EstudiantesServices
    {
        private readonly EstudiantesRepositorio _estudiantesR;
        public EstudiantesServices()
        {
            _estudiantesR = new EstudiantesRepositorio();

        }

        public async Task<List<Estudiantes>> GetAsync()
        {
            return await _estudiantesR.GetAsync();
        }


        public async Task<Estudiantes> GetEstudiante()
        {
            return await _estudiantesR.GetEstudianteAsync();

        }
        //Agregar estudiantes
        public async Task<HttpResponseMessage> PostAsync(Estudiantes estudiante)
        {
            return await _estudiantesR.PostAsync(estudiante);
        }

        //Modificar grupo
        public async Task<bool> PutAsync(Estudiantes estudiante)
        {
            return await _estudiantesR.PutAsync(estudiante);
        }
        public async Task<bool> DeleteEstudiantesAsync(Estudiantes estudiante)
        {
            return await _estudiantesR.DeleteEstudiantesAsync(estudiante);
        }


    }
}