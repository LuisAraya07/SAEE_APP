using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using Newtonsoft.Json;
using SAEEP.ManejoNotificaciones;
using System;
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            etCedula = FindViewById<EditText>(Resource.Id.etCedula);
            etContrasenia = FindViewById<EditText>(Resource.Id.etContrasenia);
            btnIngresar = FindViewById<Button>(Resource.Id.btAceptar);
            pbInicioSesion = FindViewById<ProgressBar>(Resource.Id.pbInicioSesion);
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
                Toast.MakeText(this, "Ingresando", ToastLength.Short).Show();
                pbInicioSesion.Visibility = Android.Views.ViewStates.Visible;

                InicioSesionServices inicioSesionServices = new InicioSesionServices();
                var response = await inicioSesionServices.IniciarSesion(etCedula.Text, etContrasenia.Text);
                ModificarNotificaciones(ClienteHttp.Usuario.Profesor.Id);
                if (response.IsSuccessStatusCode)
                {
                    //Se abre el menu
                    Intent siguiente = new Intent(this, typeof(MenuActivity));
                    StartActivity(siguiente);
                    Toast.MakeText(this, $"¡Bienvenido {ClienteHttp.Usuario.Profesor.Nombre}!", ToastLength.Short).Show();
                    
                }
                else
                {
                    ClienteHttp.Usuario = null;
                    Toast.MakeText(this, $"Cédula o contraseña incorrecta", ToastLength.Short).Show();
                }
                // Se restablecen los controles y se oculta la barra
                pbInicioSesion.Visibility = Android.Views.ViewStates.Invisible;
                ActivarDesactivarControles(true);
                etContrasenia.Text = string.Empty;// Se limpia, ya sea correcta o no
            }
        }
        //Se eliminar temporalmente las notificaciones que no son del profesor ingresado
        private async void ModificarNotificaciones(int id)
        {

            var notificacionIns = new NotificacionesServices("dbNotificaciones.db");
            //Id del profesor
            var listNotasAgregar = await notificacionIns.GetNotificaciones(id);
            var listNotasEliminar = await notificacionIns.GetNotificacionesEliminar(id);
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