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
using Microsoft.EntityFrameworkCore;
using Xamarin.core.Models;
using Xamarin.core.Offline;

namespace Xamarin.core.OfflineServices
{
    public class NotificacionesServices
    {
        //private readonly string dbNotificaciones = "dbNotificaciones.db";
        private DatabaseContext db;

        public NotificacionesServices(string databasePath)
        {
            db = new DatabaseContext(databasePath);
        }

        public async Task<List<Notificaciones>> GetNotificaciones(int idProfesor)
        {
            await db.Database.MigrateAsync();
            var notificaciones = await db.Notificaciones.Where(x => x.IdProfesor == idProfesor).ToListAsync();
            return (notificaciones==null)?new List<Notificaciones>() : notificaciones;

        }
        public async Task<List<Notificaciones>> GetNotificacionesEliminar(int idProfesor) {
            
            await db.Database.MigrateAsync();
            var notificacionesNoProfesor = await db.Notificaciones.Where(x => x.IdProfesor != idProfesor).ToListAsync();
            return notificacionesNoProfesor;
        }


       
        //Modificar
        public Boolean PutNotificaciones(Notificaciones notificacion)
        {
            try
            {

                
                 db.Database.MigrateAsync();
                db.Entry(notificacion).State = EntityState.Modified;
                 db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }

        //Agregar
        public Notificaciones PostNotificaciones(Notificaciones notificacion)
        {
            try
            {
                
                 db.Database.MigrateAsync();
                db.Notificaciones.Add(notificacion);
                 db.SaveChangesAsync();
                return notificacion;
            }
            catch (DbUpdateConcurrencyException)
            {
                return notificacion;
            }

        }

        //Borrar
        public Boolean DeleteNotificaciones(Notificaciones notificacion)
        {
            try
            {
                
                 db.Database.MigrateAsync();
                db.Notificaciones.Remove(notificacion);
                 db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }

    }
}