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
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{
    [Activity(Label = "ProfesoresActivity")]
    public class ProfesoresActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_profesores);
            ListView lvProfesores = FindViewById<ListView>(Resource.Id.lvProfesores);
            ProfesoresServices servicioProfesores = new ProfesoresServices();
            List<Profesores> profesores = servicioProfesores.Get();
            lvProfesores.Adapter = new ProfesoresListAdapter(this, profesores);

        }
    }
}