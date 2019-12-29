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
    public class AgregarProfesorActivity
    {

        AlertDialog.Builder alertDialogBuilder;
        AlertDialog alertDialogAndroid;
        Context context;
        EditText etCedula, etNombre, etApellido1, etApellido2, etCorreo, etContrasenia;

        public AgregarProfesorActivity(Context context)
        {
            this.context = context;
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
            Toast.MakeText(context, "No cierra solo", ToastLength.Long).Show();
        }

        private void Cancelar(object sender, EventArgs e)
        {
            alertDialogAndroid.Dismiss();
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