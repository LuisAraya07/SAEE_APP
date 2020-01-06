﻿using System;
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
    [Activity(Label = "Cursos", Theme = "@style/AppTheme")]
    public class CursosActivity : AppCompatActivity
    {
        List<Cursos> cursos = null;
        ListView lvCursos;
        CursosListAdapter cursosAdapter;
        TextView tvCargando;
        CursosServices servicioCursos;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            servicioCursos = new CursosServices();
            SetContentView(Resource.Layout.activity_cursos);
            FloatingActionButton btAgregar = FindViewById<FloatingActionButton>(Resource.Id.btAgregar);
            btAgregar.Click += AgregarCurso;
        }

        protected override async void OnStart()
        {
            base.OnStart();

            lvCursos = FindViewById<ListView>(Resource.Id.lvCursos);
            tvCargando = FindViewById<TextView>(Resource.Id.tvCargando);

            if (cursos == null)
            {
                cursos = await servicioCursos.GetAsync();
                if (cursos.Count == 0)
                {
                    tvCargando.Text = "No hay datos";
                }
                else
                {
                    tvCargando.Visibility = ViewStates.Gone;
                    cursosAdapter = new CursosListAdapter(this, cursos);
                    lvCursos.Adapter = cursosAdapter;
                }
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