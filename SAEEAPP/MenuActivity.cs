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

namespace SAEEAPP
{
    [Activity(Label = "MenuActivity")]
    public class MenuActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_Menu);
            Button btGrupos = FindViewById<Button>(Resource.Id.btGrupos);
            btGrupos.Click += OnClick_Grupos;
            Button btProfesores = FindViewById<Button>(Resource.Id.btProfesores);
            btProfesores.Click += OnClick_Profesores;
        }

        public void OnClick_Grupos(object sender, EventArgs e)
        {
            Intent grupos = new Intent(this, typeof(GruposActivity));
            StartActivity(grupos);
        }

        public void OnClick_Profesores(object sender, EventArgs e)
        {
            Intent profesores = new Intent(this, typeof(ProfesoresActivity));
            StartActivity(profesores);
        }
    }
}