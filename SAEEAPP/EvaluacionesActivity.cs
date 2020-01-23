﻿using Android.App;
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
    [Activity(Label = "Evaluaciones", Theme = "@style/AppTheme")]
    public class EvaluacionesActivity : AppCompatActivity
    {
        Spinner spCurso, spGrupo, spPeriodo, spTipo, spAsignacion;
        Button btVerNotas;
        string rubro;
        int cursoid = 0, grupoid=0,periodo = 0,asignacionid = 0;
        List<Cursos> cursos;
        List<string> cursosNombres, gruposNombres;
        CursosServices cursoServicios;
        EvaluacionesServices servicioEvaluaciones;
        AsignacionesServices servicioAsignaciones;
        List<Grupos> gruposporcurso;
        List<Asignaciones> asignaciones;
        List<int> periodos;
        List<string> asignacionesNombres;
        List<Evaluaciones> evaluaciones;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            periodos = new List<int>();
            cursos = new List<Cursos>();
            asignacionesNombres = new List<string>();
            cursosNombres = new List<string>();
            gruposporcurso = new List<Grupos>();
            gruposNombres = new List<string>();
            cursoServicios = new CursosServices();
            asignaciones = new List<Asignaciones>();
            evaluaciones = new List<Evaluaciones>();
            servicioEvaluaciones = new EvaluacionesServices();
            servicioAsignaciones = new AsignacionesServices();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_evaluaciones);
            cursos = await cursoServicios.GetAsync();
            foreach (Cursos cu in cursos)
            {
                cursosNombres.Add(cu.Nombre);
            }
            /****************************************************************************************************************************************************/
            spCurso = FindViewById<Spinner>(Resource.Id.spCurso);
            spCurso.Prompt = "Elija Curso";
            spCurso.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spCurso_ItemSelected);
            var dataAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, cursosNombres);
            dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spCurso.Adapter = dataAdapter;
            /****************************************************************************************************************************************************/

            spGrupo = FindViewById<Spinner>(Resource.Id.spGrupo);
            /****************************************************************************************************************************************************/
            spPeriodo = FindViewById<Spinner>(Resource.Id.spPeriodo);
            /****************************************************************************************************************************************************/
            spTipo = FindViewById<Spinner>(Resource.Id.spTipo);
            spTipo.Prompt = "Elija Rubro";
            spTipo.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spTipo_ItemSelected);
            var adapter1 = ArrayAdapter.CreateFromResource(this, Resource.Array.listaRubros, Android.Resource.Layout.SimpleSpinnerItem);
            adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spTipo.Adapter = adapter1;
            /****************************************************************************************************************************************************/
            spAsignacion = FindViewById<Spinner>(Resource.Id.spAsignacion);

            /****************************************************************************************************************************************************/
            btVerNotas = FindViewById<Button>(Resource.Id.btVerNotas);
            btVerNotas.Click += VerNotas;
            // Create your application here
        }
        public async void VerNotas(object sender, EventArgs e)
        {
            evaluaciones = new List<Evaluaciones>();
            var evaTemp = await servicioEvaluaciones.GetEvaluacionesxAsignacionAsync(asignacionid);
            foreach(Evaluaciones ev in evaTemp)
            {
                if(ev.Periodo == periodo)
                {
                    evaluaciones.Add(ev);
                }
            }
            Toast.MakeText(this, "Size: " + evaluaciones.Count, ToastLength.Short).Show();
        }
        public async void getgruposCurso(int id)
        {
            gruposporcurso = new List<Grupos>();
            gruposNombres = new List<string>();
            var serviciogrupos = new GruposServices();
            var gruposn = await serviciogrupos.GetAsync();
            var gruposporcursoid = await cursoServicios.GetCursosGruposAsync(id);
            foreach (CursosGrupos cu in gruposporcursoid)
            {
                foreach (Grupos gru in gruposn)
                {
                    if (cu.IdGrupo == gru.Id)
                    {
                        gruposporcurso.Add(gru);
                        gruposNombres.Add(gru.Grupo);
                    }
                }
            }
            spGrupo = FindViewById<Spinner>(Resource.Id.spGrupo);
            spGrupo.Prompt = "Elija Grupo";
            spGrupo.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spGrupo_ItemSelected);
            var dataAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, gruposNombres);
            dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spGrupo.Adapter = dataAdapter;

        }
        private void spTipo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            rubro = spinner.GetItemAtPosition(e.Position).ToString();
            cargarAsignaciones();

        }
        private async void cargarAsignaciones()
        {
            asignacionesNombres = new List<string>();
            asignaciones = new List<Asignaciones>();
            var asigsTemp = await servicioAsignaciones.GetAsync();
            foreach(Asignaciones asig in asigsTemp)
            {
                if (asig.Tipo.Equals(rubro) && asig.Curso == cursoid && asig.Grupo == grupoid)
                {
                    asignaciones.Add(asig);
                    asignacionesNombres.Add(asig.Nombre);
                }
            }
            spAsignacion = FindViewById<Spinner>(Resource.Id.spAsignacion);
            spAsignacion.Prompt = "Elija Asignación";
            spAsignacion.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spAsignacion_ItemSelected);
            var dataAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, asignacionesNombres);
            dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spAsignacion.Adapter = dataAdapter;
        }
        private void spGrupo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            grupoid = gruposporcurso[e.Position].Id;
            //  Toast.MakeText(context, "You choose:" + spinner.GetItemAtPosition(e.Position), ToastLength.Short).Show();
        }
        private void spAsignacion_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            asignacionid = asignaciones[e.Position].Id;
            //  Toast.MakeText(context, "You choose:" + spinner.GetItemAtPosition(e.Position), ToastLength.Short).Show();
        }
        private void spPeriodo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            periodo = int.Parse(spinner.GetItemAtPosition(e.Position).ToString());
        }

        private void spCurso_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            periodos = new List<int>();
            var spinner = sender as Spinner;
            cursoid = cursos[e.Position].Id;
            for(int i = 1;i<= cursos[e.Position].CantidadPeriodos; i++)
            {
                periodos.Add(i);
            }
            spPeriodo.Prompt = "Elija Periodo";
            spPeriodo.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spPeriodo_ItemSelected);
            var dataAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, periodos);
            dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spPeriodo.Adapter = dataAdapter;
            getgruposCurso(cursoid);

            //           Toast.MakeText(context, "Curso: " + spinner.GetItemAtPosition(e.Position)+" ID: "+cursos[e.Position].Id+" GRUPOS EN CURSO : "+ gruposporcurso.Count, ToastLength.Short).Show();
        }
    }
}