using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using SAEEAPP.Adaptadores;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{
    [Activity(Label = "Estudiantes", Theme = "@style/AppTheme")]
    public class EstudiantesActivity : AppCompatActivity
    {
        private List<Estudiantes> listaEstudiantes = new List<Estudiantes>();
        ListEstudiantesAdaptador adaptadorEstudiantes;
        private FloatingActionButton fab;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_estudiantes);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fabEstudiante);
            fab.Click += AgregarEstudiante;

        }

        protected override async void OnStart()
        {
            base.OnStart();
            var estudiantesServicio = new EstudiantesServices();
            var lvEstudiantes = FindViewById<ListView>(Resource.Id.listViewEstudiantes);
            listaEstudiantes = await estudiantesServicio.GetAsync(1);
            TextView tvCargando = FindViewById<TextView>(Resource.Id.tvCargandoE);
            if (listaEstudiantes.Count == 0)
            {
               
                tvCargando.Text = "No hay datos";
            }
            else
            {
                adaptadorEstudiantes = new ListEstudiantesAdaptador(this, listaEstudiantes);
                tvCargando.Visibility = ViewStates.Gone;
                lvEstudiantes.Adapter = adaptadorEstudiantes;
            }
        }

        private void AgregarEstudiante(object sender, EventArgs e)
        {
            AgregarEstudianteActivity agregarEstudianteActivity = new AgregarEstudianteActivity(this, adaptadorEstudiantes, listaEstudiantes);
            agregarEstudianteActivity.Show();
        }
    }
}