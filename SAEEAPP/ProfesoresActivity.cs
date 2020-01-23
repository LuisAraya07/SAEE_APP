﻿using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
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
        FloatingActionButton btAgregar;
        ProgressBar pbCargandoProfesores;
        private Android.Support.V7.Widget.SearchView _searchView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_profesores);
            lvProfesores = FindViewById<ListView>(Resource.Id.lvProfesores);
            tvCargando = FindViewById<TextView>(Resource.Id.tvCargando);
            pbCargandoProfesores = FindViewById<ProgressBar>(Resource.Id.pbCargandoProfesores);
            btAgregar = FindViewById<FloatingActionButton>(Resource.Id.btAgregar);
            btAgregar.Click += AgregarProfesor;
        }

        protected override async void OnStart()
        {
            base.OnStart();

            if (profesores == null)
            {
                ProfesoresServices servicioProfesores = new ProfesoresServices();
                profesores = await servicioProfesores.GetAsync();
                if (profesores.Count > 0)
                {
                    tvCargando.Visibility = ViewStates.Invisible;
                }
                // El mensaje de "no hay datos", lo asigna el Adapter
                profesoresAdapter = new ProfesoresListAdapter(this, profesores, tvCargando);
                lvProfesores.Adapter = profesoresAdapter;
                pbCargandoProfesores.Visibility = ViewStates.Gone;
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
            //var salir = menu.FindItem(Resource.Id.CerrarSesion);
            //var btnS = MenuItemCompat.GetActionView(salir);
            ////var draw = ContextCompat.GetDrawable(this, Resource.Drawable.ic_salir);
            ////btnSalir.SetCompoundDrawablesWithIntrinsicBounds(draw, null, null, null);
            //btnSalir.Click += delegate
            //{
            //    Toast.MakeText(this, "Cerrr Sesión", ToastLength.Short).Show();
            //};
            var searchView = MenuItemCompat.GetActionView(item);
            _searchView = searchView.JavaCast<Android.Support.V7.Widget.SearchView>();

            _searchView.QueryTextChange += (s, e) => profesoresAdapter.Filter.InvokeFilter(e.NewText);

            _searchView.QueryTextSubmit += (s, e) =>
            {
                Toast.MakeText(this, "Buscando: ", ToastLength.Short).Show();
                e.Handled = true;
            };

            MenuItemCompat.SetOnActionExpandListener(item, new SearchViewExpandListener(profesoresAdapter));
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            SincronizarActivity menuOpciones = new SincronizarActivity(this);
            var itemS = item.ItemId;
            switch(itemS){
                case Resource.Id.CerrarSesion:
                    menuOpciones.CerrarApp();
                    break;
                case Resource.Id.Sincronizar:
                    menuOpciones.Sincronizar();
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        



    }
}