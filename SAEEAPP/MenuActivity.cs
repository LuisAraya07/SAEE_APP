﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;
using Xamarin.core;
using Xamarin.core.Data;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{
    [Activity(Label = "Menú Principal",  Theme = "@style/AppTheme")]
    public class MenuActivity : AppCompatActivity
    {
        Button btUsuario;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_Menu);
            btUsuario = FindViewById<Button>(Resource.Id.btUsuario);
            btUsuario.Click += OnClick_Usuario;
            Button btCursos = FindViewById<Button>(Resource.Id.btCursos);
            btCursos.Click += OnClick_Cursos;
            Button btGrupos = FindViewById<Button>(Resource.Id.btGrupos);
            btGrupos.Click += OnClick_Grupos;
            Button btProfesores = FindViewById<Button>(Resource.Id.btProfesores);
            btProfesores.Click += OnClick_Profesores;
            Button btEstudiantes = FindViewById<Button>(Resource.Id.btEstudiantes);
            btEstudiantes.Click += OnClick_Estudiantes;
            Button btNotificaciones = FindViewById<Button>(Resource.Id.btNotificaciones);
            btNotificaciones.Click += OnClick_Notificaciones;


            //nuevo
            Button btAsignaciones = FindViewById<Button>(Resource.Id.btAsignaciones);
            btAsignaciones.Click += OnClick_Asignaciones;

            Button btEvaluaciones = FindViewById<Button>(Resource.Id.btEvaluaciones);
            btEvaluaciones.Click += OnClick_Evaluaciones;
            InicioSesionServices inicioSesionServices = new InicioSesionServices();
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            //Verificamos que haya conexión
            if (conectado)
            {
                if (!ClienteHttp.Usuario.Profesor.Administrador)
                {
                    btProfesores.Visibility = ViewStates.Gone;
                }
            }

                
        }

        protected async override void OnStart()
        {
            base.OnStart();
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                btUsuario.Text = $"{ClienteHttp.Usuario.Profesor.Nombre} {ClienteHttp.Usuario.Profesor.PrimerApellido}";
            }
            else
            {
                ProfesoresServices ns = new ProfesoresServices(1);
                Profesores profesor = await ns.GetProfesorConectado();
                btUsuario.Text = $"{profesor.Nombre} {profesor.PrimerApellido}";
            }
            
        }

        public void OnClick_Usuario(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                Intent usuario = new Intent(this, typeof(UsuarioActivity));
                StartActivity(usuario);
            }
            else
            {
                Toast.MakeText(this, "Necesita conexión a internet.", ToastLength.Long).Show();
            }

        }


        //NUEVO ASIGNACIONES
        public async void OnClick_Asignaciones(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                Intent asignacion = new Intent(this, typeof(AsignacionesActivity));
                StartActivity(asignacion);
            }
            else
            {
                //AQUI OFFLINE
                ProfesoresServices ns = new ProfesoresServices(1);
                Profesores profesor = await ns.GetProfesorConectado();
                if (!(profesor == null))
                {
                    Intent asignacion = new Intent(this, typeof(AsignacionesActivity));
                    StartActivity(asignacion);
                }
                else
                {
                    Toast.MakeText(this, "No hay bases de datos local.", ToastLength.Long).Show();
                }
                    
            }

        }

        public async void OnClick_Evaluaciones(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                Intent asignacion = new Intent(this, typeof(EvaluacionesActivity));
                StartActivity(asignacion);
            }
            else
            {
                ProfesoresServices ns = new ProfesoresServices(1);
                Profesores profesor = await ns.GetProfesorConectado();
                if (!(profesor == null))
                {
                    Intent asignacion = new Intent(this, typeof(EvaluacionesActivity));
                    StartActivity(asignacion);
                }
                else
                {
                    Toast.MakeText(this, "No hay bases de datos local.", ToastLength.Long).Show();
                }
                    
            }

        }

        public async void OnClick_Cursos(object sender, EventArgs e)
        {
            
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                Intent cursos = new Intent(this, typeof(CursosActivity));
                StartActivity(cursos);
            }
            else
            {
                ProfesoresServices ns = new ProfesoresServices(1);
                Profesores profesor = await ns.GetProfesorConectado();
                if (!(profesor == null))
                {
                    Intent cursos = new Intent(this, typeof(CursosActivity));
                    StartActivity(cursos);
                }
                else
                {
                    Toast.MakeText(this, "No hay bases de datos local.", ToastLength.Long).Show();
                }

            }

        }

        public async void OnClick_Grupos(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                Intent grupos = new Intent(this, typeof(GruposActivity));
                StartActivity(grupos);
            }
            else
            {
                ProfesoresServices ns = new ProfesoresServices(1);
                Profesores profesor = await ns.GetProfesorConectado();
                if (!(profesor == null))
                {
                    Intent grupos = new Intent(this, typeof(GruposActivity));
                    StartActivity(grupos);
                }
                else
                {
                    Toast.MakeText(this, "No hay bases de datos local.", ToastLength.Long).Show();
                }

            }

        }

        public void OnClick_Profesores(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                Intent profesores = new Intent(this, typeof(ProfesoresActivity));
                StartActivity(profesores);
            }
            else
            {
                Toast.MakeText(this, "Necesita conexión a internet.", ToastLength.Long).Show();
            }
            
        }

        public async void OnClick_Estudiantes(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                Intent estudiantes = new Intent(this, typeof(EstudiantesActivity));
                StartActivity(estudiantes);
            }
            else
            {
                ProfesoresServices ns = new ProfesoresServices(1);
                Profesores profesor = await ns.GetProfesorConectado();
                if (!(profesor == null))
                {
                    Intent estudiantes = new Intent(this, typeof(EstudiantesActivity));
                    StartActivity(estudiantes);
                }
                else
                {
                    Toast.MakeText(this, "No hay bases de datos local.", ToastLength.Long).Show();
                }

            }

        }

        public void OnClick_Notificaciones(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(this);
            Intent notificaciones = new Intent(this, typeof(NotificacionesActivity));
            StartActivity(notificaciones);
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main2, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            SincronizarActivity menuOpciones = new SincronizarActivity(this);
            var itemS = item.ItemId;
            switch (itemS)
            {
                case Resource.Id.CerrarSesion:
                    menuOpciones.CerrarApp();
                    break;
                case Resource.Id.Sincronizar:
                    EnviarSync();
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public void EnviarSync()
        {
            var vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            //Verificamos que haya conexión
            if (conectado)
            {
                Intent usuario = new Intent(this, typeof(SyncActivity));
                StartActivity(usuario);
            }
            else
            {
                Toast.MakeText(this, "Necesita conexión a internet.", ToastLength.Short).Show();
            }

        }

    }
}