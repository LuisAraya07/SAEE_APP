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
    class CursosListener : Java.Lang.Object, View.IOnClickListener
    {
        private Activity _context;
        private List<Cursos> _cursos;
        private Cursos _curso;
        private CursosListAdapter _cursosListAdapter;
        private Button _btnOpciones;
        private AlertDialog alertDialogAndroid;
        public CursosListener(Activity context, List<Cursos> cursos, Cursos curso, CursosListAdapter cursosListAdapter, Button btnOpciones)
        {
            _context = context;
            _cursos = cursos;
            _curso = curso;
            _cursosListAdapter = cursosListAdapter;
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

            menu.Inflate(Resource.Layout.menu_popup_grupos);

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
                    case Resource.Id.item3://Grupos
                        OnClick_Grupos();
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

        private void OnClick_Grupos()
        { CursosGruposAgregadosActivity agregarCursoActivity =
                new CursosGruposAgregadosActivity(_context, _cursosListAdapter, _curso);
                agregarCursoActivity.Show();
           
                
        }

        private void OnClick_Borrar()
        {
            alertDialogAndroid = new AlertDialog.Builder(_context, Resource.Style.AlertDialogStyle)
               .SetIcon(Resource.Drawable.trash_can_outline)
               .SetTitle("Eliminando curso")
               .SetMessage($"¿Realmente desea borrar el curso \"{_curso.Nombre}\" y toda su información relacionada?")
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
            CursosServices servicioCursos = new CursosServices();
            VerificarConexion vc = new VerificarConexion(_context);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                bool resultado = await servicioCursos.DeleteCursoAsync(_curso.Id);
                if (resultado)
                {
                    Toast.MakeText(_context, "Se ha eliminado con éxito.", ToastLength.Long).Show();
                    _cursos.Remove(_curso);
                    _cursosListAdapter.ActualizarDatos();
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

            AgregarEditarCursosActivity agregarCursoActivity =
                new AgregarEditarCursosActivity(_context, _cursosListAdapter, _cursos, _curso);
            agregarCursoActivity.Show();

        }
    }



    class BorrarCListener : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Activity _context;
        private readonly List<Cursos> _cursos;
        private readonly Cursos _curso;
        private readonly CursosListAdapter _cursosAdapter;
        AlertDialog alertDialogAndroid;

        public BorrarCListener(Activity context, List<Cursos> cursos, Cursos curso,
            CursosListAdapter cursosAdapter)
        {
            _context = context;
            _cursos = cursos;
            _curso = curso;
            _cursosAdapter = cursosAdapter;
        }

        public void OnClick(View v)
        {
            alertDialogAndroid = new AlertDialog.Builder(_context, Resource.Style.AlertDialogStyle)
              .SetIcon(Resource.Drawable.trash_can_outline)
              .SetTitle("Eliminando curso")
              .SetMessage($"¿Realmente desea borrar el curso \"{_curso.Nombre}\" y toda su información relacionada?")
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
            CursosServices servicioCursos = new CursosServices();
            bool resultado = await servicioCursos.DeleteCursoAsync(_curso.Id);
            if (resultado)
            {
                Toast.MakeText(_context, "Se ha eliminado con éxito.", ToastLength.Long).Show();
                _cursos.Remove(_curso);
                _cursosAdapter.ActualizarDatos();
            }
            else
            {
                Toast.MakeText(_context, "Error al eliminar, intente nuevamente", ToastLength.Short).Show();
            }
        }
    }

    class EditarCListener 
    {
        private readonly Activity _context;
        private readonly List<Cursos> _cursos;
        private readonly Cursos _curso;
        private readonly CursosListAdapter _cursosAdapter;

        public EditarCListener(Activity context, List<Cursos> cursos, Cursos curso,
            CursosListAdapter cursosAdapter)
        {
            _context = context;
            _cursos = cursos;
            _curso = curso;
            _cursosAdapter = cursosAdapter;
            
        }

        public void OnClick(View v)
        {
            AgregarEditarCursosActivity agregarCursoActivity =
                new AgregarEditarCursosActivity(_context, _cursosAdapter, _cursos, _curso);
            agregarCursoActivity.Show();
        }
    }

    class GruposCListener : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Activity _context;
        private readonly Cursos _curso;
        private readonly CursosListAdapter _cursosAdapter;

        public GruposCListener(Activity context, Cursos curso,
            CursosListAdapter cursosAdapter)
        {
            _context = context;
            _curso = curso;
            _cursosAdapter = cursosAdapter;
        }

        public void OnClick(View v)
        {
            CursosGruposAgregadosActivity agregarCursoActivity =
                new CursosGruposAgregadosActivity(_context, _cursosAdapter, _curso);
            agregarCursoActivity.Show();
        }
    }
}