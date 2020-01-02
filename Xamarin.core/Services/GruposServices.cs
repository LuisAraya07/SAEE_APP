﻿using System;
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
    public class GruposServices
    {

        private readonly GruposRepositorio _gruposR;
        public GruposServices()
        {
            _gruposR = new GruposRepositorio();

        }

        public async Task<List<Grupos>> GetAsync(int id)
        {
            return await _gruposR.GetAsync(id);
        }

        public async Task <List<Estudiantes>> GetGrupo(int id) {
            return await _gruposR.GetGrupoAsync(id);
        
        }
        //Agregar grupo
        public async Task<HttpResponseMessage> PostAsync(Grupos grupo)
        {
            return await _gruposR.PostAsync(grupo);
        }
        //Modificar grupo
        public async Task<bool> PutAsync(Grupos grupo)
        {
            return await _gruposR.PutAsync(grupo);
        }
        public async Task<bool> DeleteGruposAsync(Grupos grupo)
        {
            return await _gruposR.DeleteGrupoAsync(grupo);
        }
        public async Task<List<EstudiantesXgrupos>> GetEGAsync(int id)
        {
            return await _gruposR.GetEGAsync(id);
        }
        public async Task<Boolean> DeleteEGAsync(EstudiantesXgrupos EG)
        {
            return await _gruposR.DeleteEGAsync(EG);
        }
        public async Task<EstudiantesXgrupos> PostEGAsync(EstudiantesXgrupos EG) {
            return await _gruposR.PostEGAsync(EG);
        
        }

    }
}