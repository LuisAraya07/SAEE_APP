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
    public class ProfesoresServices
    {
        private readonly ProfesoresRepositorio _profesoresR;
        //Offline
        private readonly int idProfesor;
        private readonly DatabaseContextSistema db;

        public ProfesoresServices()
        {
            _profesoresR = new ProfesoresRepositorio();
        }

        //Servicio offline
        public ProfesoresServices(int idProfesor)
        {
            db = new DatabaseContextSistema();
            this.idProfesor = idProfesor;

        }
        //Agregar profesor
        public async Task<Profesores> PostOffline(Profesores profesores)
        {
            await db.Database.MigrateAsync();
            var listaProfesores = db.Profesores.ToList();
            db.Profesores.RemoveRange(listaProfesores);
            await db.SaveChangesAsync();
            db.Profesores.Add(profesores);
            await db.SaveChangesAsync();
            return profesores;
        }
        //Obtener profesores
        public async Task<List<Profesores>> GetOffline()
        {
            await db.Database.MigrateAsync();
            return await db.Profesores.ToListAsync();
        }
        //Obtener Profesor Conectado
        public async Task<Profesores> GetProfesorConectado()
        {
            await db.Database.MigrateAsync();
            var profesor = await db.Profesores.FirstOrDefaultAsync();
            return profesor;
        }

        public async Task<bool> VerificarProfesorConectado(Profesores profesorConectado)
        {
            await db.Database.MigrateAsync();
            var profesor = await db.Profesores.FirstOrDefaultAsync();
            if (profesor == null)
            {
                return false;
            }
            if(profesor.Id == profesorConectado.Id)
            {
                return true;
            }return false;
        }
        //Editar profesores No debería servir
        public async Task<bool> UpdateProfesorOffline(Profesores profesor)
        {
            try
            {
                await db.Database.MigrateAsync();
                db.Entry(profesor).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }
        //Eliminar profesor
        public async Task<bool> DeleteProfesorOffline(int id)
        {
            await db.Database.MigrateAsync();
            var profesores = await db.Profesores.Include(profesor => profesor.Cursos)
                .Include(profesor => profesor.EstudiantesXgrupos)
                .Include(profesor => profesor.Estudiantes)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (profesores == null)
            {
                return false;
            }
            db.Profesores.Remove(profesores);
            await db.SaveChangesAsync();
            return true;
        }



        /*   Termina offline       */
        public async Task<HttpResponseMessage> PostAsync(Profesores profesor)
        {
            return await _profesoresR.PostAsync(profesor);
        }

        public async Task<List<Profesores>> GetAsync()
        {
            return await _profesoresR.GetAsync();
        }

        public async Task<Profesores> GetProfesorAsync(int id)
        {
            return await _profesoresR.GetProfesorAsync(id);
        }

        public async Task<bool> UpdateProfesorAsync(Profesores profesor)
        {
            return await _profesoresR.UpdateProfesorAsync(profesor);
        }

        public async Task<bool> UpdatePerfilAsync(Profesores profesor)
        {
            return await _profesoresR.UpdatePerfilAsync(profesor);
        }

        public async Task<bool> DeleteProfesorAsync(int id)
        {
            return await _profesoresR.DeleteProfesorAsync(id);
        }
    }
}