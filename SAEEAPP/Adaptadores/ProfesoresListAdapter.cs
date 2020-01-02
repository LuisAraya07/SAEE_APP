using Android.App;
using Android.Views;
using Android.Widget;
using SAEEAPP.Listeners;
using System.Collections.Generic;
using Xamarin.core.Models;

namespace SAEEAPP.Adaptadores
{
    public class ProfesoresListAdapter : BaseAdapter<Profesores>
    {
        private readonly Activity _context;
        private readonly List<Profesores> _profesores;

        public ProfesoresListAdapter(Activity context, List<Profesores> profesores)
        {
            _context = context;
            _profesores = profesores;
        }

        public override Profesores this[int position] => _profesores[position];

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
            NotifyDataSetChanged();
        }
    }
}