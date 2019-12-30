using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP.Adaptadores
{
    class ListEstudiantesAdaptador : BaseAdapter<Estudiantes>
    {
        private readonly Activity _context;
        private readonly List<Estudiantes> _estudiantes;

        public ListEstudiantesAdaptador(Activity context, List<Estudiantes> estudiantes)
        {
            _context = context;
            _estudiantes = estudiantes;
        }

        public override Estudiantes this[int position] => _estudiantes[position];

        public override int Count => _estudiantes.Count;

        public override long GetItemId(int position)
        {
            return this[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Estudiantes estudiante = this[position];

            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.EstudiantesListRow, null);
            }
            convertView.
                FindViewById<TextView>(Resource.Id.textViewNombreE).
                Text = $"{estudiante.Nombre} {estudiante.PrimerApellido} {estudiante.SegundoApellido}";
            convertView.FindViewById<TextView>(Resource.Id.textViewCedE).Text = estudiante.Cedula;
            Button btBorrar = convertView.FindViewById<Button>(Resource.Id.btBorrarE);
            btBorrar.Click += delegate
            {
                OnClick_Borrar(estudiante);

            };
            Button btEditar = convertView.FindViewById<Button>(Resource.Id.btEditarE);
            btEditar.Click += delegate
            {
                OnClick_Editar(estudiante);

            };
            return convertView;
        }
        public void OnClick_Borrar(Estudiantes estudiante)
        {
            new AlertDialog.Builder(_context)
              .SetIcon(Resource.Drawable.trash_can_outline)
              .SetTitle("¿Está seguro?")
              .SetMessage("Quiere eliminar al estudiante: " + $"{estudiante.Nombre} {estudiante.PrimerApellido} {estudiante.SegundoApellido}")
              .SetPositiveButton("Sí", delegate
              {
                  EstudiantesServices gruposServices = new EstudiantesServices();
                  gruposServices.DeleteEstudiantesAsync(estudiante);
                  Toast.MakeText(_context, "Se ha eliminado con éxito.", ToastLength.Long).Show();
                  _estudiantes.Remove(estudiante);
                  this.NotifyDataSetChanged();
              })
              .SetNegativeButton("No", delegate
              {
                  this.Dispose();

              })
              .Show();
        }
        public void OnClick_Editar(Estudiantes estudiante)
        {
        }
    }
}