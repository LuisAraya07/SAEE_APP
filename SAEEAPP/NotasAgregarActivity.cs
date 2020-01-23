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
    public class NotasAgregarActivity
    {
        AlertDialog.Builder alertDialogBuilder;
        AlertDialog alertDialogAndroid;
        Activity context;
        Button  btCancelar,btModificar;
        EstudianteEvaluacion _estudiante;
        Asignaciones _asignacion;
        View VistaAgregar;
        EditText etPuntos, etPorcentaje, etNota;
        public NotasAgregarActivity(Activity context,EstudianteEvaluacion estudiante, Asignaciones asignacion)
        {
            this.context = context;
            _estudiante = estudiante;
            _asignacion = asignacion;
            LayoutInflater layoutInflater = LayoutInflater.From(context);
            VistaAgregar = layoutInflater.Inflate(Resource.Layout.Dialogo_Notas_Modificar, null);
            etPuntos = VistaAgregar.FindViewById<EditText>(Resource.Id.etPuntos);
            etPorcentaje = VistaAgregar.FindViewById<EditText>(Resource.Id.etPorcentaje);
            etNota = VistaAgregar.FindViewById<EditText>(Resource.Id.etNota);
                                                                                                                                                            
            etPuntos.Text = _estudiante.Puntos.ToString();
            etPorcentaje.Text = _estudiante.Porcentaje.ToString();
            etNota.Text = _estudiante.Nota.ToString();
            alertDialogBuilder = new AlertDialog.Builder(context, Resource.Style.AlertDialogStyle)
            .SetView(VistaAgregar)
            .SetPositiveButton("Guardar", (EventHandler<DialogClickEventArgs>)null)
            .SetNegativeButton("Cerrar", (EventHandler<DialogClickEventArgs>)null)
            .SetTitle("Notas : "+estudiante.Cedula);
            alertDialogAndroid = alertDialogBuilder.Create();
        }
        private void Cancelar(object sender, EventArgs e)
        {
            alertDialogAndroid.Dismiss();
        }
        private async void Guardar(object sender, EventArgs e)
        {

        }
        public void Show()
        {
            alertDialogAndroid.Show();
            btCancelar = alertDialogAndroid.GetButton((int)DialogButtonType.Negative);
            btCancelar.Click += Cancelar;
            btModificar = alertDialogAndroid.GetButton((int)DialogButtonType.Positive);
            btModificar.Click += Guardar;
        }
    }
}