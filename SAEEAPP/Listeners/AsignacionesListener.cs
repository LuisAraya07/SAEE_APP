using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang.Reflect;
using SAEEAPP.Adaptadores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.core;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP.Listeners
{
    class AsignacionesListener : Java.Lang.Object, View.IOnClickListener
    {
        private Activity _context;
        private List<Asignaciones> _asignaciones;
        private Asignaciones _asignacion;
        private AsignacionesAdaptador _adapter;
        private Button _btnOpciones;
        private AlertDialog alertDialogAndroid;

        private List<Cursos> _cursos;
        public AsignacionesListener(Activity context, List<Asignaciones> asignaciones, Asignaciones asignacion, AsignacionesAdaptador adapter, Button btnOpciones,List<Cursos> cursos)
        {
            _context = context;
            _asignaciones = asignaciones;
            _asignacion = asignacion;
            _adapter = adapter;
            _btnOpciones = btnOpciones;
            _cursos = cursos;
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
        private void OnClick_Borrar()
        {
            alertDialogAndroid = new AlertDialog.Builder(_context, Resource.Style.AlertDialogStyle)
               .SetIcon(Resource.Drawable.trash_can_outline)
               .SetTitle("Eliminando asignación")
               .SetMessage($"¿Realmente desea borrar el asignación \"{_asignacion.Nombre}\" y toda su información relacionada?")
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
            AsignacionesServices servicioAsignaciones = new AsignacionesServices();
            EvaluacionesServices servicioEvaluaciones = new EvaluacionesServices();
            VerificarConexion vc = new VerificarConexion(_context);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                List<Evaluaciones> evaluaciones = await servicioEvaluaciones.GetEvaluacionesxAsignacionAsync(_asignacion.Id);
                foreach(Evaluaciones eva in evaluaciones)
                {
                    await servicioEvaluaciones.DeleteEvaluacionAsync(eva.Id);
                }
                bool resultado = await servicioAsignaciones.DeleteAsignacionAsync(_asignacion.Id);
                if (resultado)
                {
                    Toast.MakeText(_context, "Se ha eliminado con éxito.", ToastLength.Long).Show();
                    _asignaciones.Remove(_asignacion);
                    _adapter.ActualizarDatos();
                }
                else
                {
                    Toast.MakeText(_context, "Error al eliminar, intente nuevamente", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(_context,"Necesita conexión a internet.",ToastLength.Long).Show();
            }
        }
        private void OnClick_Editar()
        {

            AgregarEditarAsignacionesActivity agregarAsignacionActivity =
                new AgregarEditarAsignacionesActivity(_context, _adapter, _asignaciones, _asignacion,_cursos);
            agregarAsignacionActivity.Show();
          
        }
    }



    class BorrarAListener : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Activity _context;
        private readonly List<Asignaciones> _asignaciones;
        private readonly Asignaciones _asignacion;
        private readonly AsignacionesAdaptador _adapter;
        AlertDialog alertDialogAndroid;

        public BorrarAListener(Activity context, List<Asignaciones> asignaciones, Asignaciones asignacion,AsignacionesAdaptador adapter)
        {
            _context = context;
            _asignaciones = asignaciones;
            _asignacion = asignacion;
            _adapter = adapter;
        }

        public void OnClick(View v)
        {
            alertDialogAndroid = new AlertDialog.Builder(_context, Resource.Style.AlertDialogStyle)
              .SetIcon(Resource.Drawable.trash_can_outline)
              .SetTitle("Eliminando asignación")
              .SetMessage($"¿Realmente desea borrar el asignación \"{_asignacion.Nombre}\" y toda su información relacionada?")
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
            AsignacionesServices servicioAsignaciones = new AsignacionesServices();
            bool resultado = await servicioAsignaciones.DeleteAsignacionAsync(_asignacion.Id);
            if (resultado)
            {
                Toast.MakeText(_context, "Se ha eliminado con éxito.", ToastLength.Long).Show();
                _asignaciones.Remove(_asignacion);
                _adapter.ActualizarDatos();
            }
            else
            {
                Toast.MakeText(_context, "Error al eliminar, intente nuevamente", ToastLength.Short).Show();
            }
        }
    }

    class EditarAListener 
    {
        private readonly Activity _context;
        private readonly List<Asignaciones> _asignaciones;
        private readonly Asignaciones _asignacion;
        private readonly AsignacionesAdaptador _adapter;
        private readonly List<Cursos> _cursos;

        public EditarAListener(Activity context, List<Asignaciones> asignaciones, Asignaciones asignacion, AsignacionesAdaptador adapter,List<Cursos> cursos)
        {
            _context = context;
            _asignaciones = asignaciones;
            _asignacion = asignacion;
            _adapter = adapter;
            _cursos = cursos;
        }

        public void OnClick(View v)
        {
            AgregarEditarAsignacionesActivity agregarAsignacionActivity =
                new AgregarEditarAsignacionesActivity(_context, _adapter, _asignaciones, _asignacion,_cursos);
            agregarAsignacionActivity.Show();
        }
    }

}