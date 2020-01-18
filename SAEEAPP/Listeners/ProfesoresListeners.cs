using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang.Reflect;
using SAEEAPP.Adaptadores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP.Listeners
{

    class ProfesoresListener:Java.Lang.Object, View.IOnClickListener
    {
        private Activity _context;
        private List<Profesores> _profesores;
        private Profesores _profesor;
        private ProfesoresListAdapter _profesoresListAdapter;
        private Button _btnOpciones;
        private AlertDialog alertDialogAndroid;
        public ProfesoresListener(Activity context, List<Profesores> profesores, Profesores profesor, ProfesoresListAdapter profesoresListAdapter, Button btnOpciones)
        {
            _context = context;
            _profesores = profesores;
            _profesor = profesor;
            _profesoresListAdapter = profesoresListAdapter;
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
                        OnClick_Borrar();
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

        private void OnClick_Editar()
        {
            AgregarEditarProfesorActivity agregarProfesorActivity =
                 new AgregarEditarProfesorActivity(_context, _profesoresListAdapter, _profesores, _profesor);
            agregarProfesorActivity.Show();
        }

        private void OnClick_Borrar()
        {
            alertDialogAndroid = new AlertDialog.Builder(_context, Resource.Style.AlertDialogStyle)
              .SetIcon(Resource.Drawable.trash_can_outline)
              .SetTitle("Eliminando profesor")
              .SetMessage($"¿Realmente desea borrar al profesor \"{_profesor.Nombre} {_profesor.PrimerApellido} {_profesor.SegundoApellido}\" y toda su información relacionada?")
              .SetPositiveButton("Borrar", Borrar)
              .SetNegativeButton("Cancelar", Cancelar)
              .Create();
            alertDialogAndroid.Show();
        }
        private void Cancelar(object sender, DialogClickEventArgs e)
        {
            alertDialogAndroid.Dismiss();
        }

        private async void Borrar(object sender, DialogClickEventArgs e)
        {
            ProfesoresServices servicioProfesores = new ProfesoresServices();
            bool resultado = await servicioProfesores.DeleteProfesorAsync(_profesor.Id);
            if (resultado)
            {
                Toast.MakeText(_context, "Se ha eliminado con éxito.", ToastLength.Long).Show();
                _profesores.Remove(_profesor);
                _profesoresListAdapter.ActualizarDatos();
            }
            else
            {
                Toast.MakeText(_context, "Error al eliminar, intente nuevamente", ToastLength.Short).Show();
            }
        }
    }


    class BorrarPListener : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Activity _context;
        private readonly List<Profesores> _profesores;
        private readonly Profesores _profesor;
        private readonly ProfesoresListAdapter _profesoresAdapter;
        AlertDialog alertDialogAndroid;

        public BorrarPListener(Activity context, List<Profesores> profesores, Profesores profesor,
            ProfesoresListAdapter profesoresAdapter)
        {
            _context = context;
            _profesores = profesores;
            _profesor = profesor;
            _profesoresAdapter = profesoresAdapter;
        }

        public void OnClick(View v)
        {
            alertDialogAndroid = new AlertDialog.Builder(_context, Resource.Style.AlertDialogStyle)
              .SetIcon(Resource.Drawable.trash_can_outline)
              .SetTitle("Eliminando profesor")
              .SetMessage($"¿Realmente desea borrar al profesor \"{_profesor.Nombre} {_profesor.PrimerApellido} {_profesor.SegundoApellido}\" y toda su información relacionada?")
              .SetPositiveButton("Borrar", Borrar)
              .SetNegativeButton("Cancelar", Cancelar)
              .Create();
            alertDialogAndroid.Show();
        }

        private void Cancelar(object sender, DialogClickEventArgs e)
        {
            alertDialogAndroid.Dismiss();
        }

        private async void Borrar(object sender, DialogClickEventArgs e)
        {
            ProfesoresServices servicioProfesores = new ProfesoresServices();
            bool resultado = await servicioProfesores.DeleteProfesorAsync(_profesor.Id);
            if (resultado)
            {
                Toast.MakeText(_context, "Se ha eliminado con éxito.", ToastLength.Long).Show();
                _profesores.Remove(_profesor);
                _profesoresAdapter.ActualizarDatos();
            }
            else
            {
                Toast.MakeText(_context, "Error al eliminar, intente nuevamente", ToastLength.Short).Show();
            }
        }
    }

    class EditarPListener : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Activity _context;
        private readonly List<Profesores> _profesores;
        private readonly Profesores _profesor;
        private readonly ProfesoresListAdapter _profesoresAdapter;

        public EditarPListener(Activity context, List<Profesores> profesores, Profesores profesor,
            ProfesoresListAdapter profesoresAdapter)
        {
            _context = context;
            _profesores = profesores;
            _profesor = profesor;
            _profesoresAdapter = profesoresAdapter;
        }

        public void OnClick(View v)
        {
            AgregarEditarProfesorActivity agregarProfesorActivity =
                new AgregarEditarProfesorActivity(_context, _profesoresAdapter, _profesores, _profesor);
            agregarProfesorActivity.Show();
        }
    }
}