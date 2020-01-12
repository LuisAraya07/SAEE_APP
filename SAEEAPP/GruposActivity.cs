using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SAEEAPP.Adaptadores;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.core.Models;
using Xamarin.core.Services;
namespace SAEEAPP
{
    [Activity(Label = "@string/grupo", Theme = "@style/AppTheme")]
    public class GruposActivity : AppCompatActivity
    {
        private FloatingActionButton fab;
        private List<Grupos> listaGrupos = new List<Grupos>();
        ListGruposAdaptador adaptadorGrupos;
        ListView grupoListView;
        ProgressBar pbCargandoGrupos;
        TextView tvCargando;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_grupos);
            grupoListView = FindViewById<ListView>(Resource.Id.listView);
            tvCargando = FindViewById<TextView>(Resource.Id.tvCargandoG);
            pbCargandoGrupos = FindViewById<ProgressBar>(Resource.Id.pbCargandoGrupos);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Visibility = ViewStates.Invisible;
            fab.Click += delegate
            {
                //Toast.MakeText(this,"Dialogo agregar",ToastLength.Long).Show();
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                View mView = layoutInflater.Inflate(Resource.Layout.Dialogo_Agregar_Grupos, null);
                Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AlertDialogStyle);
                alertDialogBuilder.SetView(mView);
                var etGrupo = mView.FindViewById<EditText>(Resource.Id.etGrupo);
                alertDialogBuilder.SetTitle("Agregando Grupo");
                alertDialogBuilder.SetCancelable(false)
                .SetPositiveButton("Agregar", delegate
                {
                    AgregarGrupoAsync(alertDialogBuilder, etGrupo.Text);
                })
                .SetNegativeButton("Cancelar", delegate
                {
                    alertDialogBuilder.Dispose();

                });
                Android.Support.V7.App.AlertDialog alertDialogAndroid = alertDialogBuilder.Create();
                alertDialogAndroid.Show();


            };
        }

        private async void AgregarGrupoAsync(Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder, string etGrupo)
        {
            DateTime fechaActual = DateTime.Today;
            GruposServices gruposServices = new GruposServices();
            if (!etGrupo.Equals("") && !etGrupo.StartsWith(" "))
            {
                Grupos grupo =
                new Grupos()
                {
                    
                    Anio = fechaActual.Year,
                    Grupo = etGrupo

                };
                HttpResponseMessage resultado = await gruposServices.PostAsync(grupo);
                if (resultado.IsSuccessStatusCode)
                {
                    string resultadoString = await resultado.Content.ReadAsStringAsync();
                    var grupoNuevo = JsonConvert.DeserializeObject<Grupos>(resultadoString);
                    VerificarLista(grupoNuevo);
                    Toast.MakeText(this, "Se ha agregado con éxito.", ToastLength.Long).Show();
                    alertDialogBuilder.Dispose();

                }
                else
                {
                    Toast.MakeText(this, "Error al agregar, intente nuevamente", ToastLength.Long).Show();
                }

            }
            else
            {
                Toast.MakeText(this, "Debe ingresar un grupo.", ToastLength.Long).Show();
            }

        }
        public void VerificarLista(Grupos grupoNuevo)
        {
            if (listaGrupos.Count == 0)
            {
                TextView tvCargando = FindViewById<TextView>(Resource.Id.tvCargandoG);
                adaptadorGrupos = new ListGruposAdaptador(this, listaGrupos);
                tvCargando.Visibility = ViewStates.Gone;
                grupoListView.Adapter = adaptadorGrupos;
            }
            else
            {
                listaGrupos.Add(grupoNuevo);
                adaptadorGrupos.NotifyDataSetChanged();
            }


        }
        protected override async void OnStart()
        {
            base.OnStart();
            var grupoServicio = new GruposServices();
            //Obtengo el id el profesor
            listaGrupos = await grupoServicio.GetAsync();
            if (listaGrupos.Count == 0)
            {
                tvCargando.Text = "No hay datos";
                pbCargandoGrupos.Visibility = ViewStates.Gone;
            }
            else
            {
                adaptadorGrupos = new ListGruposAdaptador(this, listaGrupos);
                tvCargando.Visibility = ViewStates.Gone;
                pbCargandoGrupos.Visibility = ViewStates.Gone;
                grupoListView.Visibility = ViewStates.Visible;
                grupoListView.Adapter = adaptadorGrupos;
            }
            fab.Visibility = ViewStates.Visible;
        }


    }
}