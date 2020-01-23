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
using SAEEAPP.Adaptadores;
using SAEEP.ManejoNotificaciones;
using Xamarin.core;
using Xamarin.core.Data;
using Xamarin.core.Models;
using Xamarin.core.OfflineServices;

namespace SAEEAPP
{
    public class AENotificacionesActivity
    {
        AlertDialog.Builder alertDialogBuilder;
        AlertDialog alertDialogAndroid;
        Activity context;
        ListNotificacionesAdaptor notificacionesAdapter;
        List<Notificaciones> notificaciones;
        readonly Notificaciones notificacion = new Notificaciones();
        readonly Notificaciones notificacionSinEditar = new Notificaciones();
        private EditText _dateDisplay;
        private EditText _timeDisplay;
        private EditText _txtNote;
      //  NumberPicker _numberPicker;
        Spinner spinner;
        int valorSpinner = 0;
        private readonly bool editando;

        public AENotificacionesActivity(Activity context, ListNotificacionesAdaptor notificacionesAdapter, List<Notificaciones> notificaciones) {
            InicializarValores(context, notificacionesAdapter, notificaciones, "    Agregando Notificación", "Agregar");
            editando = false;
        }

        public AENotificacionesActivity(Activity context, ListNotificacionesAdaptor notificacionesAdapter, List<Notificaciones> notificaciones,
            Notificaciones notificacion)
        {
            InicializarValores(context, notificacionesAdapter, notificaciones, "    Editando Notificación", "Guardar");
            editando = true;
            var fecha = Convert.ToDateTime(notificacion.Date);
            var fechaDiasAntes = Convert.ToDateTime(notificacion.DiasAntes);
            _dateDisplay.Text = notificacion.Date;
            spinner.SetSelection((fecha - fechaDiasAntes).Days);
            _timeDisplay.Text = notificacion.Time;
            _txtNote.Text = notificacion.Note;
            this.notificacion = notificacion;
            notificacionSinEditar = notificacion;
        }
        private void InicializarValores(Activity context, ListNotificacionesAdaptor notificacionesAdapter,
            List<Notificaciones> notificaciones, string titulo, string textoBotonConfirmacion)
        {
            this.context = context;
            this.notificacionesAdapter = notificacionesAdapter;
            this.notificaciones = notificaciones;
            LayoutInflater layoutInflater = LayoutInflater.From(context);
            View VistaAgregar = layoutInflater.Inflate(Resource.Layout.Dialogo_Agregar_Notificaciones, null);
            _dateDisplay = VistaAgregar.FindViewById<EditText>(Resource.Id.date_display);
            _timeDisplay = VistaAgregar.FindViewById<EditText>(Resource.Id.time_display);
            _txtNote = VistaAgregar.FindViewById<EditText>(Resource.Id.txtNote);
            spinner = VistaAgregar.FindViewById<Spinner>(Resource.Id.spinner1);
            spinner.Prompt= "Días";
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(context, Resource.Array.listaDias,Resource.Layout.SpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            //AlertDialogStyle
            alertDialogBuilder = new AlertDialog.Builder(context, Resource.Style.AlertDialogStyle)
            .SetView(VistaAgregar)
            .SetPositiveButton(textoBotonConfirmacion, (EventHandler<DialogClickEventArgs>)null)
            .SetNegativeButton("Cancelar", (EventHandler<DialogClickEventArgs>)null)
            .SetCancelable(false)
            .SetTitle(titulo);
            alertDialogAndroid = alertDialogBuilder.Create();
        }
        private void Cancelar(object sender, EventArgs e)
        {
            alertDialogAndroid.Dismiss();
        }
        private void AgregarAsync(object sender, EventArgs e)
        {
            notificacion.Note = _txtNote.Text + " - Para la fecha: " + notificacion.Date;
            DateTime currentDT = DateTime.Now;
            var selecteDiasAntes = Convert.ToDateTime(notificacion.Date + " " + notificacion.Time).AddDays(- Convert.ToInt32(spinner.SelectedItem.ToString()));
            DateTime selectedDT = Convert.ToDateTime(notificacion.Date + " " + notificacion.Time);
            if (Validar())
            {
                VerificarConexion vc = new VerificarConexion(context);
                var conectado = vc.IsOnline();
                if (conectado)
                {
                    if (selectedDT > currentDT && selecteDiasAntes > currentDT)
                    {
                        notificacion.IdProfesor = ClienteHttp.Usuario.Profesor.Id;
                        notificacion.DiasAntes = Convert.ToDateTime(notificacion.Date).AddDays(-Convert.ToInt32(spinner.SelectedItem.ToString())).ToShortDateString();
                        //Estado true
                        notificacion.Estado = 1;
                        var notificacionIns = new NotificacionesServices();
                        Notificaciones notificacionId = notificacionIns.PostNotificaciones(notificacion);
                        //Obtengo todos las notificaciones de ese profesor
                        // List<Reminder> listitem = ReminderHelper.GetReminderList(context, 1);
                        //ScheduleReminder(listitem.LastOrDefault());
                        new CrearEliminarNotificaciones(context, notificacionId, true);
                        notificaciones.Add(notificacion);
                        notificacionesAdapter.ActualizarDatos();
                        Toast.MakeText(context, "Agregado correctamente", ToastLength.Long).Show();
                        alertDialogAndroid.Dismiss();
                    }
                    else
                    {
                        Toast.MakeText(context, "Error en las fechas", ToastLength.Long).Show();
                    }
                }
                else
                {
                    Toast.MakeText(context, "Necesita conexión a internet.", ToastLength.Long).Show();
                }
                
            }
        }
        private void Editar(object sender, EventArgs e)
        {
            notificacion.Note = _txtNote.Text + " - Para la fecha: " + notificacion.Date;
            DateTime currentDT = DateTime.Now;
            var selecteDiasAntes = Convert.ToDateTime(notificacion.Date + " " + notificacion.Time).AddDays(- Convert.ToInt32(spinner.SelectedItem.ToString()));
            DateTime selectedDT = Convert.ToDateTime(notificacion.Date + " " + notificacion.Time);
            if (Validar())
            {
                VerificarConexion vc = new VerificarConexion(context);
                var conectado = vc.IsOnline();
                if (conectado)
                {
                    if (selectedDT > currentDT && selecteDiasAntes > currentDT)
                    {
                        //notificacion.IdProfesor = 1;
                        notificacion.DiasAntes = Convert.ToDateTime(notificacion.Date).AddDays(-valorSpinner).ToShortDateString();
                        //Estado true
                        notificacion.Estado = 1;
                        var notificacionIns = new NotificacionesServices();
                        var modificado = notificacionIns.PutNotificaciones(notificacion);
                        if (modificado)
                        {

                            new CrearEliminarNotificaciones(context, notificacionSinEditar, false);
                            new CrearEliminarNotificaciones(context, notificacion, true);
                            notificacionesAdapter.ActualizarDatos();
                            Toast.MakeText(context, "Modificado correctamente", ToastLength.Long).Show();
                            alertDialogAndroid.Dismiss();

                        }
                    }
                    else
                    {
                        Toast.MakeText(context, "Error en las fechas", ToastLength.Long).Show();
                    }
                }
                else
                {
                    Toast.MakeText(context, "Necesita conexión a internet.", ToastLength.Long).Show();
                }
                
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
                btAgregarEditar.Click += AgregarAsync;
            }
            btCancelar.Click += Cancelar;
            _dateDisplay.Click += DateSelect_OnClick;
            _timeDisplay.Click += TimeSelectOnClick;
        }
        #region DateOperation
        [Obsolete]
        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                _dateDisplay.Text = time.ToString().Split(' ')[0];
                notificacion.Date = _dateDisplay.Text;
            });
            frag.Show(context.FragmentManager, DatePickerFragment.TAG);
        }
        #endregion

        #region TimeOperation
        [Obsolete]
        void TimeSelectOnClick(object sender, EventArgs eventArgs)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(
                delegate (DateTime time)
                {
                    _timeDisplay.Text = time.ToShortTimeString();
                    notificacion.Time = _timeDisplay.Text;
                });

            frag.Show(context.FragmentManager, TimePickerFragment.TAG);
        }
        #endregion
        bool Validar()
        {
            if (notificacion.Date == string.Empty || notificacion.Time == string.Empty || notificacion.Note == string.Empty)
            {
                Toast.MakeText(context, "Debe llenar todos los campos", ToastLength.Long).Show();
                return false;
            }

            return true;
        }
        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            //string toast = string.Format("The planet is {0}", spinner.GetItemAtPosition(e.Position));
            //Toast.MakeText(context, toast, ToastLength.Long).Show();
            valorSpinner = Convert.ToInt32(spinner.GetItemAtPosition(e.Position).ToString());
        }


    }
}