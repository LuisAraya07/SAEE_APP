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
    public class CursosServices
    {
        private readonly CursosRepositorio _cursosR;

        //Offline
        private int idProfesor;
        private readonly DatabaseContextSistema db;
        public CursosServices()
        {
            _cursosR = new CursosRepositorio();
        }

        //Servicio offline
        public CursosServices(int idProfesor)
        {
            db = new DatabaseContextSistema();
            this.idProfesor = idProfesor;
        }
        //Agregar curso
        public async Task<Cursos> PostOffline(Cursos curso)
        {
            try
            {
                await db.Database.MigrateAsync();
                db.Cursos.Add(curso);
                await db.SaveChangesAsync();
                return curso;
            }
            catch (DbUpdateConcurrencyException)
            {
                return curso;
            }
        }
        //Agregar todos los cursos
        public async Task<Boolean> PostAllOffline(List<Cursos> listaCursos)
        {
            try
            {
                await db.Database.MigrateAsync();
                db.Cursos.AddRange(listaCursos);
                await db.SaveChangesAsync();
                
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }
        //Obtener cursos
        public async Task<List<Cursos>> GetOffline()
        {
            await db.Database.MigrateAsync();
            return await db.Cursos.Include(curso => curso.CursosGrupos)
                .ThenInclude(cursoGrupo => cursoGrupo.IdGrupoNavigation)
                .Where(curso => curso.IdProfesor == idProfesor).ToListAsync();
        }
        //Obtener un curso
        public async Task<Cursos> GetCursoOffline(int id)
        {
            await db.Database.MigrateAsync();
            var cursos = await db.Cursos.FindAsync(id);
            //Puede ser nulo
            return cursos;
        }
        //Modificar curso
        public async Task<bool> UpdateCursoOffline(Cursos curso)
        {
            await db.Database.MigrateAsync();
            db.Entry(curso).State = EntityState.Modified;
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
        //Eliminar curso
        public async Task<bool> DeleteCursoOffline(int id)
        {
            try
            {
                await db.Database.MigrateAsync();
                var cursos = await db.Cursos.Include(curso => curso.CursosGrupos)
                   .ThenInclude(cursoGrupo => cursoGrupo.IdGrupoNavigation)
                   .Where(curso => curso.Id == id).FirstOrDefaultAsync();
                if (cursos == null)
                {
                    return false;
                }
                db.Cursos.Remove(cursos);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;

        }

        
        //Obtener CG por curso
        public async Task<List<CursosGrupos>> GetCursosGruposOffline(int id)
        {
            await db.Database.MigrateAsync();
            var cursosGrupos = await db.CursosGrupos.Include(cursoGrupo => cursoGrupo.IdGrupoNavigation).
                Where(cursoGrupo => cursoGrupo.IdCurso == id).ToListAsync();

            return cursosGrupos;
        }
        //Agregar CG
        public async Task<bool> AgregarCursosGruposAllOffline(List<CursosGrupos> cursosGrupos)
        {
            await db.Database.MigrateAsync();
            db.CursosGrupos.AddRange(cursosGrupos);
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
        //Eliminar CG
        public async Task<bool> BorrarCursosGruposOffline(List<CursosGrupos> cursosGrupos)
        {
            await db.Database.MigrateAsync();
            CursosGrupos borrarGC;
            foreach (CursosGrupos cg in cursosGrupos)
            {
                borrarGC = db.CursosGrupos.FirstOrDefault(cgt => cgt.Id == cg.Id);
                db.CursosGrupos.Remove(borrarGC);
            }   
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
        //Editar CG



        /*        Termina Offline          */
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
        public async Task<List<CursosGrupos>> GetCursosGruposAllAsync()
        {
            return await _cursosR.GetCursosGruposAllAsync();
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