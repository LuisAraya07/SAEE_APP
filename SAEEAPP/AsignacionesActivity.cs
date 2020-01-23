using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using SAEEAPP.Adaptadores;
using System;
using System.Collections.Generic;
using Xamarin.core;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{
    [Activity(Label = "Asignaciones", Theme = "@style/AppTheme")]
    public class AsignacionesActivity : AppCompatActivity
    {
        List<Asignaciones> asignaciones = null;
        ListView lvAsignaciones;
        AsignacionesAdaptador AdaptadorAsignaciones;
        TextView tvCargando;
        AsignacionesServices servicioAsignaciones;
        ProgressBar pbCargandoAsignaciones;
        FloatingActionButton btAgregar;


        List<Cursos> cursos = null;
        CursosServices cursosServicio;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            servicioAsignaciones = new AsignacionesServices();
            SetContentView(Resource.Layout.activity_asignaciones);
            // Create your application here
            lvAsignaciones = FindViewById<ListView>(Resource.Id.lvAsignaciones);
            tvCargando = FindViewById<TextView>(Resource.Id.tvCargando);
            btAgregar = FindViewById<FloatingActionButton>(Resource.Id.btAgregar);
            pbCargandoAsignaciones = FindViewById<ProgressBar>(Resource.Id.pbCargandoAsignaciones);
            btAgregar.Click += AgregarAsignacion;


            cursosServicio = new CursosServices();
        }
        protected async override void OnStart()
        {
            base.OnStart();
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                if (cursos == null)
                {

                    cursos = await cursosServicio.GetAsync();
                }
                if (asignaciones == null)
                {
                    asignaciones = await servicioAsignaciones.GetAsync();
                }
            }
            else
            {
                //AQUI OFFLINE
                ProfesoresServices ns = new ProfesoresServices(1);
                Profesores profesor = await ns.GetProfesorConectado();
                if (!(profesor == null))
                {
                    CursosServices cursosServiciosOffline = new CursosServices(profesor.Id);
                    AsignacionesServices asignacionesServiciosOffline = new AsignacionesServices(profesor.Id);
                    cursos = await cursosServiciosOffline.GetOffline();
                    asignaciones = await asignacionesServiciosOffline.GetOffline();
                }
                else
                {
                    Toast.MakeText(this, "No hay bases de datos local.", ToastLength.Long).Show();
                }
            }
            if (asignaciones.Count > 0)
            {
                tvCargando.Visibility = ViewStates.Invisible;
            }
            // El mensaje de "no hay datos", lo asigna el Adapter (ya que se pueden eliminar todos los curso)
            AdaptadorAsignaciones = new AsignacionesAdaptador(this, asignaciones, tvCargando, cursos);
            lvAsignaciones.Adapter = AdaptadorAsignaciones;
            pbCargandoAsignaciones.Visibility = ViewStates.Gone;

        }
      
        private void AgregarAsignacion(object sender, EventArgs e)
        {
           AgregarEditarAsignacionesActivity agregarCursoActivity =
                new AgregarEditarAsignacionesActivity(this, AdaptadorAsignaciones, asignaciones,cursos);
            agregarCursoActivity.Show();
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