﻿using Castle.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
            var grupos = await db.Grupos.Where(x => x.IdProfesor == idProfesor).Include(grupo => grupo.EstudiantesXgrupos).ToListAsync();
            return grupos;
        }
        //Agregar grupo
        public async Task<Grupos> PostOffline(Grupos grupo)
        {
            try
            {
                grupo.IdProfesor = idProfesor;
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
                List<EstudiantesXgrupos> ListaEG = new List<EstudiantesXgrupos>();
                foreach (Grupos cg in listaGrupos)
                {
                    var grupoNuevo = new Grupos()
                    {
                        Id = cg.Id,
                        Anio = cg.Anio,
                        IdProfesor = cg.IdProfesor,
                        Grupo = cg.Grupo
                    };
                    ListaEG.AddRange(cg.EstudiantesXgrupos.ToList());
                    db.Grupos.Add(grupoNuevo);

                }
                await db.SaveChangesAsync();
                await PostAllEGOffline(ListaEG);
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
                grupo.EstudiantesXgrupos = db.EG.Where(x => x.IdGrupo == grupo.Id).ToList();
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
        //Obtengo Todos los EG
        public async Task<List<EstudiantesXgrupos>> GetEGAllOffline()
        {
            await db.Database.MigrateAsync();
            var lista = db.EG.ToListAsync();
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
            try
            {
                await db.Database.MigrateAsync();
                foreach (EstudiantesXgrupos eg in listaEG)
                {
                    if (!(db.Grupos.FindAsync(eg.IdGrupo) == null))
                    {
                        var egNuevo = new EstudiantesXgrupos()
                        {
                            Id = eg.Id,
                            IdGrupo = eg.IdGrupo,
                            IdEstudiante = eg.IdEstudiante

                        };
                        db.EG.Add(egNuevo);
                    }
                }
                // db.EG.AddRange(listaEG);
                await db.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> InsertarDatosCGFK(List<CursosGrupos> listaCG, List<Pair<int, int>> listaParesGrupos, List<Pair<int, int>> listaParesCursos)
        {
            CursosServices cursosServices = new CursosServices();
            List<CursosGrupos> listaAgregar = new List<CursosGrupos>();
            foreach (CursosGrupos CG in listaCG)
            {
                int idGrupo = CG.IdGrupo;
                int idCurso = CG.IdCurso;
                var nuevoCG = new CursosGrupos();
                for (int i = 0; i < listaParesGrupos.Count; i++)
                {
                    if (idGrupo == listaParesGrupos[i].First)
                    {
                        nuevoCG.IdGrupo = listaParesGrupos[i].Second;
                        for (int k = 0; k < listaParesCursos.Count; k++)
                        {
                            if (idCurso == listaParesCursos[k].First)
                            {
                                nuevoCG.IdCurso = listaParesCursos[k].Second;
                                listaAgregar.Add(nuevoCG);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            return await cursosServices.AgregarCursosGruposAsync(listaAgregar);


        }
        public async Task<bool> InsertarDatosEGFK(List<EstudiantesXgrupos> listaEG, List<Pair<int, int>> listaParesGrupos, List<Pair<int, int>> listaParesEstudiantes)
        {
            foreach (EstudiantesXgrupos eg in listaEG)
            {
                int idGrupo = eg.IdGrupo;
                int idEstudiante = eg.IdEstudiante;
                var nuevoEG = new EstudiantesXgrupos();
                for (int i = 0; i < listaParesGrupos.Count; i++)
                {
                    if (idGrupo == listaParesGrupos[i].First)
                    {
                        nuevoEG.IdGrupo = listaParesGrupos[i].Second;
                        for (int k = 0; k < listaParesEstudiantes.Count; k++)
                        {
                            if (idEstudiante == listaParesEstudiantes[k].First)
                            {
                                nuevoEG.IdEstudiante = listaParesEstudiantes[k].Second;
                                await PostEGAsync(nuevoEG);
                                break;
                            }
                        }
                        break;

                    }
                }

            }
            return true;
        }



        public async Task<bool> EliminarDBLocal()
        {
            try
            {
                await db.Database.MigrateAsync();
                await db.Database.EnsureDeletedAsync();
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }






        /*          TERMINA OFFLINE                */
        public async Task<List<Grupos>> GetAsync()
        {
            return await _gruposR.GetAsync();
        }
        public async Task<List<Estudiantes>> GetGrupo(int id)
        {
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
        //ELIMINAMOS TODOS LOS GRUPOS
        public async Task<Boolean> DeleteAllGruposAsync()
        {
            return await _gruposR.DeleteAllGruposAsync();
        }
        public async Task<List<EstudiantesXgrupos>> GetEGAsync(int id)
        {
            return await _gruposR.GetEGAsync(id);
        }
        public async Task<Boolean> DeleteEGAsync(EstudiantesXgrupos EG)
        {
            return await _gruposR.DeleteEGAsync(EG);
        }
        public async Task<EstudiantesXgrupos> PostEGAsync(EstudiantesXgrupos EG)
        {
            return await _gruposR.PostEGAsync(EG);

        }

        public async Task<List<EstudiantesXgrupos>> GetAllEGAsync()
        {
            return await _gruposR.GetAllEGAsync();
        }


    }
}