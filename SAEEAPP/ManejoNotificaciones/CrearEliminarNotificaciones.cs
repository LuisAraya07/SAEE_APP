using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Util;
using SAEEAPP.ManejoNotificaciones;
using Xamarin.core.Models;

namespace SAEEP.ManejoNotificaciones
{
    public class CrearEliminarNotificaciones
    {

        public CrearEliminarNotificaciones(Context context, Notificaciones notificacion,Boolean crear) {
            AlarmManager manager = (AlarmManager)context.GetSystemService(Context.AlarmService);
            Intent myIntent = new Intent(context, typeof(CrearRecordatorio));
            myIntent.PutExtra("Title", "Recordatorio");
            myIntent.PutExtra("Message", notificacion.Note);
            myIntent.PutExtra("Id", notificacion.Id);
            if (crear)
            {
                CrearNotificacion( context,  notificacion,manager,myIntent);

            }
            else
            {
                EliminarNotificacion(context, notificacion, manager, myIntent);

            }

        }
        public string ConvertirFecha(string fecha, string tiempo)
        {
            var t = tiempo.Split(':');
            var ampm = t[1].Split(' ')[1];
            var hrr = Convert.ToDouble(t[0]);
            var min = Convert.ToDouble(t[1].Split(' ')[0]);
            return fecha +" "+ hrr + ":" + min + ":00 " + ampm;
        }



        public void CrearNotificacion(Context context, Notificaciones notificacion,AlarmManager manager, Intent myIntent) {
            var fecha = ConvertirFecha(notificacion.Date,notificacion.Time);
            DateTimeOffset dateOffsetValue = DateTimeOffset.Parse(fecha);
            var millisec = dateOffsetValue.ToUnixTimeMilliseconds();
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, notificacion.Id, myIntent, 0);
            manager.Set(AlarmType.RtcWakeup, millisec, pendingIntent);
            if (!(notificacion.Date.Equals(notificacion.DiasAntes)))
            {
                var fechaDiasAntes = ConvertirFecha(notificacion.DiasAntes, notificacion.Time);
                DateTimeOffset dateOffsetValueDiasAntes = DateTimeOffset.Parse(fechaDiasAntes);
                var millisecDiasAntes = dateOffsetValueDiasAntes.ToUnixTimeMilliseconds();
                PendingIntent pendingIntentDiasAntes = PendingIntent.GetBroadcast(context, notificacion.Id, myIntent, 0);
                manager.Set(AlarmType.RtcWakeup, millisecDiasAntes, pendingIntentDiasAntes);
            }
        }

        public void EliminarNotificacion(Context context,Notificaciones notificacion, AlarmManager manager, Intent myIntent) {
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, notificacion.Id, myIntent, PendingIntentFlags.CancelCurrent);
            if (pendingIntent != null)
            {
                manager.Cancel(pendingIntent);
            }

        }


    }
}