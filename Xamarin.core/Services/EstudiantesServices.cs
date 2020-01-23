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
using Microsoft.EntityFrameworkCore;
using Xamarin.core.Data;
using Xamarin.core.Models;
using Xamarin.core.Offline;

namespace Xamarin.core.Services
{
    public class EstudiantesServices
    {
        private readonly EstudiantesRepositorio _estudiantesR;

        //Offline
        private readonly int idProfesor;
        private readonly DatabaseContextSistema db;

        public EstudiantesServices()
        {
            _estudiantesR = new EstudiantesRepositorio();

        }

        //Servicio offline
        public EstudiantesServices(int idProfesor)
        {
            db = new DatabaseContextSistema();
            this.idProfesor = idProfesor;

        }
        //Obtener Estudiantes
        public async Task<List<Estudiantes>> GetOffline()
        {
            await db.Database.MigrateAsync();
            return await db.Estudiantes.Include(grupo => grupo.EstudiantesXgrupos)
            .Where(curso => curso.IdProfesor == idProfesor).ToListAsync();
        }
        //Agregar estudiante
        public async Task<Estudiantes> PostOffline(Estudiantes estudiante)
        {
            await db.Database.MigrateAsync();
            estudiante.IdProfesor = idProfesor;
            db.Estudiantes.Add(estudiante);
            await db.SaveChangesAsync();
            return estudiante;


        }
        //Agregar TODOS estudiantes
        public async Task<Boolean> PostAllOffline(List<Estudiantes> listaEstudiantes)
        {
            await db.Database.MigrateAsync();
            foreach (Estudiantes estudiante in listaEstudiantes)
            {
                var estudianteNuevo = new Estudiantes()
                {
                    Id = estudiante.Id,
                    Nombre = estudiante.Nombre,
                    IdProfesor = estudiante.IdProfesor,
                    Cedula = estudiante.Cedula,
                    Pin = estudiante.Pin,
                    PrimerApellido = estudiante.PrimerApellido,
                    SegundoApellido = estudiante.SegundoApellido
                    
                    
                };
                db.Estudiantes.Add(estudianteNuevo);

            }
            await db.SaveChangesAsync();
            return true;

        }
        //Modificar estudiante
        public async Task<bool> PutOffline(Estudiantes estudiante)
        {
            try
            {
                await db.Database.MigrateAsync();
                db.Entry(estudiante).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;


        }
        //Eliminar estudiante
        public async Task<bool> DeleteEstudiantesOffline(Estudiantes estudiante)
        {
            try
            {
                await db.Database.MigrateAsync();
                var listaAsignaciones = db.Evaluaciones.Where(x => x.Estudiante == estudiante.Id).ToList();
                db.Evaluaciones.RemoveRange(listaAsignaciones);
                await db.SaveChangesAsync();
                db.Estudiantes.Remove(estudiante);
                await db.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }
        //Cambiar idEstudiantes en EstudiantesXGrupos
        public async Task<bool> CambiarIdEstudianteEG(int idViejo, int idNuevo)
        {
            try
            {
                await db.Database.MigrateAsync();
                var listaEG = await db.EG.Where(x => x.IdEstudiante == idViejo).ToListAsync();
                foreach (EstudiantesXgrupos eg in listaEG)
                {
                    eg.IdEstudiante = idNuevo;
                    db.Entry(eg).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }





        /*      Termina Offline        */
        public async Task<List<Estudiantes>> GetAsync()
        {
            return await _estudiantesR.GetAsync();
        }

       
        //Agregar estudiantes
        public async Task<HttpResponseMessage> PostAsync(Estudiantes estudiante)
        {
            return await _estudiantesR.PostAsync(estudiante);
        }

        //Modificar estudiante
        public async Task<bool> PutAsync(Estudiantes estudiante)
        {
            return await _estudiantesR.PutAsync(estudiante);
        }
        public async Task<bool> DeleteEstudiantesAsync(Estudiantes estudiante)
        {
            return await _estudiantesR.DeleteEstudiantesAsync(estudiante);
        }

        //ELIMINAMOS TODOS LOS Estudiantes
        public async Task<Boolean> DeleteAllEstudiantesAsync()
        {
            return await _estudiantesR.DeleteAllEstudiantesAsync();
        }


    }
}