using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using System;
using Xamarin.core.Data;

namespace SAEEAPP
{
    [Activity(Label = "Menú Principal", Theme = "@style/AppTheme")]
    public class MenuActivity : AppCompatActivity
    {
        Button btUsuario;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_Menu);
            btUsuario = FindViewById<Button>(Resource.Id.btUsuario);
            btUsuario.Click += OnClick_Usuario;
            Button btCursos = FindViewById<Button>(Resource.Id.btCursos);
            btCursos.Click += OnClick_Cursos;
            Button btGrupos = FindViewById<Button>(Resource.Id.btGrupos);
            btGrupos.Click += OnClick_Grupos;
            Button btProfesores = FindViewById<Button>(Resource.Id.btProfesores);
            btProfesores.Click += OnClick_Profesores;
            Button btEstudiantes = FindViewById<Button>(Resource.Id.btEstudiantes);
            btEstudiantes.Click += OnClick_Estudiantes;
            if (!ClienteHttp.Usuario.Profesor.Administrador)
            {
                btProfesores.Visibility = Android.Views.ViewStates.Gone;
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            btUsuario.Text = $"{ClienteHttp.Usuario.Profesor.Nombre} {ClienteHttp.Usuario.Profesor.PrimerApellido}";
        }

        public void OnClick_Usuario(object sender, EventArgs e)
        {
            Intent usuario = new Intent(this, typeof(UsuarioActivity));
            StartActivity(usuario);
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
    }
}