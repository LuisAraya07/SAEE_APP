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

namespace SAEEAPP
{
    [Activity(Label = "Menú Principal", MainLauncher = true, Theme = "@style/AppTheme")]
    public class MenuActivity : AppCompatActivity
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
            Button btEstudiantes = FindViewById<Button>(Resource.Id.btEstudiantes);
            btEstudiantes.Click += OnClick_Estudiantes;
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

        public void OnClick_Estudiantes(object sender, EventArgs e)
        {
            Intent estudiantes = new Intent(this, typeof(EstudiantesActivity));
            StartActivity(estudiantes);
        }
    }
}