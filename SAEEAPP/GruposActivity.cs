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
    [Activity(Label = "Grupos", Theme = "@style/AppTheme")]
    public class GruposActivity : AppCompatActivity
    {
        private FloatingActionButton fab;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_grupos);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += delegate
            {
                //Toast.MakeText(this,"Dialogo agregar",ToastLength.Long).Show();
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                View mView = layoutInflater.Inflate(Resource.Layout.Dialogo_Agregar_Grupos,null);
                Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(this);
                alertDialogBuilder.SetView(mView);
                var txtGrupo = mView.FindViewById<EditText>(Resource.Id.etGrupo);
                alertDialogBuilder.SetCancelable(false)
                .SetPositiveButton("Agregar",delegate {
                    Toast.MakeText(this, "Grupo: "+txtGrupo.Text, ToastLength.Long).Show();
                   // Onclick();
                })
                .SetNegativeButton("Cancelar",delegate {
                    alertDialogBuilder.Dispose();
                
                });
                Android.Support.V7.App.AlertDialog alertDialogAndroid = alertDialogBuilder.Create();
                alertDialogAndroid.Show();


            };
        }

        private void Onclick()
        {
            GruposServices gruposServices = new GruposServices();
            Grupos grupo =
            new Grupos()
            {
                IdProfesor = 1,
                Anio = 2019,
                Grupo = "7-7"

            };
            EstudiantesXgrupos EG = new EstudiantesXgrupos() {
                
               IdProfesor = 1,
               IdGrupo = 4,
               IdEstudiante = 3

            };
            grupo.EstudiantesXgrupos.Add (EG);
            gruposServices.PostAsync( grupo);

        }

        protected override async void OnStart()
        {
            base.OnStart();
            var grupoServicio = new GruposServices();
            var grupoListView = FindViewById<ListView>(Resource.Id.listView);
            var grupos = await grupoServicio.GetAsync(1);
            TextView tvCargando = FindViewById<TextView>(Resource.Id.tvCargandoG);
            TextView tvAnio = FindViewById<TextView>(Resource.Id.textViewAnio2);
            TextView tvGrupo = FindViewById<TextView>(Resource.Id.textViewGrupos2);
            TextView tvOpciones = FindViewById<TextView>(Resource.Id.textViewOpciones);

            if (grupos.Count == 0)
            {
                tvAnio.Visibility = ViewStates.Gone;
                tvOpciones.Visibility = ViewStates.Gone;
                tvGrupo.Visibility = ViewStates.Gone;
                tvCargando.Text = "No hay datos";
            }
            else
            {
                tvCargando.Visibility = ViewStates.Gone;
                grupoListView.Adapter = new ListGruposAdaptador(this, grupos);
            }
        }
    }
}