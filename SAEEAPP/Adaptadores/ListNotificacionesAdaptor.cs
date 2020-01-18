using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using SAEEAPP.Listener;
using Xamarin.core.Models;

namespace SAEEAPP.Adaptadores
{
    public class ListNotificacionesAdaptor : BaseAdapter<Notificaciones>
    {

        private readonly Activity _context;
        private readonly List<Notificaciones> _notificaciones;
        


        public ListNotificacionesAdaptor(Activity context, List<Notificaciones> notificaciones)
        {
            _context = context;
            _notificaciones = notificaciones;

        }
        public override Notificaciones this[int position] => _notificaciones[position];

        public override int Count => _notificaciones.Count;

        public override long GetItemId(int position)
        {
            return this[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            var notificacion = this[position];
            if (row == null)
            {
                row = _context.LayoutInflater.Inflate(Resource.Layout.NotificacionesListRow, null);

            }
            var fecha = Convert.ToDateTime(notificacion.Date);
            var fechaDiasAntes = Convert.ToDateTime(notificacion.DiasAntes);
            DefinirBotones(row,notificacion);
            row.FindViewById<TextView>(Resource.Id.tvFecha).Text = fecha.ToShortDateString();
            row.FindViewById<TextView>(Resource.Id.tvHora).Text = notificacion.Time;
            row.FindViewById<TextView>(Resource.Id.tvMensaje).Text = notificacion.Note;
            if (fecha == fechaDiasAntes)
            {
                row.FindViewById<TextView>(Resource.Id.tvDiasAntes).Text = "No tiene aviso días antes";

            }
            else {
                var diferencia = fecha - fechaDiasAntes;
                row.FindViewById<TextView>(Resource.Id.tvDiasAntes).Text ="Tiene aviso "+ diferencia.Days.ToString()+" días antes";
            }
            return row;
        }
        
        public void DefinirBotones(View row,Notificaciones notificacion)
        {
            var btOpciones = row.FindViewById<Button>(Resource.Id.btnNotificaciones);
            var draw = ContextCompat.GetDrawable(_context, Resource.Drawable.dots_vertical);
            btOpciones.SetCompoundDrawablesWithIntrinsicBounds(draw, null, null, null);
            btOpciones.SetTag(Resource.Id.btnNotificaciones, btOpciones);
            btOpciones.SetOnClickListener(new NotificacionesListener(_context, _notificaciones, notificacion, this, btOpciones));
        }

        public void ActualizarDatos()
        {
            NotifyDataSetChanged();
        }
       
       
    }
}