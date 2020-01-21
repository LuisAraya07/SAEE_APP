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
        }
        protected override async void OnStart()
        {
            base.OnStart();

            if (asignaciones == null)
            {
                asignaciones = await servicioAsignaciones.GetAsync();
                if (asignaciones.Count > 0)
                {
                    tvCargando.Visibility = ViewStates.Invisible;
                }
                // El mensaje de "no hay datos", lo asigna el Adapter (ya que se pueden eliminar todos los curso)
                AdaptadorAsignaciones = new AsignacionesAdaptador(this, asignaciones, tvCargando);
                lvAsignaciones.Adapter = AdaptadorAsignaciones;
                pbCargandoAsignaciones.Visibility = ViewStates.Gone;
            }
        }
        private void AgregarAsignacion(object sender, EventArgs e)
        {
         /*   AgregarEditarCursosActivity agregarCursoActivity =
                new AgregarEditarCursosActivity(this, AdaptadorAsignaciones, asignaciones);
            agregarCursoActivity.Show();*/
        }
    }
}