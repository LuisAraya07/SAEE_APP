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
    public class AgregarEstudianteActivity 
    {
        AlertDialog.Builder alertDialogBuilder;
        AlertDialog alertDialogAndroid;
        ListEstudiantesAdaptador adaptadorEstudiantes;
        List<Estudiantes> listaEstudiantes = new List<Estudiantes>();
        Activity context;
        EditText etCedula, etNombre, etApellido1, etApellido2, etContrasenia;

        public AgregarEstudianteActivity(Activity context, ListEstudiantesAdaptador adaptadorEstudiantes,List<Estudiantes> listaEstudiantes)
        {
            this.context = context;
            this.adaptadorEstudiantes = adaptadorEstudiantes;
            this.listaEstudiantes = listaEstudiantes;
            LayoutInflater layoutInflater = LayoutInflater.From(context);
            View VistaAgregar = layoutInflater.Inflate(Resource.Layout.Dialogo_Agregar_Estudiante, null);

            etCedula = VistaAgregar.FindViewById<EditText>(Resource.Id.etCedulaE);
            etNombre = VistaAgregar.FindViewById<EditText>(Resource.Id.etNombreE);
            etApellido1 = VistaAgregar.FindViewById<EditText>(Resource.Id.etPrimerApellidoE);
            etApellido2 = VistaAgregar.FindViewById<EditText>(Resource.Id.etSegundoApellidoE);
            etContrasenia = VistaAgregar.FindViewById<EditText>(Resource.Id.etContraseniaE);
            alertDialogBuilder = new AlertDialog.Builder(context)
            .SetView(VistaAgregar)
            .SetPositiveButton("Agregar", (EventHandler<DialogClickEventArgs>)null)
            .SetNegativeButton("Cancelar", (EventHandler<DialogClickEventArgs>)null)
            .SetTitle("Agregando Estudiante");
            alertDialogAndroid = alertDialogBuilder.Create();
        }

        private void Agregar(object sender, EventArgs e)
        {
            if (EntradaValida())
            {
                if (etContrasenia.Text.Equals("") || etContrasenia.Text.StartsWith(" ")) {
                    etContrasenia.Text = GenerarContrasenia();
                }
                AgregarAsync();
            }
        }
        public string GenerarContrasenia()
        {
            Random rdn = new Random();
            string caracteres = "1234567890";
            int longitud = caracteres.Length;
            char letra;
            int longitudContrasenia = 5;
            string contraseniaAleatoria = string.Empty;
            for (int i = 0; i < longitudContrasenia; i++)
            {
                letra = caracteres[rdn.Next(longitud)];
                contraseniaAleatoria += letra.ToString();
            }
            return contraseniaAleatoria;
        }
        private async Task AgregarAsync()
        {
            EstudiantesServices servicioEstudiantes = new EstudiantesServices();

            var resultado = await servicioEstudiantes.PostAsync(new Estudiantes()
            {
                //Obtengo el id del profesor
                IdProfesor = 1,
                Cedula = etCedula.Text,
                Pin = etContrasenia.Text,
                Nombre = etNombre.Text,
                PrimerApellido = etApellido1.Text,
                SegundoApellido = etApellido2.Text
            });
            if (resultado == null)
            {
                Toast.MakeText(context, "Error al agregar, intente nuevamente", ToastLength.Long).Show();
            }
            else
            {
                listaEstudiantes.Add(resultado);
                adaptadorEstudiantes.NotifyDataSetChanged();
                alertDialogBuilder.Dispose();
                Toast.MakeText(context, "Agregado correctamente", ToastLength.Long).Show();
                alertDialogAndroid.Dismiss();
            }
        }

        private void Cancelar(object sender, EventArgs e)
        {
            alertDialogAndroid.Dismiss();
        }

        private bool EntradaValida()
        {
            if (etCedula.Text.Equals("") || etCedula.Text.StartsWith(" "))
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
