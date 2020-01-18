using Android.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;
using SAEEAPP.Listeners;
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
        private List<Estudiantes> datosOriginales;

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
            DefinirBotones(view, estudiante);

            view.
                FindViewById<TextView>(Resource.Id.textViewNombreE).
                Text = $"{estudiante.Nombre} {estudiante.PrimerApellido} {estudiante.SegundoApellido}";
            view.FindViewById<TextView>(Resource.Id.textViewCedE).Text = estudiante.Cedula;
            return view;
        }

        private void DefinirBotones(View view, Estudiantes estudiante)
        {
            Button btnOpciones = view.FindViewById<Button>(Resource.Id.btnOpcionesE);
            btnOpciones.SetTag(Resource.Id.btnOpcionesE, btnOpciones);
            var draw = ContextCompat.GetDrawable(_context, Resource.Drawable.dots_vertical);
            btnOpciones.SetCompoundDrawablesWithIntrinsicBounds(draw, null, null, null);
            btnOpciones.SetOnClickListener(new EstudiantesListener(_context, _estudiantes, estudiante, this, btnOpciones));
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

                returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
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