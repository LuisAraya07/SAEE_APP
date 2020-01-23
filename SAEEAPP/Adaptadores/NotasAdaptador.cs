using Android.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using SAEEAPP.Listeners;
using System.Collections.Generic;
using Xamarin.core.Models;

namespace SAEEAPP.Adaptadores
{
    public class NotasAdaptador : BaseAdapter<EstudianteEvaluacion>
    {
        private readonly Activity _context;
        private readonly List<EstudianteEvaluacion> _estudiantes;
        private readonly Asignaciones _asignacion;

        public NotasAdaptador(Activity context, List<EstudianteEvaluacion> cursos,Asignaciones asignacion)
        {
            _context = context;
            _estudiantes = cursos;
            _asignacion = asignacion;
        }

        public override EstudianteEvaluacion this[int position] => _estudiantes[position];

        public override int Count => _estudiantes.Count;

        public override long GetItemId(int position)
        {
            return this[position].evaluacion.Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            EstudianteEvaluacion estudiante = this[position];

            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.NotasListRow, null);
            }
            TextView tvNombre = convertView.FindViewById<TextView>(Resource.Id.tvNombre);
            TextView tvPuntos = convertView.FindViewById<TextView>(Resource.Id.tvPuntos);
            TextView tvPorcentaje = convertView.FindViewById<TextView>(Resource.Id.tvPorcentaje1);
            TextView tvNota = convertView.FindViewById<TextView>(Resource.Id.tvNota);

            // TextView tvCantidadPeriodos = convertView.FindViewById<TextView>(Resource.Id.tvCantidadPeriodos);
            tvNombre.Text = estudiante.Nombre;
            tvPuntos.Text = estudiante.Puntos.ToString();
            tvPorcentaje.Text = estudiante.Porcentaje.ToString();
            tvNota.Text = estudiante.Nota.ToString();
           // tvCantidadPeriodos.Text = curso.CantidadPeriodos.ToString();
            DefinirBotones( convertView, estudiante);


            return convertView;
        }
        private void DefinirBotones(View row, EstudianteEvaluacion estudiante)
        {
          //  Button btNota = row.FindViewById<Button>(Resource.Id.btNotaEvaluar);
           // btNota.SetOnClickListener(new CursosListener(_context, _estudiantes, estudiante, this, btnOpciones));
        }
    }
}