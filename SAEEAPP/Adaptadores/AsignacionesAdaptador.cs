using Android.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using SAEEAPP.Listeners;
using System.Collections.Generic;
using Xamarin.core.Models;

namespace SAEEAPP.Adaptadores
{
    public class AsignacionesAdaptador : BaseAdapter<Asignaciones>
    {
        private readonly Activity _context;
        private readonly List<Asignaciones> _asignaciones;
        private readonly TextView _tvCargando;
        public AsignacionesAdaptador(Activity context, List<Asignaciones> asignaciones, TextView tvCargando)
        {
            _context = context;
            _asignaciones = asignaciones;
            _tvCargando = tvCargando;
            tvCargando.Text = "No hay datos";//Si no hay datos, se muestra el mensaje, y si se eliminan todos tambien
        }
        public override Asignaciones this[int position] => _asignaciones[position];

        public override int Count => _asignaciones.Count;

        public override long GetItemId(int position)
        {
            return this[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Asignaciones asignacion = this[position];

            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.AsignacionesListRow, null);
            }
            TextView tvNombre = convertView.FindViewById<TextView>(Resource.Id.tvNombre);
            TextView tvDescripcion = convertView.FindViewById<TextView>(Resource.Id.tvDescripcion);
            TextView tvFecha = convertView.FindViewById<TextView>(Resource.Id.tvFecha);
            TextView tvPuntos= convertView.FindViewById<TextView>(Resource.Id.tvPuntos);
            TextView tvPorcentaje = convertView.FindViewById<TextView>(Resource.Id.tvPorcentaje);


            tvNombre.Text = asignacion.Nombre;
            tvDescripcion.Text = asignacion.Descripcion;
            tvFecha.Text = asignacion.Fecha.ToShortDateString();
            tvPuntos.Text = asignacion.Puntos.ToString();
            tvPorcentaje.Text = asignacion.Porcentaje.ToString();
            DefinirBotones(convertView,asignacion);
            return convertView;
        }
        private void DefinirBotones(View row, Asignaciones asignacion)
        {
            Button btnOpciones = row.FindViewById<Button>(Resource.Id.btnOpcionesC);
            btnOpciones.SetTag(Resource.Id.btnOpcionesC, btnOpciones);
            var draw = ContextCompat.GetDrawable(_context, Resource.Drawable.dots_vertical);
            btnOpciones.SetCompoundDrawablesWithIntrinsicBounds(draw, null, null, null);
            btnOpciones.SetOnClickListener(new AsignacionesListener(_context, _asignaciones, asignacion, this, btnOpciones));
        }
        public void ActualizarDatos()
        {
            _tvCargando.Visibility = (_asignaciones.Count > 0) ? ViewStates.Invisible : ViewStates.Visible;
            NotifyDataSetChanged();
        }
    }
}