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
using SAEEAPP.Listeners;
using Xamarin.core.Models;

namespace SAEEAPP.Adaptadores
{
    public class CursosListAdapter : BaseAdapter<Cursos>
    {
        private readonly Activity _context;
        private readonly List<Cursos> _cursos;

        public CursosListAdapter(Activity context, List<Cursos> cursos)
        {
            _context = context;
            _cursos = cursos;
        }

        public override Cursos this[int position] => _cursos[position];

        public override int Count => _cursos.Count;

        public override long GetItemId(int position)
        {
            return this[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Cursos curso = this[position];

            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.CursosListRow, null);
            }
            TextView tvNombre = convertView.FindViewById<TextView>(Resource.Id.tvNombre);
            TextView tvCantidadPeriodos = convertView.FindViewById<TextView>(Resource.Id.tvCantidadPeriodos);
            Button btEditar = convertView.FindViewById<Button>(Resource.Id.btEditar);
            Button btGrupos = convertView.FindViewById<Button>(Resource.Id.btGrupos);
            Button btBorrar = convertView.FindViewById<Button>(Resource.Id.btBorrar);

            tvNombre.Text = curso.Nombre;
            tvCantidadPeriodos.Text = curso.CantidadPeriodos.ToString();

            btEditar.SetOnClickListener(new EditarCListener(_context, _cursos, curso, this));
            btGrupos.SetOnClickListener(new GruposCListener(_context, _cursos, curso, this));
            btBorrar.SetOnClickListener(new BorrarCListener(_context, _cursos, curso, this));

            return convertView;
        }
        public void ActualizarDatos()
        {
            NotifyDataSetChanged();
        }
    }
}