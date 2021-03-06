﻿using Android.App;
using Android.Content;
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
using Xamarin.core;
using Xamarin.core.Models;
using Xamarin.core.OfflineServices;
using Xamarin.core.Services;

namespace SAEEAPP
{
    [Activity(Label = "@string/seccionEstudiantes", Theme = "@style/AppTheme")]
    public class EstudiantesActivity : AppCompatActivity
    {
        private List<Estudiantes> listaEstudiantes = new List<Estudiantes>();
        ListEstudiantesAdaptador adaptadorEstudiantes;
        TextView tvCargando;
        ListView lvEstudiantes;
        ProgressBar pbCargandoEstudiantes;
        private FloatingActionButton fab;
        private Android.Support.V7.Widget.SearchView _searchView;
        VerificarConexion vc;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_estudiantes);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fabEstudiante);
            lvEstudiantes = FindViewById<ListView>(Resource.Id.listViewEstudiantes);
            tvCargando = FindViewById<TextView>(Resource.Id.tvCargandoE);
            pbCargandoEstudiantes = FindViewById<ProgressBar>(Resource.Id.pbCargandoEstudiantes);
            fab.Visibility = ViewStates.Invisible;
            vc = new VerificarConexion(this);
            fab.Click += AgregarEstudiante;
            // buscarEstudiante = FindViewById<SearchView>(Resource.Id.searchView1);
        }

        protected override async void OnStart()
        {
            base.OnStart();
            EstudiantesServices estudiantesServicio;
            var conectado = vc.IsOnline();
            if (conectado)
            {
                estudiantesServicio = new EstudiantesServices();
                listaEstudiantes = await estudiantesServicio.GetAsync();
            }
            else
            {
                // Toast.MakeText(this,"Necesita conexión a internet.",ToastLength.Long).Show();
                ProfesoresServices ns = new ProfesoresServices(1);
                Profesores profesor = await ns.GetProfesorConectado();
                estudiantesServicio = new EstudiantesServices(profesor.Id);
                listaEstudiantes = await estudiantesServicio.GetOffline();
            }
            
            if (listaEstudiantes.Count > 0)
            {

                tvCargando.Visibility = ViewStates.Invisible;
            }
            adaptadorEstudiantes = new ListEstudiantesAdaptador(this, listaEstudiantes, tvCargando);
            
            pbCargandoEstudiantes.Visibility = ViewStates.Gone;
            lvEstudiantes.Adapter = adaptadorEstudiantes;
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