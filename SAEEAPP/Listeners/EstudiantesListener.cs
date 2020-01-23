using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang.Reflect;
using SAEEAPP.Adaptadores;
using System;
using System.Collections.Generic;
using Xamarin.core;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP.Listeners
{
    internal class EstudiantesListener : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Activity _context;
        private readonly List<Estudiantes> _estudiantes;
        private readonly Estudiantes estudiante;
        private readonly ListEstudiantesAdaptador listEstudiantesAdaptador;
        private readonly Button btnOpciones;

        public EstudiantesListener(Activity context, List<Estudiantes> estudiantes, Estudiantes estudiante, ListEstudiantesAdaptador listEstudiantesAdaptador, Button btnOpciones)
        {
            _context = context;
            _estudiantes = estudiantes;
            this.estudiante = estudiante;
            this.listEstudiantesAdaptador = listEstudiantesAdaptador;
            this.btnOpciones = btnOpciones;
        }

        public void OnClick(View v)
        {
            Context wrapper = new ContextThemeWrapper(_context, Resource.Style.PopupTheme);
            PopupMenu menu = new PopupMenu(wrapper, btnOpciones);
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

        private void OnClick_Eliminar()
        {
            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(_context, Resource.Style.AlertDialogStyle);
            alertDialogBuilder.SetIcon(Resource.Drawable.trash_can_outline)
              .SetCancelable(false)
              .SetTitle("¿Está seguro?")
              .SetMessage("Quiere eliminar al estudiante: " + $"{estudiante.Nombre} {estudiante.PrimerApellido} {estudiante.SegundoApellido}")
              .SetPositiveButton("Sí", delegate
              {
                  EliminarEstudiante(estudiante);
              })
              .SetNegativeButton("No", delegate
              {
                  alertDialogBuilder.Dispose();

              })
              .Show();

        }
        private async void EliminarEstudiante(Estudiantes estudiante)
        {
            VerificarConexion vc = new VerificarConexion(_context);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                EstudiantesServices estudiantesServices = new EstudiantesServices();
                await estudiantesServices.DeleteEstudiantesAsync(estudiante);
                Toast.MakeText(_context, "Se ha eliminado con éxito.", ToastLength.Long).Show();
                _estudiantes.Remove(estudiante);
                listEstudiantesAdaptador.ActualizarDatos();
            }
            else
            {
                // Toast.MakeText(_context, "Necesita conexión a internet.", ToastLength.Long).Show();
                ProfesoresServices ns = new ProfesoresServices(1);
                Profesores profesor = await ns.GetProfesorConectado();
                EstudiantesServices estudiantesServices = new EstudiantesServices(profesor.Id);
                var respuesta = await estudiantesServices.DeleteEstudiantesOffline(estudiante);
                if (respuesta)
                {
                    _estudiantes.Remove(estudiante);
                    listEstudiantesAdaptador.ActualizarDatos();
                    Toast.MakeText(_context, "Se ha eliminado con éxito.", ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(_context, "No se ha podido eliminar.", ToastLength.Long).Show();
                }
               
            }
        }
        private void OnClick_Editar()
        {
            AgregarEstudianteActivity agregarEditarEstudianteActivity =
                new AgregarEstudianteActivity(_context, listEstudiantesAdaptador, _estudiantes, estudiante);
            agregarEditarEstudianteActivity.Show();
        }
    }
}