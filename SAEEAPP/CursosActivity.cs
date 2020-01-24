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
    [Activity(Label = "Cursos", Theme = "@style/AppTheme")]
    public class CursosActivity : AppCompatActivity
    {
        List<Cursos> cursos = null;
        ListView lvCursos;
        CursosListAdapter cursosAdapter;
        TextView tvCargando;
        CursosServices servicioCursos;
        ProgressBar pbCargandoCursos;
        FloatingActionButton btAgregar;
        VerificarConexion vc;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            servicioCursos = new CursosServices();
            SetContentView(Resource.Layout.activity_cursos);
            lvCursos = FindViewById<ListView>(Resource.Id.lvCursos);
            tvCargando = FindViewById<TextView>(Resource.Id.tvCargando);
            btAgregar = FindViewById<FloatingActionButton>(Resource.Id.btAgregar);
            pbCargandoCursos = FindViewById<ProgressBar>(Resource.Id.pbCargandoCursos);
            btAgregar.Click += AgregarCurso;
            vc = new VerificarConexion(this);
        }

        protected override async void OnStart()
        {
            base.OnStart();

            if (cursos == null)
            {
                var conectado = vc.IsOnline();
                if (conectado)
                {
                    cursos = await servicioCursos.GetAsync();
                }
                else
                {
                    //AQUI OFFLINE
                    ProfesoresServices ns = new ProfesoresServices(1);
                    Profesores profesor = await ns.GetProfesorConectado();
                    if (!(profesor == null))
                    {
                        CursosServices servicioOffline = new CursosServices(profesor.Id);
                        cursos = await servicioOffline.GetOffline();
                    }
                    else
                    {
                        Toast.MakeText(this,"No hay bases de datos local.",ToastLength.Long).Show();
                        cursos = new List<Cursos>();
                    }
                        
                }
                if (cursos.Count > 0)
                {
                    tvCargando.Visibility = ViewStates.Invisible;
                }
                // El mensaje de "no hay datos", lo asigna el Adapter (ya que se pueden eliminar todos los curso)
                cursosAdapter = new CursosListAdapter(this, cursos, tvCargando);
                lvCursos.Adapter = cursosAdapter;
                pbCargandoCursos.Visibility = ViewStates.Gone;
            }
        }

        private void AgregarCurso(object sender, EventArgs e)
        {
            AgregarEditarCursosActivity agregarCursoActivity =
                new AgregarEditarCursosActivity(this, cursosAdapter, cursos);
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