using Android.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.core.Models;
using Xamarin.core.Services;
using Object = Java.Lang.Object;
namespace SAEEAPP.Adaptadores
{
    public class ListEstudiantesAdaptador : BaseAdapter<Estudiantes>, IFilterable
    {
        private readonly Activity _context;
        private List<Estudiantes> _estudiantes;
        private  List<Estudiantes> datosOriginales;

        public ListEstudiantesAdaptador(Activity context, List<Estudiantes> estudiantes)
        {
            _context = context;
            _estudiantes = estudiantes;
            Filter = new EstudiantesFilter(this);
        }

        public override Estudiantes this[int position] => _estudiantes[position];

        public override int Count => _estudiantes.Count;

        public Filter Filter { get; private set; }

        public override long GetItemId(int position)
        {
            return this[position].Id;
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return position;
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
            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(_context, Resource.Style.AlertDialogStyle);
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
        // FILTRO
        private class EstudiantesFilter : Filter
        {
            private readonly ListEstudiantesAdaptador _adapter;
            public EstudiantesFilter(ListEstudiantesAdaptador adapter)
            {
                _adapter = adapter;
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var returnObj = new FilterResults();
                var results = new List<Estudiantes>();
                if (_adapter.datosOriginales == null)
                    _adapter.datosOriginales = _adapter._estudiantes;

                if (constraint == null) return returnObj;

                if (_adapter.datosOriginales != null && _adapter.datosOriginales.Any())
                {
                    results.AddRange(
                        _adapter.datosOriginales.Where(
                            estudiante => (estudiante.Nombre + estudiante.PrimerApellido + estudiante.SegundoApellido + estudiante.Cedula).ToLower().Contains(constraint.ToString())));
                }

                returnObj.Values =FromArray(results.Select(r => r.ToJavaObject()).ToArray());
                returnObj.Count = results.Count;

                constraint.Dispose();

                return returnObj;
            }
            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                using (var values = results.Values)
                    _adapter._estudiantes = values.ToArray<Object>()
                        .Select(r => r.ToNetObject<Estudiantes>()).ToList();
                _adapter.NotifyDataSetChanged();
                constraint.Dispose();
                results.Dispose();
            }

          
        }
    }
}