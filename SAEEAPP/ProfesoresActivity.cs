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
    [Activity(Label = "Profesores", Theme = "@style/AppTheme")]
    public class ProfesoresActivity : AppCompatActivity
    {
        List<Profesores> profesores = null;
        ListView lvProfesores;
        ProfesoresListAdapter profesoresAdapter;
        TextView tvCargando;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_profesores);
            FloatingActionButton btAgregar = FindViewById<FloatingActionButton>(Resource.Id.btAgregar);
            btAgregar.Click += AgregarProfesor;
        }

        protected override async void OnStart()
        {
            base.OnStart();

            lvProfesores = FindViewById<ListView>(Resource.Id.lvProfesores);
            tvCargando = FindViewById<TextView>(Resource.Id.tvCargando);

            if (profesores == null)
            {
                ProfesoresServices servicioProfesores = new ProfesoresServices();
                profesores = await servicioProfesores.GetAsync();
                if (profesores.Count == 0)
                {
                    tvCargando.Text = "No hay datos";
                }
                else
                {
                    tvCargando.Visibility = ViewStates.Gone;
                    profesoresAdapter = new ProfesoresListAdapter(this, profesores);
                    lvProfesores.Adapter = profesoresAdapter;
                }
            }
        }

        private void AgregarProfesor(object sender, EventArgs e)
        {
            AgregarEditarProfesorActivity agregarProfesorActivity =
                new AgregarEditarProfesorActivity(this, profesoresAdapter, profesores);
            agregarProfesorActivity.Show();
        }
    }
}