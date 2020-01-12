using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using SAEEAPP.Adaptadores;
using System;
using System.Collections.Generic;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{
    [Activity(Label = "Administración de cursos", Theme = "@style/AppTheme")]
    public class CursosActivity : AppCompatActivity
    {
        List<Cursos> cursos = null;
        ListView lvCursos;
        CursosListAdapter cursosAdapter;
        TextView tvCargando;
        CursosServices servicioCursos;
        ProgressBar pbCargandoCursos;
        FloatingActionButton btAgregar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            servicioCursos = new CursosServices();
#pragma warning disable CS0117 // 'Resource.Layout' no contiene una definición para 'activity_cursos'
            SetContentView(Resource.Layout.activity_cursos);
#pragma warning restore CS0117 // 'Resource.Layout' no contiene una definición para 'activity_cursos'
            lvCursos = FindViewById<ListView>(Resource.Id.lvCursos);
            tvCargando = FindViewById<TextView>(Resource.Id.tvCargando);
            btAgregar = FindViewById<FloatingActionButton>(Resource.Id.btAgregar);
            pbCargandoCursos = FindViewById<ProgressBar>(Resource.Id.pbCargandoCursos);
            btAgregar.Click += AgregarCurso;
        }

        protected override async void OnStart()
        {
            base.OnStart();

            if (cursos == null)
            {
                cursos = await servicioCursos.GetAsync();
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
    }
}