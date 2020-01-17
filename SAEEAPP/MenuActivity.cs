using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using System;

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
            Button btCursos = FindViewById<Button>(Resource.Id.btCursos);
            btCursos.Click += OnClick_Cursos;
            Button btGrupos = FindViewById<Button>(Resource.Id.btGrupos);
            btGrupos.Click += OnClick_Grupos;
            Button btProfesores = FindViewById<Button>(Resource.Id.btProfesores);
            btProfesores.Click += OnClick_Profesores;
            Button btEstudiantes = FindViewById<Button>(Resource.Id.btEstudiantes);
            btEstudiantes.Click += OnClick_Estudiantes;
            //Button btNotificaciones = FindViewById<Button>(Resource.Id.btNotificaciones);
            //btNotificaciones.Click += OnClick_Notificaciones;
        }

        public void OnClick_Cursos(object sender, EventArgs e)
        {
            Intent cursos = new Intent(this, typeof(CursosActivity));
            StartActivity(cursos);
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

        public void OnClick_Notificaciones(object sender, EventArgs e)
        {
            //Intent notificaciones = new Intent(this, typeof(NotificacionesActivity));
            //StartActivity(notificaciones);
        }
    }
}