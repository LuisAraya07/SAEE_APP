using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Microsoft.EntityFrameworkCore;
using SAEEAPP.Adaptadores;
using Xamarin.core.Data;
using Xamarin.core.Models;
using Xamarin.core.OfflineServices;

namespace SAEEAPP
{
    [Activity(Label = "Administración de notificaciones", Theme = "@style/AppTheme")]
    public class NotificacionesActivity : AppCompatActivity
    {
        TextView _txtLabel;
        ListView list;
        List<Notificaciones> listitem = new List<Notificaciones>();
        FloatingActionButton fab;
        ListNotificacionesAdaptor adapter;
        ProgressBar pbCargandoNotificaciones;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.NotificacionesListView);
            _txtLabel = FindViewById<TextView>(Resource.Id.txt_label);
            _txtLabel.Text = "Cargando...";
            list = (ListView)FindViewById(Resource.Id.listReminder);
            pbCargandoNotificaciones = FindViewById<ProgressBar>(Resource.Id.pbCargandoNotificaciones);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fabNotas);
            fab.Visibility = ViewStates.Invisible;
            fab.Click += AgregarNotificacion;
            BindDataAsync();
            
        }
        
        //private async void BinData6() {
        //    //var dbFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //    //var fileName = "dbLocal.db";
        //    //var dbFullPath = Path.Combine(dbFolder, fileName);
           

                    
               

        //}


        private async void BindDataAsync()
        {
            var notificacionIns = new NotificacionesServices("dbNotificaciones.db");
            //Id del profesor
            var idProfesor = ClienteHttp.Usuario.Profesor.Id;
            listitem = await notificacionIns.GetNotificaciones(idProfesor);
            if (listitem.Count > 0)
            {
                _txtLabel.Visibility = ViewStates.Invisible;

                //adapter = new GridReminder(this, listitem);
                //list.Adapter = adapter;
                //list.ItemClick += List_ItemClick;    
                //adapter = new ListNotificacionesAdaptor(this, listitem);
                //list.Adapter = adapter;
            }
            else
            {
                
                //list.Visibility = ViewStates.Invisible;
                _txtLabel.Visibility = ViewStates.Visible;
                _txtLabel.Text = "No tiene notificaciones";
                
            }
            fab.Visibility = ViewStates.Visible;
            adapter = new ListNotificacionesAdaptor(this, listitem);
            list.Adapter = adapter;
            list.Visibility = ViewStates.Visible;
            pbCargandoNotificaciones.Visibility = ViewStates.Gone;
        }

        private void AgregarNotificacion(object sender, EventArgs e)
        {
            AENotificacionesActivity agregarNotificacionActivity =
                new AENotificacionesActivity(this, adapter, listitem);
            agregarNotificacionActivity.Show();
        }
    }
}
