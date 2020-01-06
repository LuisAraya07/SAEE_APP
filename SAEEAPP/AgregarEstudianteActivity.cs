using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SAEEAPP.Adaptadores;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{
    public class AgregarEstudianteActivity
    {
        AlertDialog.Builder alertDialogBuilder;
        AlertDialog alertDialogAndroid;
        ListEstudiantesAdaptador adaptadorEstudiantes;
        List<Estudiantes> listaEstudiantes;
        Estudiantes _estudiante;
        Activity context;
        EditText etCedula, etNombre, etApellido1, etApellido2, etContrasenia;
        private readonly bool editando;
        public AgregarEstudianteActivity(Activity context, ListEstudiantesAdaptador adaptadorEstudiantes, List<Estudiantes> listaEstudiantes)
        {
            InicializarValores(context, adaptadorEstudiantes, listaEstudiantes, "Agregando estudiante", "Agregar");
            editando = false;
            _estudiante = null;
        }
        public AgregarEstudianteActivity(Activity context, ListEstudiantesAdaptador adaptadorEstudiantes,
            List<Estudiantes> listaEstudiantes, Estudiantes estudiante)
        {
            InicializarValores(context, adaptadorEstudiantes, listaEstudiantes, "Editando estudiante", "Guardar");
            editando = true;
            etCedula.Text = estudiante.Cedula;
            etNombre.Text = estudiante.Nombre;
            etApellido1.Text = estudiante.PrimerApellido;
            etApellido2.Text = estudiante.SegundoApellido;
            etContrasenia.Text = estudiante.Pin;
            _estudiante = estudiante;
        }
        private async void Agregar(object sender, EventArgs e)
        {
            if (EntradaValida())
            {
                if (etContrasenia.Text.Equals("") || etContrasenia.Text.StartsWith(" "))
                {
                    etContrasenia.Text = GenerarContrasenia();
                }
                await AgregarAsync();
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
        private void InicializarValores(Activity context, ListEstudiantesAdaptador estudiantesAdapter,
            List<Estudiantes> estudiantes, string titulo, string textoBotonConfirmacion)
        {
            this.context = context;
            this.adaptadorEstudiantes = estudiantesAdapter;
            this.listaEstudiantes = estudiantes;
            LayoutInflater layoutInflater = LayoutInflater.From(context);
            View VistaAgregar = layoutInflater.Inflate(Resource.Layout.Dialogo_Agregar_Estudiante, null);
            etCedula = VistaAgregar.FindViewById<EditText>(Resource.Id.etCedulaE);
            etNombre = VistaAgregar.FindViewById<EditText>(Resource.Id.etNombreE);
            etApellido1 = VistaAgregar.FindViewById<EditText>(Resource.Id.etPrimerApellidoE);
            etApellido2 = VistaAgregar.FindViewById<EditText>(Resource.Id.etSegundoApellidoE);
            etContrasenia = VistaAgregar.FindViewById<EditText>(Resource.Id.etContraseniaE);
            alertDialogBuilder = new AlertDialog.Builder(context, Resource.Style.AlertDialogStyle)
            .SetView(VistaAgregar)
            .SetPositiveButton(textoBotonConfirmacion, (EventHandler<DialogClickEventArgs>)null)
            .SetNegativeButton("Cancelar", (EventHandler<DialogClickEventArgs>)null)
            .SetTitle(titulo);
            alertDialogAndroid = alertDialogBuilder.Create();
        }


        private async Task AgregarAsync()
        {
            EstudiantesServices servicioEstudiantes = new EstudiantesServices();
            Estudiantes estudiante = new Estudiantes()
            {
                //Obtengo el id del profesor
                IdProfesor = 1,
                Cedula = etCedula.Text,
                Pin = etContrasenia.Text,
                Nombre = etNombre.Text,
                PrimerApellido = etApellido1.Text,
                SegundoApellido = etApellido2.Text
            };
            HttpResponseMessage resultado = await servicioEstudiantes.PostAsync(estudiante);
            if (resultado.IsSuccessStatusCode)
            {
                string resultadoString = await resultado.Content.ReadAsStringAsync();
                var estudianteNuevo = JsonConvert.DeserializeObject<Estudiantes>(resultadoString);
                listaEstudiantes.Add(estudianteNuevo);
                adaptadorEstudiantes.NotifyDataSetChanged();
                alertDialogBuilder.Dispose();
                Toast.MakeText(context, "Agregado correctamente", ToastLength.Long).Show();
                alertDialogAndroid.Dismiss();
            }
            else
            {
                Toast.MakeText(context, "Error al agregar, intente nuevamente", ToastLength.Long).Show();
            }
        }
        private void Editar(object sender, EventArgs e)
        {
            if (EntradaValida())
            {
                EditarAsync();
            }
        }
        private void Cancelar(object sender, EventArgs e)
        {
            alertDialogAndroid.Dismiss();
        }
        private async Task EditarAsync()
        {
            EstudiantesServices servicioEstudiantes = new EstudiantesServices();
            _estudiante.Cedula = etCedula.Text;
            _estudiante.Nombre = etNombre.Text;
            _estudiante.PrimerApellido = etApellido1.Text;
            _estudiante.SegundoApellido = etApellido2.Text;
            _estudiante.Pin = etContrasenia.Text;
            bool resultado = await servicioEstudiantes.PutAsync(_estudiante);

            if (resultado)
            {
                // Se actualiza la lista de profesores
                adaptadorEstudiantes.ActualizarDatos();

                Toast.MakeText(context, "Guardado correctamente", ToastLength.Long).Show();
                alertDialogAndroid.Dismiss();
            }
            else
            {
                Toast.MakeText(context, "Error al guardar, intente nuevamente", ToastLength.Long).Show();
            }
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
            Button btAgregarEditar = alertDialogAndroid.GetButton((int)DialogButtonType.Positive);
            Button btCancelar = alertDialogAndroid.GetButton((int)DialogButtonType.Negative);

            // Se asignan las funciones
            if (editando)
            {
                btAgregarEditar.Click += Editar;
            }
            else
            {
                btAgregarEditar.Click += Agregar;
            }
            btCancelar.Click += Cancelar;
        }
    }
}
