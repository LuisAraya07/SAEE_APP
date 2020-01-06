using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using SAEEAPP.Adaptadores;
using SAEEAPP.JavaHolder;
using System;
using System.Collections.Generic;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{
    [Activity(Label = "@string/seccionEstudiantes", Theme = "@style/AppTheme")]
    public class EstudiantesActivity : AppCompatActivity
    {
        private List<Estudiantes> listaEstudiantes = new List<Estudiantes>();
        ListEstudiantesAdaptador adaptadorEstudiantes;
        private FloatingActionButton fab;
        private Android.Support.V7.Widget.SearchView _searchView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_estudiantes);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fabEstudiante);
            fab.Visibility = ViewStates.Invisible;
            fab.Click += AgregarEstudiante;
            // buscarEstudiante = FindViewById<SearchView>(Resource.Id.searchView1);



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
            fab.Visibility = ViewStates.Visible;
            // buscarEstudiante.QueryTextChange += (s, e) => adaptadorEstudiantes.Filter.InvokeFilter(e.NewText);
        }
        private void AgregarEstudiante(object sender, EventArgs e)
        {
            AgregarEstudianteActivity agregarEstudianteActivity = new AgregarEstudianteActivity(this, adaptadorEstudiantes, listaEstudiantes);
            agregarEstudianteActivity.Show();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main, menu);

            var item = menu.FindItem(Resource.Id.searchView1);

            var searchView = MenuItemCompat.GetActionView(item);
            _searchView = searchView.JavaCast<Android.Support.V7.Widget.SearchView>();

            _searchView.QueryTextChange += (s, e) => adaptadorEstudiantes.Filter.InvokeFilter(e.NewText);

            _searchView.QueryTextSubmit += (s, e) =>
            {
                Toast.MakeText(this, "Buscando: ", ToastLength.Short).Show();
                e.Handled = true;
            };

            MenuItemCompat.SetOnActionExpandListener(item, new SearchViewExpandListener(adaptadorEstudiantes));
            return true;
        }



    }
}