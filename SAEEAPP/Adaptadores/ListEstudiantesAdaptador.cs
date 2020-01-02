using Android.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP.Adaptadores
{
    public class ListEstudiantesAdaptador : BaseAdapter<Estudiantes>
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
            View view = convertView;
            if (view == null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.EstudiantesListRow, null);
            }
            Button btBorrar = view.FindViewById<Button>(Resource.Id.btBorrarE);
            btBorrar.SetTag(Resource.Id.btBorrarE, position);
            btBorrar.Click -= OnClick_Borrar;
            btBorrar.Click += OnClick_Borrar;
            Button btEditar = view.FindViewById<Button>(Resource.Id.btEditarE);
            btEditar.SetTag(Resource.Id.btEditarE, position);
            btEditar.Click -= OnClick_Editar;
            btEditar.Click += OnClick_Editar;
            view.
                FindViewById<TextView>(Resource.Id.textViewNombreE).
                Text = $"{estudiante.Nombre} {estudiante.PrimerApellido} {estudiante.SegundoApellido}";
            view.FindViewById<TextView>(Resource.Id.textViewCedE).Text = estudiante.Cedula;
            return view;
        }
        public void OnClick_Borrar(object sender, EventArgs e)
        {
            int i = (int)((Button)sender).GetTag(Resource.Id.btBorrarE);
            var estudiante = _estudiantes.ElementAt(i);
            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(_context);
            alertDialogBuilder.SetIcon(Resource.Drawable.trash_can_outline)
              .SetCancelable(false)
              .SetTitle("¿Está seguro?")
              .SetMessage("Quiere eliminar al estudiante: " + $"{estudiante.Nombre} {estudiante.PrimerApellido} {estudiante.SegundoApellido}")
              .SetPositiveButton("Sí", async delegate
              {
                  EstudiantesServices gruposServices = new EstudiantesServices();
                  await gruposServices.DeleteEstudiantesAsync(estudiante);
                  Toast.MakeText(_context, "Se ha eliminado con éxito.", ToastLength.Long).Show();
                  _estudiantes.RemoveAt(i);
                  NotifyDataSetChanged();
              })
              .SetNegativeButton("No", delegate
              {
                  alertDialogBuilder.Dispose();

              })
              .Show();
        }
        public void OnClick_Editar(object sender, EventArgs e)
        {
            int i = (int)((Button)sender).GetTag(Resource.Id.btEditarE);
            var estudiante = _estudiantes.ElementAt(i);
            AgregarEstudianteActivity agregarEditarEstudianteActivity =
                new AgregarEstudianteActivity(_context, this, _estudiantes, estudiante);
            agregarEditarEstudianteActivity.Show();
        }

        public void ActualizarDatos()
        {
            NotifyDataSetChanged();
        }
    }
}