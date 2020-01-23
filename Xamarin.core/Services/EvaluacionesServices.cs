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
    public class EvaluacionesServices
    {
        private readonly EvaluacionesRepositorio _evaluacionesR;
        private int idProfesor;
        private readonly DatabaseContextSistema db;
        public EvaluacionesServices()
        {
            _evaluacionesR = new EvaluacionesRepositorio();
        }
        public EvaluacionesServices(int idProfesor)
        {
            db = new DatabaseContextSistema();
            this.idProfesor = idProfesor;
        }
        //Agregar curso
        public async Task<Evaluaciones> PostOffline(Evaluaciones evaluacion)
        {
            try
            {
                await db.Database.MigrateAsync();
                db.Evaluaciones.Add(evaluacion);
                await db.SaveChangesAsync();
                return evaluacion;
            }
            catch (DbUpdateConcurrencyException)
            {
                return evaluacion;
            }
        }
        //Agregar todas las Evaluaciones
        public async Task<Boolean> PostAllOffline(List<Evaluaciones> listaEvaluaciones)
        {
            try
            {
                await db.Database.MigrateAsync();
                foreach (Evaluaciones evaluacion in listaEvaluaciones)
                {
                    var evaluacionNuevo = new Evaluaciones()
                    {
                        Id = evaluacion.Id,
                        Puntos = evaluacion.Puntos,
                        Porcentaje = evaluacion.Porcentaje,
                        Asignacion =evaluacion.Asignacion,
                        Estado = evaluacion.Estado,
                        Estudiante= evaluacion.Estudiante,
                        Nota=evaluacion.Nota,
                        Periodo=evaluacion.Periodo,
                        Profesor=evaluacion.Profesor
                    };

                    db.Evaluaciones.Add(evaluacionNuevo);

                }
                await db.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }
        //Obtener asignaciones
        public async Task<List<Evaluaciones>> GetOffline()
        {
            await db.Database.MigrateAsync();
            return await db.Evaluaciones.Where(evaluacion => evaluacion.Profesor == idProfesor).ToListAsync();
        }
        //Obtener una asignacion
        public async Task<Evaluaciones> GetEvaluacionOffline(int id)
        {
            await db.Database.MigrateAsync();
            var evaluacion = await db.Evaluaciones.FindAsync(id);
            //Puede ser nulo
            return evaluacion;
        }
        //Modificar asignacion
        public async Task<bool> UpdateEvaluacionOffline(Evaluaciones evaluacion)
        {
            await db.Database.MigrateAsync();
            db.Entry(evaluacion).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }
        //Eliminar asignacion
        public async Task<bool> DeleteEvaluacionOffline(int id)
        {
            try
            {
                await db.Database.MigrateAsync();
                var asig = await db.Evaluaciones.Where(evaluacion => evaluacion.Id == id).FirstOrDefaultAsync();
                if (asig == null)
                {
                    return false;
                }
                db.Evaluaciones.Remove(asig);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;

        }



        /*        Termina Offline          */
        public async Task<HttpResponseMessage> PostAsync(Evaluaciones evaluacion)
        {
            return await _evaluacionesR.PostAsync(evaluacion);
        }

        public async Task<List<Evaluaciones>> GetEvaluacionesxAsignacionAsync(int asignacion)
        {
            return await _evaluacionesR.GetEvaluacionesxAsignacionAsync(asignacion);
        }

        public async Task<Evaluaciones> GetEvaluacionAsync(int id)
        {
            return await _evaluacionesR.GetEvaluacionAsync(id);
        }

        public async Task<bool> UpdateEvaluacionAsync(Evaluaciones evaluacion)
        {
            return await _evaluacionesR.UpdateEvaluacionAsync(evaluacion);
        }

        public async Task<bool> DeleteEvaluacionAsync(int id)
        {
            return await _evaluacionesR.DeleteEvaluacionAsync(id);
        }
        //Eliminar todas las evaluaciones dle profesor remotas
        public async Task<bool> DeleteAllEvaluacionAsync()
        {
            return await _evaluacionesR.DeleteAllEvaluacionAsync();
        }


        public async Task<List<Evaluaciones>> GetAllEvaluacionesAsync()
        {
            return await _evaluacionesR.GetAllEvaluacionesAsync();
        }
    }
}