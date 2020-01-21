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
using Xamarin.core;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{

    public class AgregarEditarAsignacionesActivity
    {
        AlertDialog.Builder alertDialogBuilder;
        AlertDialog alertDialogAndroid;
        Activity context;
        AsignacionesAdaptador AdaptadorAsignaciones;
        List<Asignaciones> asignaciones;
        Asignaciones asignacionTemp, asignacion;
        EditText etNombre, etDescripcion,etFecha,etPuntos,etPorcentaje;
        Button btAgregarEditar, btCancelar;
        private readonly bool editando;


        public AgregarEditarAsignacionesActivity(Activity context, AsignacionesAdaptador adapter, List<Asignaciones> asignaciones)
        {
            InicializarValores(context, adapter, asignaciones, "Agregando asignación", "Agregar");
            editando = false;
            asignacionTemp = null;
        }
        public AgregarEditarAsignacionesActivity(Activity context, AsignacionesAdaptador adapter,List<Asignaciones> asignaciones, Asignaciones asignacion)
        {
            InicializarValores(context, adapter, asignaciones, "Editando asignación", "Guardar");
            editando = true;
            etNombre.Text = asignacion.Nombre;
            etDescripcion.Text = asignacion.Descripcion;
            etFecha.Text = asignacion.Fecha.ToShortDateString();
            this.asignacion = asignacion;
            asignacionTemp = new Asignaciones()
            {
                Id = asignacion.Id,
                Tipo = asignacion.Tipo,
                Profesor = asignacion.Profesor,
                Curso = asignacion.Curso,
                Grupo = asignacion.Grupo,
                Nombre = asignacion.Nombre,
                Descripcion = asignacion.Descripcion,
                Estado = asignacion.Estado,
                Fecha = asignacion.Fecha,
                Puntos = asignacion.Puntos,
                Porcentaje = asignacion.Porcentaje
            };
        }
        private void InicializarValores(Activity context, AsignacionesAdaptador adapter,
            List<Asignaciones> asignaciones, string titulo, string textoBotonConfirmacion)
        {
            this.context = context;
            this.AdaptadorAsignaciones = adapter;
            this.asignaciones = asignaciones;
            LayoutInflater layoutInflater = LayoutInflater.From(context);


            View VistaAgregar = layoutInflater.Inflate(Resource.Layout.Dialogo_Agregar_Asignacion, null);


            etNombre = VistaAgregar.FindViewById<EditText>(Resource.Id.etNombre);
            etDescripcion = VistaAgregar.FindViewById<EditText>(Resource.Id.etDescripcion);
            etFecha = VistaAgregar.FindViewById<EditText>(Resource.Id.etFecha);
            etPuntos = VistaAgregar.FindViewById<EditText>(Resource.Id.etPuntos);
            etPorcentaje = VistaAgregar.FindViewById<EditText>(Resource.Id.etPorcentaje);
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
                Toast.MakeText(context, "Agregando, espere", ToastLength.Short).Show();

                AsignacionesServices servicioAsignaciones = new AsignacionesServices();
                Asignaciones asignNueva = new Asignaciones()
                {
                    Tipo = "Tareas",
                    Profesor = 1,// En el api se asigna el correcto
                    Curso = 5,
                    Grupo = 32,
                    Nombre = etNombre.Text,
                    Descripcion = etDescripcion.Text,
                    Estado = "Ninguno",
                    Fecha = Convert.ToDateTime(etFecha.Text),
                    Puntos = Decimal.Parse(etPuntos.Text),
                    Porcentaje = Decimal.Parse(etPorcentaje.Text)
                };
                VerificarConexion vc = new VerificarConexion(context);
                var conectado = vc.IsOnline();
                if (conectado)
                {
                    HttpResponseMessage resultado = await servicioAsignaciones.PostAsync(asignNueva);
                    if (resultado.IsSuccessStatusCode)
                    {
                        // Se obtiene el elemento insertado
                        string resultadoString = await resultado.Content.ReadAsStringAsync();
                        asignNueva = JsonConvert.DeserializeObject<Asignaciones>(resultadoString);
                        // Se actualiza la lista de asignaciones
                        asignaciones.Add(asignNueva);
                        AdaptadorAsignaciones.ActualizarDatos();

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
                else Toast.MakeText(context, "Necesita conexión a internet.", ToastLength.Short).Show();
            }
        }
        private bool EntradaValida()
        {
            if (etNombre.Text.Equals("") || etNombre.Text.StartsWith(" "))
            {
                Toast.MakeText(context, "Ingrese el nombre", ToastLength.Long).Show();
                return false;
            }
            else if (etDescripcion.Text.Equals("") || etDescripcion.Text.StartsWith(" "))
            {
                Toast.MakeText(context, "Indique la descripción", ToastLength.Long).Show();
                return false;
            }
            else if (etFecha.Text.Equals("") || etFecha.Text.StartsWith(" "))
            {
                Toast.MakeText(context, "Indique la cantidad de periodos", ToastLength.Long).Show();
                return false;
            }
            else if (Decimal.Parse(etPuntos.Text) <= 0)
            {
                Toast.MakeText(context, "Los puntos deben ser mayor a cero", ToastLength.Long).Show();
                return false;
            }
            else if (Decimal.Parse(etPorcentaje.Text) <= 0)
            {
                Toast.MakeText(context, "El porcentaje debe ser mayor a cero", ToastLength.Long).Show();
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
        private async void Editar(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(context);
            var conectado = vc.IsOnline();
            if (conectado)
            {

                if (EntradaValida())
                {
                    // Se bloquean los botones
                    ActivarDesactivarBotones(false);
                    Toast.MakeText(context, "Guardando, espere", ToastLength.Short).Show();

                    AsignacionesServices servAsignaciones= new AsignacionesServices();
                    asignacionTemp.Profesor = asignacion.Profesor;
                    asignacionTemp.Curso = asignacion.Curso;
                    asignacionTemp.Grupo = asignacion.Grupo;
                    asignacionTemp.Nombre = etNombre.Text;
                    asignacionTemp.Descripcion = etDescripcion.Text;
                    asignacionTemp.Fecha = Convert.ToDateTime(etFecha.Text);
                    asignacionTemp.Puntos = Decimal.Parse(etPuntos.Text);
                    asignacionTemp.Porcentaje = Decimal.Parse(etPorcentaje.Text);
                    bool resultado = await servAsignaciones.UpdateAsignacionAsync(asignacionTemp);
                    if (resultado)
                    {
                        asignacion = asignacionTemp;
                        // Se actualiza la lista de cursos
                        AdaptadorAsignaciones.ActualizarDatos();

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
            else
            {
                ActivarDesactivarBotones(true);
                Toast.MakeText(context, "Necesita conexión a internet", ToastLength.Short).Show();
            }
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
        private void Cancelar(object sender, EventArgs e)
        {
            alertDialogAndroid.Dismiss();
        }

    }
}