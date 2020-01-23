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
    public class NotasActivity
    {
        AlertDialog.Builder alertDialogBuilder;
        AlertDialog alertDialogAndroid;
        Activity context;
        Button  btCancelar;
        List<EstudianteEvaluacion> _estudiantes;
        Asignaciones _asignacion;
        View VistaAgregar;
        TextView tvAsignatura;
        ListView lvNotas;
        NotasAdaptador AdaptadorNotas;
        public NotasActivity(Activity context,List<EstudianteEvaluacion> estudiantes, Asignaciones asignacion)
        {
            this.context = context;
            _estudiantes = estudiantes;
            _asignacion = asignacion;
            LayoutInflater layoutInflater = LayoutInflater.From(context);
            VistaAgregar = layoutInflater.Inflate(Resource.Layout.activity_notas, null);
            tvAsignatura = VistaAgregar.FindViewById<TextView>(Resource.Id.tvAsignatura);

            lvNotas = VistaAgregar.FindViewById<ListView>(Resource.Id.lvNotas);

            // El mensaje de "no hay datos", lo asigna el Adapter (ya que se pueden eliminar todos los curso)
            AdaptadorNotas = new NotasAdaptador(context, _estudiantes, _asignacion);
            lvNotas.Adapter = AdaptadorNotas;

            tvAsignatura.Text = asignacion.Nombre+"  Puntos: "+asignacion.Puntos+"  Porcentaje: "+asignacion.Porcentaje;
            alertDialogBuilder = new AlertDialog.Builder(context, Resource.Style.AlertDialogStyle)
            .SetView(VistaAgregar)
            //.SetPositiveButton("Guardar", (EventHandler<DialogClickEventArgs>)null)
            .SetNegativeButton("Cerrar", (EventHandler<DialogClickEventArgs>)null)
            .SetTitle("Notas");
            alertDialogAndroid = alertDialogBuilder.Create();
        }
        private void Cancelar(object sender, EventArgs e)
        {
            alertDialogAndroid.Dismiss();
        }

        public void Show()
        {
            alertDialogAndroid.Show();
            btCancelar = alertDialogAndroid.GetButton((int)DialogButtonType.Negative);
            btCancelar.Click += Cancelar;
        }
    }
}