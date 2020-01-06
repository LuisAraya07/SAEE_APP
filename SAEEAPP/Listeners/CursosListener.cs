using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using SAEEAPP.Adaptadores;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP.Listeners
{
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
            alertDialogAndroid = new AlertDialog.Builder(_context)
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

        private void Borrar(object sender, DialogClickEventArgs e)
        {
#pragma warning disable CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
            BorrarAsync();
#pragma warning restore CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
        }

        private async Task BorrarAsync()
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
                Toast.MakeText(_context, "Error al eliminar, intente nuevamente", ToastLength.Long).Show();
            }
        }
    }

    class EditarCListener : Java.Lang.Object, View.IOnClickListener
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
        private readonly List<Cursos> _cursos;
        private readonly Cursos _curso;
        private readonly CursosListAdapter _cursosAdapter;

        public GruposCListener(Activity context, List<Cursos> cursos, Cursos curso,
            CursosListAdapter cursosAdapter)
        {
            _context = context;
            _cursos = cursos;
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