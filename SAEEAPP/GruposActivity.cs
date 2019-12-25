using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SAEEAPP.Adaptadores;
using Xamarin.core.Services;

namespace SAEEAPP
{
    [Activity(Label = "@string/seccionGrupos", Theme = "@style/AppTheme")]
    public class GruposActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_grupos);
            var grupoServicio = new GruposServices();
            var grupoListView = FindViewById<ListView>(Resource.Id.listView);
            var grupos = grupoServicio.Get();
            grupoListView.Adapter = new ListGruposAdaptador(this, grupos);
        }
    }
}