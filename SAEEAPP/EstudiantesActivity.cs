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
using Xamarin.core.Services;

namespace SAEEAPP
{
    [Activity(Label = "Estudiantes", Theme = "@style/AppTheme")]
    public class EstudiantesActivity : AppCompatActivity
    {
        private FloatingActionButton fab;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_estudiantes);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fabEstudiante);

        }

        protected override async void OnStart()
        {
            base.OnStart();
            var estudiantesServicio = new EstudiantesServices();
            var estudiantesListView = FindViewById<ListView>(Resource.Id.listViewEstudiantes);
            var estudiantes = await estudiantesServicio.GetAsync(1);
            TextView tvCargando = FindViewById<TextView>(Resource.Id.tvCargandoE);
            

            if (estudiantes.Count == 0)
            {
               
                tvCargando.Text = "No hay datos";
            }
            else
            {
                tvCargando.Visibility = ViewStates.Gone;
                estudiantesListView.Adapter = new ListEstudiantesAdaptador(this, estudiantes);
            }
        }
    }
}