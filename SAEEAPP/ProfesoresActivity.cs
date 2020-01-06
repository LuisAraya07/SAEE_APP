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
    [Activity(Label = "Administración de profesores", Theme = "@style/AppTheme")]
    public class ProfesoresActivity : AppCompatActivity
    {
        List<Profesores> profesores = null;
        ListView lvProfesores;
        ProfesoresListAdapter profesoresAdapter;
        TextView tvCargando;
        private Android.Support.V7.Widget.SearchView _searchView;

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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main, menu);

            var item = menu.FindItem(Resource.Id.searchView1);

            var searchView = MenuItemCompat.GetActionView(item);
            _searchView = searchView.JavaCast<Android.Support.V7.Widget.SearchView>();

            _searchView.QueryTextChange += (s, e) => profesoresAdapter.Filter.InvokeFilter(e.NewText);

            _searchView.QueryTextSubmit += (s, e) =>
            {
                Toast.MakeText(this, "Buscando: ", ToastLength.Short).Show();
                e.Handled = true;
            };

#pragma warning disable CS0612 // 'SearchViewExpandListener' está obsoleto
            MenuItemCompat.SetOnActionExpandListener(item, new SearchViewExpandListener(profesoresAdapter));
#pragma warning restore CS0612 // 'SearchViewExpandListener' está obsoleto
            return true;
        }

    }
}