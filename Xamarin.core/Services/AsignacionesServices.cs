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
    public class AsignacionesServices
    {
        private readonly AsignacionesRepositorio _asignacionesR;
        private int idProfesor;
        private readonly DatabaseContextSistema db;
        public AsignacionesServices()
        {
            _asignacionesR = new AsignacionesRepositorio();
        }
        public AsignacionesServices(int idProfesor)
        {
            db = new DatabaseContextSistema();
            this.idProfesor = idProfesor;
        }
        //Agregar curso
        public async Task<Asignaciones> PostOffline(Asignaciones asignacion)
        {
            try
            {
                asignacion.Id = idProfesor;
                await db.Database.MigrateAsync();
                db.Asignaciones.Add(asignacion);
                await db.SaveChangesAsync();
                return asignacion;
            }
            catch (DbUpdateConcurrencyException)
            {
                return asignacion;
            }
        }
        //Agregar todas las asignaciones
        public async Task<Boolean> PostAllOffline(List<Asignaciones> listaAsignaciones)
        {
            try
            {
                await db.Database.MigrateAsync();
                foreach (Asignaciones asignacion in listaAsignaciones)
                {
                    var asignacionNuevo = new Asignaciones()
                    {
                        Id = asignacion.Id,
                        Profesor = asignacion.Profesor,
                        Curso = asignacion.Curso,
                        Descripcion = asignacion.Descripcion,
                        Estado = asignacion.Estado,
                        Fecha = asignacion.Fecha,
                        Grupo = asignacion.Grupo,
                        Nombre = asignacion.Nombre,
                        Porcentaje =asignacion.Porcentaje,
                        Puntos=asignacion.Puntos,
                        Tipo = asignacion.Tipo
                    };
                    
                    db.Asignaciones.Add(asignacionNuevo);

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
        public async Task<List<Asignaciones>> GetOffline()
        {
            await db.Database.MigrateAsync();
            return await db.Asignaciones.Where(asignacion => asignacion.Profesor == idProfesor).ToListAsync();
        }
        //Obtener una asignacion
        public async Task<Asignaciones> GetAsignacionOffline(int id)
        {
            await db.Database.MigrateAsync();
            var asignacion = await db.Asignaciones.FindAsync(id);
            //Puede ser nulo
            return asignacion;
        }
        //Modificar asignacion
        public async Task<bool> UpdateAsignacionOffline(Asignaciones asignacion)
        {
            await db.Database.MigrateAsync();
            db.Entry(asignacion).State = EntityState.Modified;
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
        public async Task<bool> DeleteAsignacionOffline(int id)
        {
            try
            {
                await db.Database.MigrateAsync();
                var evaluaciones = await db.Evaluaciones.Where(eva => eva.Asignacion == id).ToListAsync();
                db.Evaluaciones.RemoveRange(evaluaciones);
                await db.SaveChangesAsync();
                var asig = await db.Asignaciones.Where(asignacion => asignacion.Id == id).FirstOrDefaultAsync();
                if (asig == null)
                {
                    return false;
                }
                db.Asignaciones.Remove(asig);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;

        }



        /*        Termina Offline          */
        public async Task<HttpResponseMessage> PostAsync(Asignaciones asignacion)
        {
            return await _asignacionesR.PostAsync(asignacion);
        }

        public async Task<List<Asignaciones>> GetAsync()
        {
            return await _asignacionesR.GetAsync();
        }

        public async Task<Asignaciones> GetAsignacionAsync(int id)
        {
            return await _asignacionesR.GetAsignacionesAsync(id);
        }

        public async Task<bool> UpdateAsignacionAsync(Asignaciones asignacion)
        {
            return await _asignacionesR.UpdateAsignacionesAsync(asignacion);
        }

        public async Task<bool> DeleteAsignacionAsync(int id)
        {
            return await _asignacionesR.DeleteAsignacionAsync(id);
        }
        //ELIMINA TODAS LAS ASIGNACIONES REMOTAS
        public async Task<bool> DeleteAllAsignacionesAsync()
        {
            return await _asignacionesR.DeleteAllAsignacionesAsync();
        }
    }
}