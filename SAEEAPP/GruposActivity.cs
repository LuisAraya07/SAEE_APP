﻿using Android.App;
using Android.Net;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SAEEAPP.Adaptadores;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.core.Models;
using Xamarin.core.Services;
using Android.Content;
using Xamarin.core;
using Xamarin.core.Data;
using Xamarin.core.OfflineServices;

namespace SAEEAPP
{
    [Activity(Label = "@string/grupo", Theme = "@style/AppTheme")]
    public class GruposActivity : AppCompatActivity
    {
        private FloatingActionButton fab;
        private List<Grupos> listaGrupos = new List<Grupos>();
        ListGruposAdaptador adaptadorGrupos;
        ListView grupoListView;
        ProgressBar pbCargandoGrupos;
        TextView tvCargando;
        VerificarConexion vc; 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_grupos);
            grupoListView = FindViewById<ListView>(Resource.Id.listView);
            tvCargando = FindViewById<TextView>(Resource.Id.tvCargandoG);
            pbCargandoGrupos = FindViewById<ProgressBar>(Resource.Id.pbCargandoGrupos);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            vc  = new VerificarConexion(this);
            fab.Visibility = ViewStates.Invisible;
            fab.Click += delegate
            {
                //Toast.MakeText(this,"Dialogo agregar",ToastLength.Long).Show();
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                View mView = layoutInflater.Inflate(Resource.Layout.Dialogo_Agregar_Grupos, null);
                Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AlertDialogStyle);
                alertDialogBuilder.SetView(mView);
                var etGrupo = mView.FindViewById<EditText>(Resource.Id.etGrupo);
                alertDialogBuilder.SetTitle("Agregando Grupo");
                alertDialogBuilder.SetCancelable(false)
                .SetPositiveButton("Agregar", delegate
                {
                    
                    AgregarGrupoAsync(alertDialogBuilder, etGrupo.Text);
                })
                .SetNegativeButton("Cancelar", delegate
                {
                    alertDialogBuilder.Dispose();

                });
                Android.Support.V7.App.AlertDialog alertDialogAndroid = alertDialogBuilder.Create();
                alertDialogAndroid.Show();


            };
        }

        private async void AgregarGrupoAsync(Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder, string etGrupo)
        {
            DateTime fechaActual = DateTime.Today;
            GruposServices gruposServices;
            if (!etGrupo.Equals("") && !etGrupo.StartsWith(" "))
            {
                Grupos grupo =
                new Grupos()
                {
                    
                    Anio = fechaActual.Year,
                    Grupo = etGrupo

                };
                //Verificar conexión
                
                var conectado = vc.IsOnline();
                if (conectado)
                {
                    gruposServices = new GruposServices();
                    HttpResponseMessage resultado = await gruposServices.PostAsync(grupo);
                    if (resultado.IsSuccessStatusCode)
                    {
                        string resultadoString = await resultado.Content.ReadAsStringAsync();
                        var grupoNuevo = JsonConvert.DeserializeObject<Grupos>(resultadoString);
                        listaGrupos.Add(grupoNuevo);
                        adaptadorGrupos.ActualizarDatos();
                        Toast.MakeText(this, "Se ha agregado con éxito.", ToastLength.Long).Show();
                        alertDialogBuilder.Dispose();

                    }
                    else
                    {
                        Toast.MakeText(this, "Error al agregar, intente nuevamente", ToastLength.Long).Show();
                    }

                }
                else {
                    //Toast.MakeText(this, "Necesita conexión a internet.", ToastLength.Long).Show();
                    ProfesoresServices ns = new ProfesoresServices(1);
                    Profesores profesor = await ns.GetProfesorConectado();
                    if (!(profesor == null))
                    {
                        gruposServices = new GruposServices(profesor.Id);
                        var grupoNuevo = await gruposServices.PostOffline(grupo);
                        listaGrupos.Add(grupoNuevo);
                        adaptadorGrupos.ActualizarDatos();
                        Toast.MakeText(this, "Se ha agregado con éxito.", ToastLength.Long).Show();
                        alertDialogBuilder.Dispose();
                    }
                    else
                    {
                        Toast.MakeText(this, "No hay bases de datos local.", ToastLength.Long).Show();
                        alertDialogBuilder.Dispose();
                    
                    }
                    
                }
                

            }
            else
            {
                Toast.MakeText(this, "Debe ingresar un grupo.", ToastLength.Long).Show();
            }

        }
       
        protected override async void OnStart()
        {
            base.OnStart();
            //Verificamos conexion
            var conectado = vc.IsOnline();
            GruposServices grupoServicio;
            if (conectado)
            {
                grupoServicio = new GruposServices();
                listaGrupos = await grupoServicio.GetAsync();
            }
            else
            {
                ProfesoresServices ns = new ProfesoresServices(1);
                Profesores profesor =await ns.GetProfesorConectado();
                if (!(profesor == null))
                {
                    grupoServicio = new GruposServices(profesor.Id);
                    listaGrupos = await grupoServicio.GetOffline();
                }
                else
                {
                    Toast.MakeText(this, "No hay base de datos local.", ToastLength.Long).Show();
                }
                
            }


            if (listaGrupos.Count > 0)
            {

                tvCargando.Visibility = ViewStates.Invisible;
            }
            adaptadorGrupos = new ListGruposAdaptador(this, listaGrupos, tvCargando);
            pbCargandoGrupos.Visibility = ViewStates.Gone;
            grupoListView.Adapter = adaptadorGrupos;
            fab.Visibility = ViewStates.Visible;
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main2, menu);
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