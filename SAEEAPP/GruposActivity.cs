using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using SAEEAPP.Adaptadores;
using Xamarin.core.Models;
using Xamarin.core.Services;
namespace SAEEAPP
{
    [Activity(Label = "Grupos", Theme = "@style/AppTheme")]
    public class GruposActivity : AppCompatActivity
    {
        private FloatingActionButton fab;
        private List<Grupos> listaGrupos = new List<Grupos>();
        ListGruposAdaptador adaptadorGrupos;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_grupos);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += delegate
            {
                //Toast.MakeText(this,"Dialogo agregar",ToastLength.Long).Show();
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                View mView = layoutInflater.Inflate(Resource.Layout.Dialogo_Agregar_Grupos,null);
                Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(this);
                alertDialogBuilder.SetView(mView);
                var etGrupo = mView.FindViewById<EditText>(Resource.Id.etGrupo);
                alertDialogBuilder.SetTitle("Agregando Grupo");
                alertDialogBuilder.SetCancelable(false)
                .SetPositiveButton("Agregar", async delegate {
                    // Toast.MakeText(this, "Grupo: "+txtGrupo.Text, ToastLength.Long).Show();
                    await AgregarGrupoAsync(alertDialogBuilder, etGrupo.Text);
                })
                .SetNegativeButton("Cancelar",delegate {
                    alertDialogBuilder.Dispose();
                
                });
                Android.Support.V7.App.AlertDialog alertDialogAndroid = alertDialogBuilder.Create();
                alertDialogAndroid.Show();


            };
        }

        private async System.Threading.Tasks.Task AgregarGrupoAsync(Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder, string etGrupo)
        {
            DateTime fechaActual = DateTime.Today;
            GruposServices gruposServices = new GruposServices();
            if (!etGrupo.Equals("") && !etGrupo.StartsWith(" "))
            {
                Grupos grupo =
                new Grupos()
                {
                    //Debo traerme el id del profesor
                    IdProfesor = 1,
                    Anio = fechaActual.Year,
                    Grupo = etGrupo

                };
                /* EstudiantesXgrupos EG = new EstudiantesXgrupos() {

                    IdProfesor = 1,
                    IdGrupo = 4,
                    IdEstudiante = 3

                 };
                 grupo.EstudiantesXgrupos.Add (EG);*/
                //listaGrupos.ElementAt(0)
                var _grupo= await gruposServices.PostAsync(grupo);
                Toast.MakeText(this, "Se ha agregado con éxito.", ToastLength.Long).Show();
                listaGrupos.Add(_grupo);
                adaptadorGrupos.NotifyDataSetChanged();
                alertDialogBuilder.Dispose();
            }
            else {
                Toast.MakeText(this, "Debe ingresar un grupo.", ToastLength.Long).Show();

            }

        }
        
        protected override async void OnStart()
        {
            base.OnStart();
            var grupoServicio = new GruposServices();
            var grupoListView = FindViewById<ListView>(Resource.Id.listView);
            //Obtengo el id el profesor
            listaGrupos = await grupoServicio.GetAsync(1);
            TextView tvCargando = FindViewById<TextView>(Resource.Id.tvCargandoG);
            if (listaGrupos.Count == 0)
            {
                tvCargando.Text = "No hay datos";
            }
            else
            {
                adaptadorGrupos = new ListGruposAdaptador(this, listaGrupos);
                tvCargando.Visibility = ViewStates.Gone;
                grupoListView.Adapter = adaptadorGrupos;
            }
        }


    }
}