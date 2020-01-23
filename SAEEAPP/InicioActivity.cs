using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Xamarin.core;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{
    [Activity(Theme = "@style/AppTheme", MainLauncher = false)]
    public class InicioActivity : AppCompatActivity
    {
        private ProgressBar pbInicioSesion;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_inicio);
            pbInicioSesion = FindViewById<ProgressBar>(Resource.Id.pbInicioSesion);
            int estadoBarra = 0;
            
            new Thread(new ThreadStart(delegate
            {
                while (estadoBarra < 100)
                {
                    estadoBarra += 10;
                    pbInicioSesion.Progress = estadoBarra;
                    Thread.Sleep(400);
                }
                pbInicioSesion.Visibility = ViewStates.Invisible;
                VerificarUsuario();
            })).Start();
            
        }

     
        public async void VerificarUsuario()
        {
            VerificarConexion vc = new VerificarConexion(this);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                Intent siguiente = new Intent(this, typeof(MainActivity));
                StartActivity(siguiente);
            }
            else
            {
                ProfesoresServices ns = new ProfesoresServices(1);
                Profesores profesor = await ns.GetProfesorConectado();
                if (!(profesor == null))
                {
                    Intent siguiente = new Intent(this, typeof(MainActivity));
                    StartActivity(siguiente);
                }
                else
                {
                    Intent siguiente = new Intent(this, typeof(MainActivity));
                    StartActivity(siguiente);
                }
            }
            
        }
    }
}