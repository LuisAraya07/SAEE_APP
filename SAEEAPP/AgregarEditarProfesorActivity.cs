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
using Xamarin.core.Data;
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
        Profesores profesor, profesorTemp;
        EditText etCedula, etNombre, etApellido1, etApellido2, etCorreo, etContrasenia;
        Button btAgregarEditar, btCancelar;
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

        private async void Agregar(object sender, EventArgs e)
        {
            if (EntradaValida())
            {
                // Se bloquean los botones
                ActivarDesactivarBotones(false);
                Toast.MakeText(context, "Agregando, un momento...", ToastLength.Short).Show();

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
                    // Se restablecen los botones
                    ActivarDesactivarBotones(true);
                    Toast.MakeText(context, "Error al agregar, intente nuevamente", ToastLength.Short).Show();
                }
            }
        }

        private void Cancelar(object sender, EventArgs e)
        {
            alertDialogAndroid.Dismiss();
        }

        private async void Editar(object sender, EventArgs e)
        {
            if (EntradaValida())
            {
                // Se bloquean los botones
                ActivarDesactivarBotones(false);
                Toast.MakeText(context, "Guardando, un momento...", ToastLength.Short).Show();

                ProfesoresServices servicioProfesores = new ProfesoresServices();
                profesorTemp.Cedula = etCedula.Text;
                profesorTemp.Nombre = etNombre.Text;
                profesorTemp.PrimerApellido = etApellido1.Text;
                profesorTemp.SegundoApellido = etApellido2.Text;
                profesorTemp.Correo = etCorreo.Text;
                profesorTemp.Contrasenia = etContrasenia.Text;
                bool resultado = await servicioProfesores.UpdateProfesorAsync(profesorTemp);
                if (resultado)
                {
                    if (profesor.Id == ClienteHttp.Usuario.Profesor.Id)
                    {
                        ClienteHttp.Usuario.Profesor.Cedula = profesorTemp.Cedula;
                        ClienteHttp.Usuario.Profesor.Nombre = profesorTemp.Nombre;
                        ClienteHttp.Usuario.Profesor.PrimerApellido = profesorTemp.PrimerApellido;
                        ClienteHttp.Usuario.Profesor.SegundoApellido = profesorTemp.SegundoApellido;
                        ClienteHttp.Usuario.Profesor.Correo = profesorTemp.Correo;
                        ClienteHttp.Usuario.Profesor.Contrasenia = profesorTemp.Contrasenia;
                        ClienteHttp.ActualizarHeaders();
                    }
                    // Se asigna lo del temp al profesor
                    profesor.Cedula = profesorTemp.Cedula;
                    profesor.Nombre = profesorTemp.Nombre;
                    profesor.PrimerApellido = profesorTemp.PrimerApellido;
                    profesor.SegundoApellido = profesorTemp.SegundoApellido;
                    profesor.Correo = profesorTemp.Correo;
                    profesor.Contrasenia = profesorTemp.Contrasenia;
                    // Se actualiza la lista de profesores
                    profesoresAdapter.ActualizarDatos();

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
            if (etCedula.Text.Equals("") || etCedula.Text.StartsWith(" "))
            {
                Toast.MakeText(context, "Ingrese la cédula", ToastLength.Short).Show();
                return false;
            }
            else if (etNombre.Text.Equals("") || etNombre.Text.StartsWith(" "))
            {
                Toast.MakeText(context, "Ingrese el nombre", ToastLength.Short).Show();
                return false;
            }
            else if (etApellido1.Text.Equals("") || etApellido1.Text.StartsWith(" "))
            {
                Toast.MakeText(context, "Ingrese el primer apellido", ToastLength.Short).Show();
                return false;
            }
            else if (etApellido2.Text.Equals("") || etApellido2.Text.StartsWith(" "))
            {
                Toast.MakeText(context, "Ingrese el segundo apellido", ToastLength.Short).Show();
                return false;
            }
            else if (etContrasenia.Text.Equals("") || etContrasenia.Text.StartsWith(" "))
            {
                Toast.MakeText(context, "Ingrese la contraseña", ToastLength.Short).Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ActivarDesactivarBotones(bool estado)
        {
            btAgregarEditar.Enabled = estado;
            btCancelar.Enabled = estado;
            alertDialogAndroid.SetCancelable(estado);
        }

        public void Show()
        {
            alertDialogAndroid.Show();
            // Se obtienen los botones para asignarles los métodos nuevos (no cierran el diálogo).
            btAgregarEditar = alertDialogAndroid.GetButton((int)DialogButtonType.Positive);
            btCancelar = alertDialogAndroid.GetButton((int)DialogButtonType.Negative);

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