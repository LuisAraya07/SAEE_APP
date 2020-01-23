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
using Xamarin.core.Data;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{
    public class CambiarContraseniaActivity
    {
        AlertDialog.Builder alertDialogBuilder;
        AlertDialog alertDialogAndroid;
        Activity context;
        Profesores profesor, profesorTemp;
        EditText etContraseniaActual, etContraseniaNueva, etContraseniaConfirmada;
        Button btCambiar, btCancelar;

        public CambiarContraseniaActivity(Activity context, Profesores profesor)
        {
            this.context = context;
            this.profesor = profesor;
            profesorTemp = new Profesores()
            {
                Cedula = profesor.Cedula,
                Id = profesor.Id,
                Nombre = profesor.Nombre,
                PrimerApellido = profesor.PrimerApellido,
                SegundoApellido = profesor.SegundoApellido,
                Correo = profesor.Correo,
                Contrasenia = profesor.Contrasenia
            };
            LayoutInflater layoutInflater = LayoutInflater.From(context);
            View VistaAgregar = layoutInflater.Inflate(Resource.Layout.Dialogo_Cambiar_Contrasenia, null);
            etContraseniaActual = VistaAgregar.FindViewById<EditText>(Resource.Id.etContraseniaActual);
            etContraseniaNueva = VistaAgregar.FindViewById<EditText>(Resource.Id.etContraseniaNueva);
            etContraseniaConfirmada = VistaAgregar.FindViewById<EditText>(Resource.Id.etContraseniaConfirmada);
            alertDialogBuilder = new AlertDialog.Builder(context, Resource.Style.AlertDialogStyle)
            .SetView(VistaAgregar)
            .SetPositiveButton("Cambiar", (EventHandler<DialogClickEventArgs>)null)
            .SetNegativeButton("Cancelar", (EventHandler<DialogClickEventArgs>)null)
            .SetTitle("Cambiando contraseña");
            alertDialogAndroid = alertDialogBuilder.Create();
        }

        private void Cancelar(object sender, EventArgs e)
        {
            alertDialogAndroid.Dismiss();
        }

        private async void Cambiar(object sender, EventArgs e)
        {
            if (EntradaValida())
            {
                // Se bloquean los botones
                ActivarDesactivarBotones(false);
                Toast.MakeText(context, "Guardando, un momento...", ToastLength.Short).Show();

                ProfesoresServices servicioProfesores = new ProfesoresServices();
                profesorTemp.Contrasenia = etContraseniaNueva.Text;
                bool resultado = await servicioProfesores.UpdatePerfilAsync(profesorTemp);

                if (resultado)
                {
                    ClienteHttp.Usuario.Profesor.Contrasenia = profesorTemp.Contrasenia;
                    ClienteHttp.ActualizarHeaders();
                    // Se asigna lo del temp al profesor
                    profesor.Contrasenia = profesorTemp.Contrasenia;

                    Toast.MakeText(context, "Guardado correctamente", ToastLength.Long).Show();
                    alertDialogAndroid.Dismiss();
                }
                else
                {
                    // Se restablecen los botones
                    ActivarDesactivarBotones(true);
                    Toast.MakeText(context, "Error al guardar, intente nuevamente", ToastLength.Short).Show();
                }
            }
        }

        private bool EntradaValida()
        {
            if (string.IsNullOrWhiteSpace(etContraseniaActual.Text))
            {
                Toast.MakeText(context, "Ingrese la contraseña actual", ToastLength.Short).Show();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(etContraseniaNueva.Text))
            {
                Toast.MakeText(context, "Ingrese la contraseña nueva", ToastLength.Short).Show();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(etContraseniaConfirmada.Text))
            {
                Toast.MakeText(context, "Ingrese la confirmación", ToastLength.Short).Show();
                return false;
            }
            else if (!etContraseniaConfirmada.Text.Equals(etContraseniaNueva.Text))
            {
                Toast.MakeText(context, "Las contraseñas no son iguales", ToastLength.Short).Show();
                return false;
            }
            else if (!etContraseniaActual.Text.Equals(profesor.Contrasenia))
            {
                Toast.MakeText(context, "La contraseña actual no es correcta", ToastLength.Short).Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ActivarDesactivarBotones(bool estado)
        {
            btCambiar.Enabled = estado;
            btCancelar.Enabled = estado;
            alertDialogAndroid.SetCancelable(estado);
        }

        public void Show()
        {
            alertDialogAndroid.Show();
            // Se obtienen los botones para asignarles los métodos nuevos (no cierran el diálogo).
            btCambiar = alertDialogAndroid.GetButton((int)DialogButtonType.Positive);
            btCancelar = alertDialogAndroid.GetButton((int)DialogButtonType.Negative);

            // Se asignan las funciones
            btCambiar.Click += Cambiar;
            btCancelar.Click += Cancelar;
        }
    }
}