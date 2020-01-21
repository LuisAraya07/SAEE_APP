using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using System;
using Xamarin.core;
using Xamarin.core.Data;

namespace SAEEAPP
{
    [Activity(Label = "Menú Principal",  Theme = "@style/AppTheme")]
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
            Button btNotificaciones = FindViewById<Button>(Resource.Id.btNotificaciones);
            btNotificaciones.Click += OnClick_Notificaciones;


            //nuevo
            Button btAsignaciones = FindViewById<Button>(Resource.Id.btAsignaciones);
            btAsignaciones.Click += OnClick_Asignaciones;


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
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado) {
                Intent usuario = new Intent(this, typeof(UsuarioActivity));
                StartActivity(usuario);
            }
            else
            {
                Toast.MakeText(this,"Necesita conexión a internet.",ToastLength.Long).Show();
            }
                
        }


        //NUEVO ASIGNACIONES
        public void OnClick_Asignaciones(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                Intent asignacion = new Intent(this, typeof(AsignacionesActivity));
                StartActivity(asignacion);
            }
            else
            {
                Toast.MakeText(this, "Necesita conexión a internet.", ToastLength.Long).Show();
            }

        }

        public void OnClick_Cursos(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                Intent cursos = new Intent(this, typeof(CursosActivity));
                StartActivity(cursos);
            }
            else
            {
                Toast.MakeText(this, "Necesita conexión a internet.", ToastLength.Long).Show();
            }
            
        }

        public void OnClick_Grupos(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                Intent grupos = new Intent(this, typeof(GruposActivity));
                StartActivity(grupos);
            }
            else
            {
                Toast.MakeText(this, "Necesita conexión a internet.", ToastLength.Long).Show();
            }
            
        }

        public void OnClick_Profesores(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                Intent profesores = new Intent(this, typeof(ProfesoresActivity));
                StartActivity(profesores);
            }
            else
            {
                Toast.MakeText(this, "Necesita conexión a internet.", ToastLength.Long).Show();
            }
            
        }

        public void OnClick_Estudiantes(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                Intent estudiantes = new Intent(this, typeof(EstudiantesActivity));
                StartActivity(estudiantes);
            }
            else
            {
                Toast.MakeText(this, "Necesita conexión a internet.", ToastLength.Long).Show();
            }
            
        }

        public void OnClick_Notificaciones(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                Intent notificaciones = new Intent(this, typeof(NotificacionesActivity));
                StartActivity(notificaciones);
            }
            else
            {
                Toast.MakeText(this, "Necesita conexión a internet.", ToastLength.Long).Show();
            }
            
        }
    }
}