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
using Xamarin.core.Services;

namespace SAEEAPP
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class GruposActivity : AppCompatActivity
    {
        private FloatingActionButton fab;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_grupos);
            var grupoServicio = new GruposServices();
            var grupoListView = FindViewById<ListView>(Resource.Id.listView);
            var grupos = grupoServicio.Get(1);
            grupoListView.Adapter = new ListGruposAdaptador(this, grupos);
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

                })
                .SetNegativeButton("Cancelar",delegate {
                    alertDialogBuilder.Dispose();
                
                });
                Android.Support.V7.App.AlertDialog alertDialogAndroid = alertDialogBuilder.Create();
                alertDialogAndroid.Show();


            };
        }
    }
}