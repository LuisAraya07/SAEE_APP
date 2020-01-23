using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using Newtonsoft.Json;
using SAEEP.ManejoNotificaciones;
using System;
using System.Threading.Tasks;
using Xamarin.core;
using Xamarin.core.Data;
using Xamarin.core.Models;
using Xamarin.core.OfflineServices;
using Xamarin.core.Services;

namespace SAEEAPP
{
    [Activity(Label = "@string/app_name",  Theme = "@style/AppTheme",MainLauncher =true)]
    public class MainActivity : AppCompatActivity
    {
        private EditText etCedula;
        private EditText etContrasenia;
        private Button btnIngresar;
        private ProgressBar pbInicioSesion;
        private CheckBox cbOffline;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            etCedula = FindViewById<EditText>(Resource.Id.etCedula);
            etContrasenia = FindViewById<EditText>(Resource.Id.etContrasenia);
            btnIngresar = FindViewById<Button>(Resource.Id.button1);
            pbInicioSesion = FindViewById<ProgressBar>(Resource.Id.pbInicioSesion);
            cbOffline = FindViewById<CheckBox>(Resource.Id.checkBox1);
            btnIngresar.Click += OnClick_Ingresar;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public async void OnClick_Ingresar(object sender, EventArgs e)
        {
            etCedula.Text = "701110111";
            etContrasenia.Text = "12";
            if (EntradaValida())
            {
                // Se bloquean los controles y se activa el progress bar
                ActivarDesactivarControles(false);
                //Toast.MakeText(this, "Ingresando", ToastLength.Short).Show();
                pbInicioSesion.Visibility = Android.Views.ViewStates.Visible;

                InicioSesionServices inicioSesionServices = new InicioSesionServices();
                VerificarConexion vc = new VerificarConexion(this);
                var conectado = vc.IsOnline();
                if (conectado)
                {
                    
                    var response = await inicioSesionServices.IniciarSesion(etCedula.Text, etContrasenia.Text);
                    if (response.IsSuccessStatusCode)
                    {
                        var finalizadoNotificaciones = await ModificarNotificaciones(ClienteHttp.Usuario.Profesor);
                        //Se abre el menu
                        if (finalizadoNotificaciones)
                        {
                            Intent siguiente = new Intent(this, typeof(MenuActivity));
                            StartActivity(siguiente);
                            Toast.MakeText(this, $"¡Bienvenido {ClienteHttp.Usuario.Profesor.Nombre}!", ToastLength.Long).Show();

                        }

                    }
                    else
                    {
                        ClienteHttp.Usuario = null;
                        Toast.MakeText(this, $"Cédula o contraseña incorrecta", ToastLength.Short).Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, "Necesita conexión a internet.", ToastLength.Long).Show();
                }
                // Se restablecen los controles y se oculta la barra
                pbInicioSesion.Visibility = Android.Views.ViewStates.Invisible;
                ActivarDesactivarControles(true);
                etContrasenia.Text = string.Empty;// Se limpia, ya sea correcta o no
            }
        }
        //Se eliminar temporalmente las notificaciones que no son del profesor ingresado
        private async Task<Boolean> ModificarNotificaciones(Profesores profesor)
        {

            var notificacionIns = new NotificacionesServices();
            
            //Id del profesor
            var listNotasAgregar = await notificacionIns.GetNotificaciones(profesor.Id);
            var listNotasEliminar = await notificacionIns.GetNotificacionesEliminar(profesor.Id);
            foreach (Notificaciones nota in listNotasEliminar)
            {
                if (DateTime.Now < Convert.ToDateTime(nota.Date))
                {
                    new CrearEliminarNotificaciones(this, nota, false);

                }
                nota.Estado = 0;
                notificacionIns.PutNotificaciones(nota);
            }
            foreach (Notificaciones nota in listNotasAgregar)
            {
                if (DateTime.Now < Convert.ToDateTime(nota.Date))
                {
                    new CrearEliminarNotificaciones(this, nota, true);
                    nota.Estado = 1;
                    notificacionIns.PutNotificaciones(nota);
                }
                else
                {
                    nota.Estado = 0;
                    notificacionIns.PutNotificaciones(nota);
                }
            }
            //Verifico si le dio check
            if (cbOffline.Checked)
            {
                Toast.MakeText(this, "Sincronizando Datos, espere...", ToastLength.Long).Show();
                //Cargamos todos los datos
                //Aquí agregamos al profesor conectado
                var rs = await CargarBDLocal(profesor);
                if (rs) return true;
                Toast.MakeText(this, "No todas las funcionalidades están disponibles.", ToastLength.Short).Show();

            }
            return true;
        }

        private async Task<Boolean> CargarBDLocal(Profesores profesor)
        {
            var idProfesor = profesor.Id;
            GruposServices gruposServicio = new GruposServices();
            GruposServices gruposServicioOffline = new GruposServices(idProfesor);
            CursosServices cursosServicio = new CursosServices();
            CursosServices cursosServicioOffline = new CursosServices(idProfesor);
            ProfesoresServices profesoresServicioOffline = new ProfesoresServices(idProfesor);
            EstudiantesServices estudiantesServicio = new EstudiantesServices();
            EstudiantesServices estudiantesServicioOffline = new EstudiantesServices(idProfesor);

            //Guardamos el profesor en local
            await profesoresServicioOffline.PostOffline(profesor);

            //Agregar Estudiantes
            var listaEstudiantes = await estudiantesServicio.GetAsync();
            await estudiantesServicioOffline.PostAllOffline(listaEstudiantes);

            //Agregar grupos
            var listaGrupos = await gruposServicio.GetAsync();
            await gruposServicioOffline.PostAllOffline(listaGrupos);

            //Agregar cursos
            var listaCursos = await cursosServicio.GetAsync();
            await cursosServicioOffline.PostAllOffline(listaCursos);

            //Agregar CursosGrupos
            //var listaCursosGrupos = await cursosServicio.GetCursosGruposAllAsync();
            //await cursosServicioOffline.AgregarCursosGruposAllOffline(listaCursosGrupos);
            ////var listaProfesores = await profesoresServicio.GetAsync();

            //Agregar EG
            //   var listaEG = await gruposServicio.GetAllEGAsync();
            // await gruposServicioOffline.PostAllEGOffline(listaEG);
            return true;
        }

        private bool EntradaValida()
        {
            if (string.IsNullOrWhiteSpace(etCedula.Text))
            {
                Toast.MakeText(this, "Ingrese la cédula", ToastLength.Short).Show();
                return false;
            }
            if (string.IsNullOrWhiteSpace(etContrasenia.Text))
            {
                Toast.MakeText(this, "Ingrese la contraseña", ToastLength.Short).Show();
                return false;
            }
            return true;
        }

        private void ActivarDesactivarControles(bool estado)
        {
            etCedula.Enabled = estado;
            etContrasenia.Enabled = estado;
            btnIngresar.Enabled = estado;
        }
    }
}