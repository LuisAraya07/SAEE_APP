using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Android.OS;
using Microsoft.EntityFrameworkCore;
using Xamarin.core.Data;
using Xamarin.core.Models;
using Xamarin.core.Offline;

namespace Xamarin.core.Services
{
    public class GruposServices
    {

        private readonly GruposRepositorio _gruposR;
        //Offline
        private int idProfesor;
        private readonly DatabaseContextSistema db;

        public GruposServices()
        {
            _gruposR = new GruposRepositorio();

        }


        //Servicio offline
        public GruposServices(int idProfesor)
        {
            db = new DatabaseContextSistema();
            this.idProfesor = idProfesor;

        }
        //Obtiene los grupos
        public async Task<List<Grupos>> GetOffline()
        {
            await db.Database.MigrateAsync();
            var grupos = await db.Grupos.Where(x => x.IdProfesor == idProfesor).ToListAsync();
            return grupos ?? new List<Grupos>();
        }
        //Agregar grupo
        public async Task<Grupos> PostOffline(Grupos grupo)
        {
            try
            {
                await db.Database.MigrateAsync();
                db.Grupos.Add(grupo);
                await db.SaveChangesAsync();
                return grupo;
            }
            catch (DbUpdateConcurrencyException)
            {
                return grupo;
            }
        }
        //AGREGAR GRUPOS
        public async Task<Boolean> PostAllOffline(List<Grupos> listaGrupos)
        {
            try
            {
                
                await db.Database.MigrateAsync();
                //db.Grupos.AddRange(listaGrupos);
                
                foreach (Grupos cg in listaGrupos)
                {
                 
                    db.Grupos.Add(cg);
                }
                await db.SaveChangesAsync();
                
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }

        //Modificar grupo
        public async Task<Boolean> PutOffline(Grupos grupo)
        {
            try
            {
                await db.Database.MigrateAsync();
                db.Entry(grupo).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }
        //GetGrupo


        //Eliminar Grupo
        public async Task<Boolean> DeleteOffline(Grupos grupo)
        {
            try
            {
                await db.Database.MigrateAsync();
                grupo.EstudiantesXgrupos = db.EG.Where(x => x.IdGrupo == grupo.Id).Include(z => z.IdEstudianteNavigation).ToList();
                db.Grupos.Remove(grupo);
                await db.SaveChangesAsync();
                
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }
        //Obtengo el EG
        public async Task<List<EstudiantesXgrupos>> GetEGOffline(int id)
        {
            await db.Database.MigrateAsync();
            var lista = db.EG.Where(x => x.IdGrupo == id).Include(z => z.IdEstudianteNavigation).ToListAsync();
            return await lista;
        }
        //Elimino el EG
        public async Task<Boolean> DeleteEGOffline(EstudiantesXgrupos EG)
        {
            try
            {
                await db.Database.MigrateAsync();
                db.EG.Remove(EG);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }
        //Agregar el EG
        public async Task<EstudiantesXgrupos> PostEGOffline(EstudiantesXgrupos EG)
        {
            await db.Database.MigrateAsync();
            db.EG.Add(EG);
            await db.SaveChangesAsync();
            return EG;

        }
        //AGREGAR EG TODOS
        public async Task<Boolean> PostAllEGOffline(List<EstudiantesXgrupos> listaEG)
        {
            try { 
                await db.Database.MigrateAsync();
                db.EG.AddRange(listaEG);
                await db.SaveChangesAsync();
                
                }
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }
                return true;
        }

        public async Task<Boolean> PostBDRemota(List<Grupos> listaGrupos)
        {
            GruposRepositorio gruposRepositorio = new GruposRepositorio();
            foreach (Grupos grupo in listaGrupos)
            {
                var grupoNuevo = new Grupos()
                {
                    Anio = grupo.Anio,
                    IdProfesor = grupo.IdProfesor,
                    Grupo =  grupo.Grupo,
                    
                };
                var idNuevoGrupo = gruposRepositorio.PostAsync(grupoNuevo);
            }
            return true;
        }

        /*                          */
        public async Task<List<Grupos>> GetAsync()
        {
             return await _gruposR.GetAsync();
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

        public async Task<List<EstudiantesXgrupos>> GetAllEGAsync()
        {
            return await _gruposR.GetAllEGAsync();
        }


    }
}