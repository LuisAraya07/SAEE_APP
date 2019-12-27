using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using SAEEAPP.Adaptadores;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{
    [Activity(Label = "Profesores", Theme = "@style/AppTheme")]
    public class ProfesoresActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_profesores);
        }

        protected override async void OnStart()
        {
            base.OnStart();
            ListView lvProfesores = FindViewById<ListView>(Resource.Id.lvProfesores);
            TextView tvCargando = FindViewById<TextView>(Resource.Id.tvCargando);
            ProfesoresServices servicioProfesores = new ProfesoresServices();
            List<Profesores> profesores = await servicioProfesores.GetAsync();
            if(profesores.Count == 0)
            {
                tvCargando.Text = "No hay datos";
            }
            else
            {
                tvCargando.Visibility = ViewStates.Gone;
                lvProfesores.Adapter = new ProfesoresListAdapter(this, profesores);
            }
        }
    }
}