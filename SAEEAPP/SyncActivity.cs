using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{
    [Activity(Label = "Sincronizando Datos", Theme = "@style/AppTheme")]
    public class SyncActivity : AppCompatActivity
    {
        private ProgressBar progressBar;
        private TextView txtProgress;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_sincronizando);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            txtProgress = FindViewById<TextView>(Resource.Id.txtProgress);
            
        }
        protected async override void OnStart()
        {
            base.OnStart();
            int estadoBarra = 0;
            txtProgress.Text = "Sincronizando....";
            SincronizarActivity menuOpciones = new SincronizarActivity(this);
            var sincronizado = await menuOpciones.Sincronizar();

            new Thread(new ThreadStart(delegate
            {
                while (estadoBarra < 100)
                {
                    estadoBarra += 1;
                    
                    Thread.Sleep(50);
                    progressBar.Progress = estadoBarra;
                    

                }
                
                VolverMain(sincronizado,menuOpciones);

            })).Start();
            
        }


        public async void VolverMain(bool sincronizado, SincronizarActivity menuOpciones)
        {
            if (sincronizado)
            {
                Intent mainActivity = new Intent(this, typeof(MainActivity));
                StartActivity(mainActivity);
            }
            else
            {
                ProfesoresServices ns = new ProfesoresServices(1);
                Profesores profesor = await ns.GetProfesorConectado();
                var GruposServiciosOffline = new GruposServices(profesor.Id);
                await GruposServiciosOffline.EliminarDBLocal();
                Intent mainActivity = new Intent(this, typeof(MainActivity));
                StartActivity(mainActivity);
            }
            
        }

    }
}