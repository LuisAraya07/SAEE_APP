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
using Castle.Core;
using Java.Lang;
using Newtonsoft.Json;
using Xamarin.core;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{
    public class SincronizarActivity
    {
        private Context context;
        VerificarConexion vc;
        List<Pair<int, int>> listaParesCursos = new List<Pair<int, int>>();
        List<Pair<int, int>> listaParesGrupos = new List<Pair<int, int>>();
        List<Pair<int, int>> listaParesEstudiantes = new List<Pair<int, int>>();

        public SincronizarActivity(Context context)
        {
            this.context = context;
            vc = new VerificarConexion(context);
        }

        public void CerrarApp()
        {
            Toast.MakeText(context, "Saliendo...", ToastLength.Short).Show();
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            
        }

        public async void Sincronizar()
        {
            ProfesoresServices ns = new ProfesoresServices(1);
            InicioSesionServices inicioSesionServices = new InicioSesionServices();
            var conectado = vc.IsOnline();
            //Verificamos que haya conexión
            if (conectado)
            {
                Profesores profesor = await ns.GetProfesorConectado();
                if(!(profesor == null))
                {
                    int idProfesor = profesor.Id;
                    Toast.MakeText(context, "Sincronizando, un momento...", ToastLength.Short).Show();
                   
                    GruposServices GruposServicios = new GruposServices();
                    GruposServices GruposServiciosOffline = new GruposServices(profesor.Id);



                    CursosServices CursosServicios = new CursosServices();
                    CursosServices CursosServiciosOffline = new CursosServices(idProfesor);


                    EstudiantesServices EstudiantesServicios = new EstudiantesServices();
                    EstudiantesServices EstudiantesServiciosOffline = new EstudiantesServices(idProfesor);

                    EvaluacionesServices EvaluacionesServicios = new EvaluacionesServices();
                    EvaluacionesServices EvaluacionesServiciosOffline = new EvaluacionesServices(idProfesor);

                    AsignacionesServices AsignacionesServicios = new AsignacionesServices();
                    AsignacionesServices AsignacionesServiciosOffline = new AsignacionesServices(idProfesor);
                    //Agregamos el profesor al header
                    var response = await inicioSesionServices.IniciarSesion(profesor.Cedula, profesor.Contrasenia);
                    if (response.IsSuccessStatusCode)
                    {
                        var GruposEliminados = await GruposServicios.DeleteAllGruposAsync();
                        var EstudiantesEliminados = await EstudiantesServicios.DeleteAllEstudiantesAsync();
                        var CursosEliminados = await CursosServicios.DeleteAllCursosAsync();
                        var EvaluacionesEliminados = await EvaluacionesServicios.DeleteAllEvaluacionAsync();
                        var AsignacionesEliminados = await AsignacionesServicios.DeleteAllAsignacionesAsync();
                        if (GruposEliminados && EstudiantesEliminados && CursosEliminados && EvaluacionesEliminados && AsignacionesEliminados)
                        {

                            //Obtenemos la lista de cursos
                            var insertadoCursos = await InsertarDatosCursos(CursosServiciosOffline, CursosServicios);
                            if (insertadoCursos)
                            {
                                var insertadoGrupos = await InsertarDatosGrupos(GruposServiciosOffline, GruposServicios, CursosServiciosOffline);
                                if (insertadoGrupos)
                                {
                                    var insertadoEstudiantes = await InsertarDatosEstudiantes(EstudiantesServiciosOffline, EstudiantesServicios);
                                    if (insertadoEstudiantes)
                                    {
                                        var insertadoFK = await InsertarDatosFK(CursosServiciosOffline, GruposServiciosOffline, GruposServicios);
                                        if (insertadoFK)
                                        {
                                            Toast.MakeText(context, "Datos sincronizados con éxito!", ToastLength.Long).Show();
                                            var bdEliminada = await GruposServiciosOffline.EliminarDBLocal();
                                            if (bdEliminada)
                                            {
                                                Intent mainActivity = new Intent(context, typeof(MainActivity));
                                                context.StartActivity(mainActivity);
                                            }
                                            else
                                            {
                                                Toast.MakeText(context, "Error al eliminar Base de Datos local.", ToastLength.Long).Show();
                                            }
                                        }
                                        else
                                        {
                                            Toast.MakeText(context, "Error al insertar los FK.", ToastLength.Long).Show();
                                        }

                                            
                                    }
                                }
                            }
                        }
                        else
                        {
                            Toast.MakeText(context, "No se pudieron eliminar.", ToastLength.Long).Show();
                        }

                    }
                    else
                    {
                        Toast.MakeText(context, "Error el sincronizar el profesor.", ToastLength.Long).Show();
                    }
                }
                else
                {
                    Toast.MakeText(context, "No tiene base de datos local.", ToastLength.Long).Show();
                }
                
            }
            else
            {
                Toast.MakeText(context,"Necesita conexión a internet.",ToastLength.Long).Show();
            }
        }
        private async Task<bool> InsertarDatosCursos(CursosServices CursosServiciosOffline, CursosServices CursosServicios)
        {
            var listaCursosAgregar = await CursosServiciosOffline.GetOffline();
            foreach (Cursos curso in listaCursosAgregar)
            {
                int idCurso = curso.Id;
                var cursoNuevo = new Cursos()
                {
                    Nombre = curso.Nombre,
                    CantidadPeriodos = curso.CantidadPeriodos
                };
                HttpResponseMessage resultado = await CursosServicios.PostAsync(cursoNuevo);
                if (resultado.IsSuccessStatusCode)
                {
                    // Se obtiene el elemento insertado
                    string resultadoString = await resultado.Content.ReadAsStringAsync();
                    var cursoRemoto = JsonConvert.DeserializeObject<Cursos>(resultadoString);
                    listaParesCursos.Add(new Pair<int, int>(idCurso, cursoRemoto.Id));
                }
                else
                {
                    Toast.MakeText(context, "Error en sincronización Cursos.", ToastLength.Short).Show();
                }
            }
            return true;
        }

        private async Task<bool> InsertarDatosGrupos(GruposServices GruposServiciosOffline, GruposServices GruposServicios, CursosServices CursosServiciosOffline)
        {
            var listaGruposAgregar = await GruposServiciosOffline.GetOffline();
            foreach (Grupos grupo in listaGruposAgregar)
            {
                int idGrupo = grupo.Id;
                var grupoNuevo = new Grupos()
                {
                    Grupo = grupo.Grupo,
                    Anio = grupo.Anio,

                };
                HttpResponseMessage resultado = await GruposServicios.PostAsync(grupoNuevo);
                if (resultado.IsSuccessStatusCode)
                {
                    // Se obtiene el elemento insertado
                    string resultadoString = await resultado.Content.ReadAsStringAsync();
                    var grupoRemoto = JsonConvert.DeserializeObject<Grupos>(resultadoString);
                    //Cabiamos el id en EstudiantesGrupos y CursosGrupos
                    grupoRemoto.IdProfesorNavigation = null;
                    listaParesGrupos.Add(new Pair<int, int>(idGrupo, grupoRemoto.Id));
                }
                else
                {
                    Toast.MakeText(context, "Error en sincronización Grupos.", ToastLength.Short).Show();
                }

            }
            return true;
        }


        private async Task<bool> InsertarDatosEstudiantes(EstudiantesServices EstudiantesServiciosOffline, EstudiantesServices EstudiantesServicios)
        {
            var listaEstudiantesAgregar = await EstudiantesServiciosOffline.GetOffline();
            foreach (Estudiantes estudiante in listaEstudiantesAgregar)
            {
                int idEstudiante = estudiante.Id;
                var estudianteNuevo = new Estudiantes()
                {
                    Cedula = estudiante.Cedula,
                    Nombre = estudiante.Nombre,
                    PrimerApellido = estudiante.PrimerApellido,
                    SegundoApellido = estudiante.SegundoApellido,
                    Pin = estudiante.Pin
                };
                HttpResponseMessage resultado = await EstudiantesServicios.PostAsync(estudianteNuevo);
                if (resultado.IsSuccessStatusCode)
                {
                    // Se obtiene el elemento insertado
                    string resultadoString = await resultado.Content.ReadAsStringAsync();
                    var estudianteRemoto = JsonConvert.DeserializeObject<Estudiantes>(resultadoString);
                    //Cabiamos el id en EstudiantesGrupos y CursosGrupos
                    estudianteRemoto.IdProfesorNavigation = null;
                    listaParesEstudiantes.Add(new Pair<int, int>(idEstudiante, estudianteRemoto.Id));
                }
                else
                {
                    Toast.MakeText(context, "Error en sincronización Estudiantes.", ToastLength.Short).Show();
                }

            }
            return true;
        }

       

        private async Task<bool> InsertarDatosFK(CursosServices cursosServiciosOffline, GruposServices gruposServiciosOffline, GruposServices gruposServicios)
        {
            var listaCG = await cursosServiciosOffline.GetCursosGruposAllOffline();
            var listaEG = await gruposServiciosOffline.GetEGAllOffline();
            var insertadoEG = await gruposServicios.InsertarDatosEGFK(listaEG,listaParesGrupos,listaParesEstudiantes);
            var insertadoCG = await gruposServicios.InsertarDatosCGFK(listaCG,listaParesGrupos,listaParesCursos);
            if(insertadoCG && insertadoEG)
            {
                return true;
            }return false;

            
        }
    }
}