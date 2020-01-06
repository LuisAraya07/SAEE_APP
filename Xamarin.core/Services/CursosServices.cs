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
    public class CursosServices
    {
        private readonly CursosRepositorio _cursosR;

        public CursosServices()
        {
            _cursosR = new CursosRepositorio();
        }

        public async Task<HttpResponseMessage> PostAsync(Cursos curso)
        {
            return await _cursosR.PostAsync(curso);
        }

        public async Task<List<Cursos>> GetAsync()
        {
            return await _cursosR.GetAsync();
        }

        public async Task<Cursos> GetCursoAsync(int id)
        {
            return await _cursosR.GetCursoAsync(id);
        }

        public async Task<bool> UpdateCursoAsync(Cursos curso)
        {
            return await _cursosR.UpdateCursoAsync(curso);
        }

        public async Task<bool> AgregarCursosGruposAsync(List<CursosGrupos> cursosGrupos)
        {
            return await _cursosR.AgregarCursosGruposAsync(cursosGrupos);
        }

        public async Task<bool> BorrarCursosGruposAsync(List<CursosGrupos> cursosGrupos)
        {
            return await _cursosR.BorrarCursosGruposAsync(cursosGrupos);
        }

        public async Task<List<CursosGrupos>> GetCursosGruposAsync(int id)
        {
            return await _cursosR.GetCursosGruposAsync(id);
        }

        public async Task<bool> DeleteCursoAsync(int id)
        {
            return await _cursosR.DeleteCursoAsync(id);
        }
    }
}