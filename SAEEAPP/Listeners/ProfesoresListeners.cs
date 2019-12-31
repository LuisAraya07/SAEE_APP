using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SAEEAPP.Adaptadores;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP.Listeners
{
    class BorrarPListener : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Activity _context;
        private readonly List<Profesores> _profesores;
        private readonly Profesores _profesor;
        private readonly ProfesoresListAdapter _profesoresAdapter;

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
            new AlertDialog.Builder(_context)
              .SetIcon(Resource.Drawable.trash_can_outline)
              .SetTitle("Eliminando profesor")
              .SetMessage($"¿Realmente desea borrar al profesor \"{_profesor.Nombre} {_profesor.PrimerApellido} {_profesor.SegundoApellido}\" y toda su información relacionada?")
              .SetPositiveButton("Borrar", Borrar)
              .SetNegativeButton("Cancelar", delegate
              {
                  Dispose();
              })
              .Show();
        }

        private void Borrar(object sender, DialogClickEventArgs e)
        {
            BorrarAsync();
        }

        private async Task BorrarAsync()
        {
            ProfesoresServices servicioProfesores = new ProfesoresServices();
            bool resultado = await servicioProfesores.DeleteProfesorAsync(_profesor.Id);
            Toast.MakeText(_context, "Se ha eliminado con éxito.", ToastLength.Long).Show();
            _profesores.Remove(_profesor);
            _profesoresAdapter.ActualizarDatos();
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