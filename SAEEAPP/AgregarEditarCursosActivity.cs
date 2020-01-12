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
    public class AgregarEditarCursosActivity
    {
        AlertDialog.Builder alertDialogBuilder;
        AlertDialog alertDialogAndroid;
        Activity context;
        CursosListAdapter cursosAdapter;
        List<Cursos> cursos;
        readonly Cursos curso;
        EditText etNombre, etCantidadPeriodos;
        private readonly bool editando;

        public AgregarEditarCursosActivity(Activity context, CursosListAdapter cursosAdapter, List<Cursos> cursos)
        {
            InicializarValores(context, cursosAdapter, cursos, "Agregando curso", "Agregar");
            editando = false;
            curso = null;
        }

        public AgregarEditarCursosActivity(Activity context, CursosListAdapter cursosAdapter,
            List<Cursos> cursos, Cursos curso)
        {
            InicializarValores(context, cursosAdapter, cursos, "Editando curso", "Guardar");
            editando = true;
            etNombre.Text = curso.Nombre;
            etCantidadPeriodos.Text = curso.CantidadPeriodos.ToString();
            this.curso = curso;
        }

        private void InicializarValores(Activity context, CursosListAdapter cursosAdapter,
            List<Cursos> cursos, string titulo, string textoBotonConfirmacion)
        {
            this.context = context;
            this.cursosAdapter = cursosAdapter;
            this.cursos = cursos;
            LayoutInflater layoutInflater = LayoutInflater.From(context);
#pragma warning disable CS0117 // 'Resource.Layout' no contiene una definición para 'Dialogo_Agregar_Curso'
            View VistaAgregar = layoutInflater.Inflate(Resource.Layout.Dialogo_Agregar_Curso, null);
#pragma warning restore CS0117 // 'Resource.Layout' no contiene una definición para 'Dialogo_Agregar_Curso'
            etNombre = VistaAgregar.FindViewById<EditText>(Resource.Id.etNombre);
#pragma warning disable CS0117 // 'Resource.Id' no contiene una definición para 'etCantidadPeriodos'
            etCantidadPeriodos = VistaAgregar.FindViewById<EditText>(Resource.Id.etCantidadPeriodos);
#pragma warning restore CS0117 // 'Resource.Id' no contiene una definición para 'etCantidadPeriodos'
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
                CursosServices servicioCursos = new CursosServices();
                Cursos cursoNuevo = new Cursos()
                {
                    IdProfesor = 1,
                    Nombre = etNombre.Text,
                    CantidadPeriodos = int.Parse(etCantidadPeriodos.Text)
                };
                HttpResponseMessage resultado = await servicioCursos.PostAsync(cursoNuevo);

                if (resultado.IsSuccessStatusCode)
                {
                    // Se obtiene el elemento insertado
                    string resultadoString = await resultado.Content.ReadAsStringAsync();
                    cursoNuevo = JsonConvert.DeserializeObject<Cursos>(resultadoString);
                    // Se actualiza la lista de cursos
                    cursos.Add(cursoNuevo);
                    cursosAdapter.ActualizarDatos();

                    Toast.MakeText(context, "Agregado correctamente", ToastLength.Long).Show();
                    alertDialogAndroid.Dismiss();
                }
                else
                {
                    Toast.MakeText(context, "Error al agregar, intente nuevamente", ToastLength.Long).Show();
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
                CursosServices servicioCursos = new CursosServices();
                curso.Nombre = etNombre.Text;
                curso.CantidadPeriodos = int.Parse(etCantidadPeriodos.Text);
                bool resultado = await servicioCursos.UpdateCursoAsync(curso);

                if (resultado)
                {
                    // Se actualiza la lista de cursos
                    cursosAdapter.ActualizarDatos();

                    Toast.MakeText(context, "Guardado correctamente", ToastLength.Long).Show();
                    alertDialogAndroid.Dismiss();
                }
                else
                {
                    Toast.MakeText(context, "Error al guardar, intente nuevamente", ToastLength.Long).Show();
                }
            }
        }

        private bool EntradaValida()
        {
            if (etNombre.Text.Equals("") || etNombre.Text.StartsWith(" "))
            {
                Toast.MakeText(context, "Ingrese el nombre", ToastLength.Long).Show();
                return false;
            }
            else if (etCantidadPeriodos.Text.Equals("") || etCantidadPeriodos.Text.StartsWith(" "))
            {
                Toast.MakeText(context, "Indique la cantidad de periodos", ToastLength.Long).Show();
                return false;
            }
            else if (int.Parse(etCantidadPeriodos.Text) <= 0)
            {
                Toast.MakeText(context, "La cantidad de periodos debe ser mayor a cero", ToastLength.Long).Show();
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