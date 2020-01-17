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
using Xamarin.core.Data;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{
    [Activity(Label = "Administrar perfil", Theme = "@style/AppTheme")]
    public class UsuarioActivity : AppCompatActivity
    {
        EditText etCedula, etNombre, etPrimerApellido, etSegundoApellido, etCorreo;
        Button btEditar, btCancelar, btCambiarContrasenia;
        Profesores profesor;
        bool editando = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_usuario);
            profesor = new Profesores()
            {
                Id = ClienteHttp.Usuario.Profesor.Id,
                Cedula = ClienteHttp.Usuario.Profesor.Cedula,
                Nombre = ClienteHttp.Usuario.Profesor.Nombre,
                PrimerApellido = ClienteHttp.Usuario.Profesor.PrimerApellido,
                SegundoApellido = ClienteHttp.Usuario.Profesor.SegundoApellido,
                Correo = ClienteHttp.Usuario.Profesor.Correo,
                Contrasenia = ClienteHttp.Usuario.Profesor.Contrasenia
            };
            // Se obtienen los elementos
            etCedula = FindViewById<EditText>(Resource.Id.etCedula);
            etNombre = FindViewById<EditText>(Resource.Id.etNombre);
            etPrimerApellido = FindViewById<EditText>(Resource.Id.etPrimerApellido);
            etSegundoApellido = FindViewById<EditText>(Resource.Id.etSegundoApellido);
            etCorreo = FindViewById<EditText>(Resource.Id.etCorreo);
            btEditar = FindViewById<Button>(Resource.Id.btEditar);
            btCancelar = FindViewById<Button>(Resource.Id.btCancelar);
            btCambiarContrasenia = FindViewById<Button>(Resource.Id.btCambiarContrasenia);
            // Se asignan los valores
            AsignarValoresUsuario();
            // Se asignan las funciones
            btEditar.Click += BtEditar_Click;
            btCancelar.Click += BtCancelar_Click;
            btCambiarContrasenia.Click += BtCambiarContrasenia_Click;
        }

        protected override void OnStart()
        {
            base.OnStart();
        }


        private async void BtEditar_Click(object sender, EventArgs e)
        {
            if (editando) // Si está editando, hay que guardar la información
            {
                if (EntradaValida())
                {
                    //Bloquear botones
                    ActivarDesactivarBotones(false);
                    // Guardar información...
                    Toast.MakeText(this, "Guardando, espere...", ToastLength.Short).Show();
                    // Se utiliza esta clase en caso de que falle, para restaurar los valores
                    profesor.Nombre = etNombre.Text;
                    profesor.PrimerApellido = etPrimerApellido.Text;
                    profesor.SegundoApellido = etSegundoApellido.Text;
                    profesor.Correo = etCorreo.Text;
                    // La contraseña se asigna en el dialogo de cambio de contraseña
                    ProfesoresServices servicioProfesores = new ProfesoresServices();
                    bool respuesta = await servicioProfesores.UpdatePerfilAsync(profesor);
                    if (respuesta)
                    {
                        ClienteHttp.Usuario.Profesor.Nombre = profesor.Nombre;
                        ClienteHttp.Usuario.Profesor.PrimerApellido = profesor.PrimerApellido;
                        ClienteHttp.Usuario.Profesor.SegundoApellido = profesor.SegundoApellido;
                        ClienteHttp.Usuario.Profesor.Correo = profesor.Correo;
                        CambiarEstadoBotones(false);
                        editando = false;
                        Toast.MakeText(this, "Guardado correctamente", ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "Error al guardar", ToastLength.Short).Show();
                    }
                    ActivarDesactivarBotones(true);
                }
            }
            else
            {
                CambiarEstadoBotones(true);
                editando = true;
            }
        }

        private void BtCancelar_Click(object sender, EventArgs e)
        {
            editando = false;
            CambiarEstadoBotones(false);
            // Se restablecen los valores
            AsignarValoresUsuario();
        }

        private void BtCambiarContrasenia_Click(object sender, EventArgs e)
        {
            CambiarContraseniaActivity cambiarContrasenia =
                new CambiarContraseniaActivity(this, profesor);
            cambiarContrasenia.Show();
        }

        private void AsignarValoresUsuario()
        {
            etCedula.Text = ClienteHttp.Usuario.Profesor.Cedula;
            etNombre.Text = ClienteHttp.Usuario.Profesor.Nombre;
            etPrimerApellido.Text = ClienteHttp.Usuario.Profesor.PrimerApellido;
            etSegundoApellido.Text = ClienteHttp.Usuario.Profesor.SegundoApellido;
            etCorreo.Text = ClienteHttp.Usuario.Profesor.Correo;
        }

        private void CambiarEstadoBotones(bool estado)
        {
            etNombre.Enabled = estado;
            etPrimerApellido.Enabled = estado;
            etSegundoApellido.Enabled = estado;
            etCorreo.Enabled = estado;
            if (estado) // Si es true, va a editar
            {
                btEditar.Text = "Guardar";
                btCambiarContrasenia.Visibility = ViewStates.Gone;
                btCancelar.Visibility = ViewStates.Visible;
            }
            else
            {
                btEditar.Text = "Editar";
                btCambiarContrasenia.Visibility = ViewStates.Visible;
                btCancelar.Visibility = ViewStates.Gone;
            }
        }

        private bool EntradaValida()
        {
            if (string.IsNullOrWhiteSpace(etNombre.Text))
            {
                Toast.MakeText(this, "Ingrese el nombre", ToastLength.Short).Show();
                return false;
            }

            if (string.IsNullOrWhiteSpace(etPrimerApellido.Text))
            {
                Toast.MakeText(this, "Ingrese el primer apellido", ToastLength.Short).Show();
                return false;
            }

            if (string.IsNullOrWhiteSpace(etSegundoApellido.Text))
            {
                Toast.MakeText(this, "Ingrese el segundo apellido", ToastLength.Short).Show();
                return false;
            }

            if (string.IsNullOrWhiteSpace(etCorreo.Text))
            {
                Toast.MakeText(this, "Ingrese el correo", ToastLength.Short).Show();
                return false;
            }

            return true;
        }

        private void ActivarDesactivarBotones(bool estado)
        {
            btEditar.Enabled = estado;
            btCancelar.Enabled = estado;
        }
    }
}