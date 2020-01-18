using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang.Reflect;
using SAEEAPP.Adaptadores;
using SAEEP.ManejoNotificaciones;
using Xamarin.core.Models;
using Xamarin.core.OfflineServices;

namespace SAEEAPP.Listener
{
    public class NotificacionesListener : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Activity _context;
        private readonly List<Notificaciones> _notificaciones;
        private readonly Notificaciones _notificacion;
        private readonly ListNotificacionesAdaptor _adaptador;
        private readonly Button _btnOpciones;
        public NotificacionesListener(Activity context, List<Notificaciones> notificaciones, Notificaciones notificacion,
            ListNotificacionesAdaptor adaptador,Button btnOpciones)
        {
            _context = context;
            _notificaciones = notificaciones;
            _notificacion = notificacion;
            _adaptador = adaptador;
            _btnOpciones = btnOpciones;
        }
        public void OnClick(View v)
        {
            Context wrapper = new ContextThemeWrapper(_context, Resource.Style.PopupTheme);
            PopupMenu menu = new PopupMenu(wrapper, _btnOpciones);
            Field field = menu.Class.GetDeclaredField("mPopup");
            field.Accessible = true;
            Java.Lang.Object menuPopupHelper = field.Get(menu);
            Method setForceIcons = menuPopupHelper.Class.GetDeclaredMethod("setForceShowIcon", Java.Lang.Boolean.Type);
            setForceIcons.Invoke(menuPopupHelper, true);
            menu.Inflate(Resource.Layout.menu_popup);
            
            menu.MenuItemClick += (s, args) =>
            {
                var botonSeleccionado = args.Item.ItemId;
                switch (botonSeleccionado)
                {
                    case Resource.Id.item1://Editar
                        OnClick_Editar();
                        break;
                    case Resource.Id.item2://Borrar
                        OnClick_Eliminar();
                        break;
                    default://ERROR
                        Toast.MakeText(_context, "ERROR", ToastLength.Short).Show();
                        break;
                }

            };
            menu.DismissEvent += (s, args) =>
            {

            };
            menu.Show();
            
        }
        private void OnClick_Eliminar() {
            AlertDialog.Builder dialog = new AlertDialog.Builder(_context);
            AlertDialog alert = dialog.Create();
            alert.SetTitle("Eliminar");
            alert.SetMessage("Está seguro?");

            alert.SetButton("Sí", (c, ev) =>
            {
                var eliminarNotificacion = new NotificacionesServices("dbNotificaciones.db");
                var eliminado = eliminarNotificacion.DeleteNotificaciones(_notificacion);
                if (eliminado)
                {
                    EliminarAlarmManager(_notificacion);
                    _notificaciones.Remove(_notificacion);
                    _adaptador.ActualizarDatos();

                }
                else
                {
                    Toast.MakeText(_context, "No se pudo eliminar", ToastLength.Long).Show();
                }

            });
            alert.SetButton2("Cancelar", (c, ev) => { });
            alert.Show();
        }
        private void EliminarAlarmManager(Notificaciones notificacion)
        {

            new CrearEliminarNotificaciones(_context, notificacion, false);
            Toast.MakeText(_context, "Eliminado con éxito!", ToastLength.Long).Show();
        }

        private void OnClick_Editar() {
            AENotificacionesActivity editarEstudiante =
                new AENotificacionesActivity(_context, _adaptador, _notificaciones, _notificacion);
            editarEstudiante.Show();

        }
    }
}