using Android.App;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SAEEAPP.Adaptadores;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.core;
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
        Cursos cursoTemp, curso;
        EditText etNombre, etCantidadPeriodos;
        Button btAgregarEditar, btCancelar;
        private readonly bool editando;

        public AgregarEditarCursosActivity(Activity context, CursosListAdapter cursosAdapter, List<Cursos> cursos)
        {
            editando = false;
            InicializarValores(context, cursosAdapter, cursos, "Agregando curso", "Agregar");
            cursoTemp = null;
        }

        public AgregarEditarCursosActivity(Activity context, CursosListAdapter cursosAdapter,
            List<Cursos> cursos, Cursos curso)
        {
            editando = true;
            InicializarValores(context, cursosAdapter, cursos, "Editando curso", "Guardar");
            etNombre.Text = curso.Nombre;
            etCantidadPeriodos.Text = curso.CantidadPeriodos.ToString();
            this.curso = curso;
            cursoTemp = new Cursos()
            {
                Id = curso.Id,
                Nombre = curso.Nombre,
                CantidadPeriodos = curso.CantidadPeriodos,
                CursosGrupos = curso.CursosGrupos,
                IdProfesor = curso.IdProfesor,
                //Aqui puede dar problemas offline
                IdProfesorNavigation = curso.IdProfesorNavigation
            };
        }

        private void InicializarValores(Activity context, CursosListAdapter cursosAdapter,
            List<Cursos> cursos, string titulo, string textoBotonConfirmacion)
        {
            this.context = context;
            this.cursosAdapter = cursosAdapter;
            this.cursos = cursos;
            LayoutInflater layoutInflater = LayoutInflater.From(context);
            View VistaAgregar = layoutInflater.Inflate(Resource.Layout.Dialogo_Agregar_Curso, null);
            etNombre = VistaAgregar.FindViewById<EditText>(Resource.Id.etNombre);
            etCantidadPeriodos = VistaAgregar.FindViewById<EditText>(Resource.Id.etCantidadPeriodos);
            var tilCantidadPeriodos = VistaAgregar.FindViewById<TextInputLayout>(Resource.Id.tilCantidadPeriodos);
            if (editando)
            {
                etCantidadPeriodos.Enabled = false;
            }
            else
            {
                tilCantidadPeriodos.Hint += "(No se puede editar posteriormente)";
            }
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
                Toast.MakeText(context, "Agregando, un momento", ToastLength.Short).Show();

                CursosServices servicioCursos;
                Cursos cursoNuevo = new Cursos()
                {
                    IdProfesor = 1,// En el api se asigna el correcto
                    Nombre = etNombre.Text,
                    CantidadPeriodos = int.Parse(etCantidadPeriodos.Text)
                };
                VerificarConexion vc = new VerificarConexion(context);
                var conectado = vc.IsOnline();
                if (conectado)
                {
                    servicioCursos = new CursosServices();
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
                        // Se restablecen los botones
                        ActivarDesactivarBotones(true);
                        Toast.MakeText(context, "Error al agregar, intente nuevamente", ToastLength.Short).Show();
                    }
                }
                else {
                    //AQUI OFFLINE
                    // Toast.MakeText(context, "Necesita conexión a internet.", ToastLength.Short).Show(); 
                    ProfesoresServices ns = new ProfesoresServices(1);
                    Profesores profesor = await ns.GetProfesorConectado();
                    if (!(profesor == null))
                    {
                        servicioCursos = new CursosServices(profesor.Id);
                        var cursoAgregar = await servicioCursos.PostOffline(cursoNuevo);
                        cursos.Add(cursoAgregar);
                        cursosAdapter.ActualizarDatos();
                        Toast.MakeText(context, "Agregado correctamente", ToastLength.Long).Show();
                        alertDialogAndroid.Dismiss();
                    }
                    else
                    {
                        Toast.MakeText(context, "No hay bases de datos local.", ToastLength.Long).Show();
                        alertDialogAndroid.Dismiss();
                    }
                    
                }
            }
        }

        private void Cancelar(object sender, EventArgs e)
        {
            alertDialogAndroid.Dismiss();
        }

        private async void Editar(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(context);
            var conectado = vc.IsOnline();
            if (EntradaValida())
            {
                // Se bloquean los botones
                ActivarDesactivarBotones(false);
                Toast.MakeText(context, "Guardando, un momento...", ToastLength.Short).Show();

                CursosServices servicioCursos;
                cursoTemp.Nombre = etNombre.Text;
                cursoTemp.CantidadPeriodos = int.Parse(etCantidadPeriodos.Text);
                bool resultado;
                if (conectado)
                {
                    servicioCursos = new CursosServices();
                    resultado = await servicioCursos.UpdateCursoAsync(cursoTemp);
                }
                else
                {
                    //AQUI OFFLINE
                    ProfesoresServices ns = new ProfesoresServices(1);
                    Profesores profesor = await ns.GetProfesorConectado();
                    if (!(profesor == null))
                    {
                        servicioCursos = new CursosServices(profesor.Id);
                        resultado = await servicioCursos.UpdateCursoOffline(cursoTemp);
                    }
                    else
                    {
                        resultado = false;
                    }
                        
                }
                if (resultado)
                {
                    curso.Nombre = cursoTemp.Nombre;
                    curso.CantidadPeriodos = cursoTemp.CantidadPeriodos;
                    // Se actualiza la lista de cursos
                    cursosAdapter.ActualizarDatos();
                    Toast.MakeText(context, "Guardado correctamente", ToastLength.Long).Show();
                    alertDialogAndroid.Dismiss();
                }
                else
                {
                    // Se restablecen los botones
                    ActivarDesactivarBotones(true);
                    Toast.MakeText(context, "Error al guardar.", ToastLength.Short).Show();
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