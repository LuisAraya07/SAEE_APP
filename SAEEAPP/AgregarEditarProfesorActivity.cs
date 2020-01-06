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
    public class AgregarEditarProfesorActivity
    {

        AlertDialog.Builder alertDialogBuilder;
        AlertDialog alertDialogAndroid;
        Activity context;
        ProfesoresListAdapter profesoresAdapter;
        List<Profesores> profesores;
        readonly Profesores profesor;
        EditText etCedula, etNombre, etApellido1, etApellido2, etCorreo, etContrasenia;
        private readonly bool editando;

        public AgregarEditarProfesorActivity(Activity context, ProfesoresListAdapter profesoresAdapter, List<Profesores> profesores)
        {
            InicializarValores(context, profesoresAdapter, profesores, "Agregando profesor", "Agregar");
            editando = false;
            profesor = null;
        }

        public AgregarEditarProfesorActivity(Activity context, ProfesoresListAdapter profesoresAdapter,
            List<Profesores> profesores, Profesores profesor)
        {
            InicializarValores(context, profesoresAdapter, profesores, "Editando profesor", "Guardar");
            editando = true;
            etCedula.Text = profesor.Cedula;
            etNombre.Text = profesor.Nombre;
            etApellido1.Text = profesor.PrimerApellido;
            etApellido2.Text = profesor.SegundoApellido;
            etCorreo.Text = profesor.Correo;
            etContrasenia.Text = profesor.Contrasenia;
            this.profesor = profesor;
        }

        private void InicializarValores(Activity context, ProfesoresListAdapter profesoresAdapter,
            List<Profesores> profesores, string titulo, string textoBotonConfirmacion)
        {
            this.context = context;
            this.profesoresAdapter = profesoresAdapter;
            this.profesores = profesores;
            LayoutInflater layoutInflater = LayoutInflater.From(context);
            View VistaAgregar = layoutInflater.Inflate(Resource.Layout.Dialogo_Agregar_Profesor, null);
            etCedula = VistaAgregar.FindViewById<EditText>(Resource.Id.etCedula);
            etNombre = VistaAgregar.FindViewById<EditText>(Resource.Id.etNombre);
            etApellido1 = VistaAgregar.FindViewById<EditText>(Resource.Id.etPrimerApellido);
            etApellido2 = VistaAgregar.FindViewById<EditText>(Resource.Id.etSegundoApellido);
            etCorreo = VistaAgregar.FindViewById<EditText>(Resource.Id.etCorreo);
            etContrasenia = VistaAgregar.FindViewById<EditText>(Resource.Id.etContrasenia);
            alertDialogBuilder = new AlertDialog.Builder(context, Resource.Style.AlertDialogStyle)
            .SetView(VistaAgregar)
            .SetPositiveButton(textoBotonConfirmacion, (EventHandler<DialogClickEventArgs>)null)
            .SetNegativeButton("Cancelar", (EventHandler<DialogClickEventArgs>)null)
            .SetTitle(titulo);
            alertDialogAndroid = alertDialogBuilder.Create();
        }

        private void Agregar(object sender, EventArgs e)
        {
            if (EntradaValida())
            {
#pragma warning disable CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
                AgregarAsync();
#pragma warning restore CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
            }
        }

        private async Task AgregarAsync()
        {
            ProfesoresServices servicioProfesores = new ProfesoresServices();
            Profesores profesorNuevo = new Profesores()
            {
                Cedula = etCedula.Text,
                Administrador = false,
                Contrasenia = etContrasenia.Text,
                Correo = etCorreo.Text,
                Nombre = etNombre.Text,
                PrimerApellido = etApellido1.Text,
                SegundoApellido = etApellido2.Text
            };
            HttpResponseMessage resultado = await servicioProfesores.PostAsync(profesorNuevo);

            if (resultado.IsSuccessStatusCode)
            {
                // Se obtiene el elemento insertado
                string resultadoString = await resultado.Content.ReadAsStringAsync();
                profesorNuevo = JsonConvert.DeserializeObject<Profesores>(resultadoString);
                // Se actualiza la lista de profesores
                profesores.Add(profesorNuevo);
                profesoresAdapter.ActualizarDatos();

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

        private void Editar(object sender, EventArgs e)
        {
            if (EntradaValida())
            {
#pragma warning disable CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
                EditarAsync();
#pragma warning restore CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
            }
        }

        private async Task EditarAsync()
        {
            ProfesoresServices servicioProfesores = new ProfesoresServices();
            profesor.Cedula = etCedula.Text;
            profesor.Nombre = etNombre.Text;
            profesor.PrimerApellido = etApellido1.Text;
            profesor.SegundoApellido = etApellido2.Text;
            profesor.Correo = etCorreo.Text;
            profesor.Contrasenia = etContrasenia.Text;
            bool resultado = await servicioProfesores.UpdateProfesorAsync(profesor);

            if (resultado)
            {
                // Se actualiza la lista de profesores
                profesoresAdapter.ActualizarDatos();

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