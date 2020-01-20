using Android.App;
using Android.Content;
using Android.Support.V7.View.Menu;
using Android.Views;
using Android.Widget;
using Java.Lang.Reflect;
using SAEEAPP.Adaptadores;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.core;
using Xamarin.core.Models;
using Xamarin.core.Services;
using Field = Java.Lang.Reflect.Field;

namespace SAEEAPP.Listeners
{
    internal class GruposListener : Java.Lang.Object, View.IOnClickListener
    {
        private Activity _context;
        private List<Grupos> _grupos;
        private Grupos grupo;
        private ListGruposAdaptador listGruposAdaptador;
        private Button _btnOpciones;

        public GruposListener(Activity context, List<Grupos> grupos, Grupos grupo, ListGruposAdaptador listGruposAdaptador, Button btnOpciones)
        {
            _context = context;
            _grupos = grupos;
            this.grupo = grupo;
            this.listGruposAdaptador = listGruposAdaptador;
            this._btnOpciones = btnOpciones;
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
            
            menu.Inflate(Resource.Layout.menu_popup_estudiantes);

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
                    case Resource.Id.item3://Estudiantes
                        OnClick_Estudiantes();
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

        

        private void OnClick_Eliminar()
        {
            VerificarConexion vc = new VerificarConexion(_context);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(_context, Resource.Style.AlertDialogStyle);
                alertDialogBuilder.SetCancelable(false)
                .SetIcon(Resource.Drawable.trash_can_outline)
                   .SetTitle("¿Está seguro?")
                   .SetMessage("Quiere eliminar el grupo: " + grupo.Grupo)
                   .SetPositiveButton("Sí", async delegate
                   {
                       GruposServices gruposServices = new GruposServices();
                       await gruposServices.DeleteGruposAsync(grupo);
                       Toast.MakeText(_context, "Se ha eliminado con éxito.", ToastLength.Long).Show();
                       _grupos.Remove(grupo);
                       listGruposAdaptador.ActualizarDatos();
                   })
                   .SetNegativeButton("No", delegate
                   {
                       alertDialogBuilder.Dispose();
                   })
                   .Show();
            }
            else
            {
                Toast.MakeText(_context, "Necesita conexión a internet.", ToastLength.Long).Show();
            }
        }

        private void OnClick_Editar()
        {
            VerificarConexion vc = new VerificarConexion(_context);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                LayoutInflater layoutInflater = LayoutInflater.From(_context);
                View mView = layoutInflater.Inflate(Resource.Layout.Dialogo_Agregar_Grupos, null);
                Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(_context, Resource.Style.AlertDialogStyle);
                alertDialogBuilder.SetView(mView);
                mView.FindViewById<EditText>(Resource.Id.etGrupo).Text = grupo.Grupo;
                alertDialogBuilder.SetTitle("Editando Grupo");
                alertDialogBuilder.SetCancelable(false)
                .SetPositiveButton("Guardar", delegate
                {
                    EditarGrupoAsync(alertDialogBuilder, mView);
                })
                .SetNegativeButton("Cancelar", delegate
                {
                    alertDialogBuilder.Dispose();

                });
                Android.Support.V7.App.AlertDialog alertDialogAndroid = alertDialogBuilder.Create();
                alertDialogAndroid.Show();
            }
            else
            {
                Toast.MakeText(_context, "Necesita conexión a internet.", ToastLength.Long).Show();
            }
        }

        private async void EditarGrupoAsync(Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder, View mView)
        {
            DateTime fechaActual = DateTime.Today;
            GruposServices gruposServices = new GruposServices();
            var etGrupo = mView.FindViewById<EditText>(Resource.Id.etGrupo).Text;
            if (!etGrupo.Equals("") && !etGrupo.StartsWith(" "))
            {
                grupo.Grupo = etGrupo;
                grupo.Anio = fechaActual.Year;
                VerificarConexion vc = new VerificarConexion(_context);
                var conectado = vc.IsOnline();
                if (conectado)
                {
                    var actualizado = await gruposServices.PutAsync(grupo);
                    if (actualizado)
                    {
                        Toast.MakeText(_context, "Se ha editado con éxito.", ToastLength.Long).Show();
                        _grupos.Where(x => x.Id == grupo.Id).FirstOrDefault().Grupo = etGrupo;
                        listGruposAdaptador.ActualizarDatos();
                        alertDialogBuilder.Dispose();
                    }
                    else
                    {
                        Toast.MakeText(_context, "Error al editar grupo.", ToastLength.Long).Show();
                        alertDialogBuilder.Dispose();
                    }
                }
                else
                {
                    Toast.MakeText(_context, "Necesita conexión a internet.", ToastLength.Long).Show();

                }

            }
            else
            {
                Toast.MakeText(_context, "Debe ingresar un grupo.", ToastLength.Long).Show();
            }
        }
        private async void OnClick_Estudiantes()
        {
            VerificarConexion vc = new VerificarConexion(_context);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                List<Estudiantes> listaEstudiantes = new List<Estudiantes>();
                List<EstudiantesXgrupos> listaEG = new List<EstudiantesXgrupos>();
                GruposServices gruposServicios = new GruposServices();
                //Cada vez vamos a obtener los estudiantes de ese grupo
                listaEG = await gruposServicios.GetEGAsync(grupo.Id);//grupo.EstudiantesXgrupos.Select(x => x.IdEstudianteNavigation).ToList();
                listaEstudiantes = listaEG.Select(x => x.IdEstudianteNavigation).ToList();
                var estudiantesListView = _context.FindViewById<ListView>(Resource.Id.listViewEG);
                ListEGAdaptador adaptadorEG = new ListEGAdaptador(_context, listaEstudiantes, listaEG);
                Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(_context, Resource.Style.AlertDialogStyle);
                alertDialogBuilder.SetCancelable(true)
                .SetTitle("Estudiantes del Grupo")
                .SetView(estudiantesListView)
                .SetAdapter(adaptadorEG, (s, e) =>
                {
                    var index = e.Which;
                })
                .SetNegativeButton("Cerrar", delegate
                {
                    alertDialogBuilder.Dispose();

                })
                .SetPositiveButton("Añadir", delegate
                {
                    //Toast.MakeText(_context,"Aqui agregamos",ToastLength.Short).Show();
                    MostrarLVAgregarAsync(listaEstudiantes, listaEG);


                });

                Android.Support.V7.App.AlertDialog alertDialogAndroid = alertDialogBuilder.Create();
                alertDialogAndroid.Show();
            }
            else
            {
                Toast.MakeText(_context, "Necesita conexión a internet.", ToastLength.Long).Show();
            }
        }

        private async void MostrarLVAgregarAsync(List<Estudiantes> listaAgregados, List<EstudiantesXgrupos> listaEG)
        {
            VerificarConexion vc = new VerificarConexion(_context);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                EstudiantesServices estudiantesServicios = new EstudiantesServices();
                List<Estudiantes> listaTemporal = new List<Estudiantes>();
                listaTemporal = await estudiantesServicios.GetAsync();
                var listaAgregar = listaTemporal.Where(st => !(listaAgregados.Select(x => x.Id).Contains(st.Id))).ToList();
                var estudiantesListView = _context.FindViewById<ListView>(Resource.Id.listViewEGA);
                ListAgregarEGAdaptador adaptadorEG = new ListAgregarEGAdaptador(_context, listaAgregar, listaEG, grupo.Id);
                Android.Support.V7.App.AlertDialog.Builder alertDialogBuilderAgregar = new Android.Support.V7.App.AlertDialog.Builder(_context, Resource.Style.AlertDialogStyle);
                alertDialogBuilderAgregar.SetCancelable(true)
                .SetTitle("Estudiantes Para Agregar")
                .SetAdapter(adaptadorEG, (s, e) =>
                {
                    var index = e.Which;
                })
                .SetNegativeButton("Cerrar", delegate
                {
                    alertDialogBuilderAgregar.Dispose();
                });
                Android.Support.V7.App.AlertDialog alertDialogAndroidAgregar = alertDialogBuilderAgregar.Create();
                alertDialogAndroidAgregar.Show();
            }
            else
            {
                Toast.MakeText(_context, "Necesita conexión a internet.", ToastLength.Long).Show();
            }

        }
    }
}