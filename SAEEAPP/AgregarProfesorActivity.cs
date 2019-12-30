using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class AgregarProfesorActivity
    {

        AlertDialog.Builder alertDialogBuilder;
        AlertDialog alertDialogAndroid;
        Activity context;
        ListView lvProfesores;
        List<Profesores> profesores;
        EditText etCedula, etNombre, etApellido1, etApellido2, etCorreo, etContrasenia;

        public AgregarProfesorActivity(Activity context, ListView lvProfesores, List<Profesores> profesores)
        {
            this.context = context;
            this.lvProfesores = lvProfesores;
            this.profesores = profesores;
            LayoutInflater layoutInflater = LayoutInflater.From(context);
            View VistaAgregar = layoutInflater.Inflate(Resource.Layout.Dialogo_Agregar_Profesor, null);

            etCedula = VistaAgregar.FindViewById<EditText>(Resource.Id.etCedula);
            etNombre = VistaAgregar.FindViewById<EditText>(Resource.Id.etNombre);
            etApellido1 = VistaAgregar.FindViewById<EditText>(Resource.Id.etPrimerApellido);
            etApellido2 = VistaAgregar.FindViewById<EditText>(Resource.Id.etSegundoApellido);
            etCorreo = VistaAgregar.FindViewById<EditText>(Resource.Id.etCorreo);
            etContrasenia = VistaAgregar.FindViewById<EditText>(Resource.Id.etContrasenia);

            alertDialogBuilder = new AlertDialog.Builder(context)
            .SetView(VistaAgregar)
            .SetPositiveButton("Agregar", (EventHandler<DialogClickEventArgs>)null)
            .SetNegativeButton("Cancelar", (EventHandler<DialogClickEventArgs>)null)
            .SetTitle("Agregando profesor");
            alertDialogAndroid = alertDialogBuilder.Create();
        }

        private void Agregar(object sender, EventArgs e)
        {
            if (EntradaValida())
            {
                AgregarAsync();
            }
        }

        private async Task AgregarAsync()
        {
            ProfesoresServices servicioProfesores = new ProfesoresServices();
            bool resultado = await servicioProfesores.PostAsync(new Profesores()
            {
                Cedula = etCedula.Text,
                Administrador = false,
                Contrasenia = etContrasenia.Text,
                Correo = etCorreo.Text,
                Nombre = etNombre.Text,
                PrimerApellido = etApellido1.Text,
                SegundoApellido = etApellido2.Text
            });

            if (resultado)
            {
                // Se actualiza la lista de profesores
                profesores = await servicioProfesores.GetAsync();
                lvProfesores.Adapter = new ProfesoresListAdapter(context, profesores);

                Toast.MakeText(context, "Agregado correctamente", ToastLength.Long).Show();
                alertDialogAndroid.Dismiss();
            }
            else
            {
                Toast.MakeText(context, "Error al agregar, intente nuevamente", ToastLength.Long).Show();
            }
        }

        private void Cancelar(object sender, EventArgs e)
        {
            alertDialogAndroid.Dismiss();
        }

        private bool EntradaValida()
        {
            if(etCedula.Text.Equals("") || etCedula.Text.StartsWith(" "))
            {
                Toast.MakeText(context, "Ingrese la cédula", ToastLength.Long).Show();
                return false;
            }
            else if (etNombre.Text.Equals("") || etNombre.Text.StartsWith(" "))
            {
                Toast.MakeText(context, "Ingrese el nombre", ToastLength.Long).Show();
                return false;
            }
            else if (etApellido1.Text.Equals("") || etApellido1.Text.StartsWith(" "))
            {
                Toast.MakeText(context, "Ingrese el primer apellido", ToastLength.Long).Show();
                return false;
            }
            else if (etApellido2.Text.Equals("") || etApellido2.Text.StartsWith(" "))
            {
                Toast.MakeText(context, "Ingrese el segundo apellido", ToastLength.Long).Show();
                return false;
            }
            else if (etContrasenia.Text.Equals("") || etContrasenia.Text.StartsWith(" "))
            {
                Toast.MakeText(context, "Ingrese la contraseña", ToastLength.Long).Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Show()
        {
            alertDialogAndroid.Show();
            // Se obtienen los botones para asignarles los métodos nuevos (no cierran el diálogo).
            Button btAgregar = alertDialogAndroid.GetButton((int)DialogButtonType.Positive);
            Button btCancelar = alertDialogAndroid.GetButton((int)DialogButtonType.Negative);

            // Se asignan las funciones
            btAgregar.Click += Agregar;
            btCancelar.Click += Cancelar;
        }
    }
}