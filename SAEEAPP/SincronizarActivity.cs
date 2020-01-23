using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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
                    Toast.MakeText(context, "Sincronizando, espere...", ToastLength.Short).Show();
                   
                    GruposServices GruposServicios = new GruposServices();
                    GruposServices GruposServiciosOffline = new GruposServices(profesor.Id);



                    CursosServices CursosServicios = new CursosServices();
                    CursosServices CursosServiciosOffline = new CursosServices(idProfesor);


                    EstudiantesServices EstudiantesServicios = new EstudiantesServices();
                    EstudiantesServices EstudiantesServiciosOffline = new EstudiantesServices(idProfesor);

                    
                    //Agregamos el profesor al header
                    var response = await inicioSesionServices.IniciarSesion(profesor.Cedula, profesor.Contrasenia);
                    if (response.IsSuccessStatusCode)
                    {
                        var GruposEliminados = await GruposServicios.DeleteAllGruposAsync();
                        var EstudiantesEliminados = await EstudiantesServicios.DeleteAllEstudiantesAsync();
                        var CursosEliminados = await CursosServicios.DeleteAllCursosAsync();
                        if (GruposEliminados && EstudiantesEliminados && CursosEliminados)
                        {
                            Toast.MakeText(context, "Datos eliminados.", ToastLength.Long).Show();

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

    }
}