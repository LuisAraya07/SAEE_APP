using Android.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using SAEEAPP.Listeners;
using System.Collections.Generic;
using System.Linq;
using Xamarin.core.Models;

namespace SAEEAPP.Adaptadores
{
    public class ProfesoresListAdapter : BaseAdapter<Profesores>, IFilterable
    {
        private readonly Activity _context;
        private List<Profesores> _profesores;
        private List<Profesores> datosOriginales;
        private readonly TextView _tvCargando;
        public ProfesoresListAdapter(Activity context, List<Profesores> profesores, TextView tvCargando)
        {
            _context = context;
            _profesores = profesores;
            _tvCargando = tvCargando;
            tvCargando.Text = "No hay datos";
            Filter = new ProfesoresFilter(this);
        }

        public override Profesores this[int position] => _profesores[position];
        public Filter Filter { get; private set; }
        public override int Count => _profesores.Count;

        public override long GetItemId(int position)
        {
            return this[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Profesores profesor = this[position];

            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.ProfesoresListRow, null);
            }
            TextView tvNombre = convertView.FindViewById<TextView>(Resource.Id.tvNombre);
            TextView tvCedula = convertView.FindViewById<TextView>(Resource.Id.tvCedula);
            TextView tvCorreo = convertView.FindViewById<TextView>(Resource.Id.tvCorreo);
            Button btEditar = convertView.FindViewById<Button>(Resource.Id.btEditar);
            Button btBorrar = convertView.FindViewById<Button>(Resource.Id.btBorrar);

            tvNombre.Text = $"{profesor.Nombre} {profesor.PrimerApellido} {profesor.SegundoApellido}";
            tvCedula.Text = profesor.Cedula;
            tvCorreo.Text = profesor.Correo;

            btEditar.SetOnClickListener(new EditarPListener(_context, _profesores, profesor, this));
            btBorrar.SetOnClickListener(new BorrarPListener(_context, _profesores, profesor, this));

            return convertView;
        }
        public void ActualizarDatos()
        {
            _tvCargando.Visibility = (_profesores.Count > 0) ? ViewStates.Invisible : ViewStates.Visible;
            NotifyDataSetChanged();
        }

        // FILTRO
        private class ProfesoresFilter : Filter
        {
            private readonly ProfesoresListAdapter _adapter;
            public ProfesoresFilter(ProfesoresListAdapter adapter)
            {
                _adapter = adapter;
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var returnObj = new FilterResults();
                var results = new List<Profesores>();
                if (_adapter.datosOriginales == null)
                    _adapter.datosOriginales = _adapter._profesores;

                if (constraint == null) return returnObj;

                if (_adapter.datosOriginales != null && _adapter.datosOriginales.Any())
                {
                    results.AddRange(
                        _adapter.datosOriginales.Where(
                            profesor => (profesor.Nombre + profesor.PrimerApellido + profesor.SegundoApellido + profesor.Cedula).ToLower().Contains(constraint.ToString())));
                }

                returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
                returnObj.Count = results.Count;

                constraint.Dispose();

                return returnObj;
            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                using (var values = results.Values)
                    _adapter._profesores = values.ToArray<Object>()
                        .Select(r => r.ToNetObject<Profesores>()).ToList();
                _adapter.NotifyDataSetChanged();
                constraint.Dispose();
                results.Dispose();
            }
        }
    }
}