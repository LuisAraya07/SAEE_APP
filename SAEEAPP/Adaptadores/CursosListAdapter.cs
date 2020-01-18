using Android.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using SAEEAPP.Listeners;
using System.Collections.Generic;
using Xamarin.core.Models;

namespace SAEEAPP.Adaptadores
{
    public class CursosListAdapter : BaseAdapter<Cursos>
    {
        private readonly Activity _context;
        private readonly List<Cursos> _cursos;
        private readonly TextView _tvCargando;

        public CursosListAdapter(Activity context, List<Cursos> cursos, TextView tvCargando)
        {
            _context = context;
            _cursos = cursos;
            _tvCargando = tvCargando;
            tvCargando.Text = "No hay datos";//Si no hay datos, se muestra el mensaje, y si se eliminan todos tambien
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
            //Button btEditar = convertView.FindViewById<Button>(Resource.Id.btEditar);
            //Button btGrupos = convertView.FindViewById<Button>(Resource.Id.btGrupos);
            //Button btBorrar = convertView.FindViewById<Button>(Resource.Id.btBorrar);

            tvNombre.Text = curso.Nombre;
            tvCantidadPeriodos.Text = curso.CantidadPeriodos.ToString();
            DefinirBotones( convertView, curso);
            //btEditar.SetOnClickListener(new EditarCListener(_context, _cursos, curso, this));
            //btGrupos.SetOnClickListener(new GruposCListener(_context, curso, this));
            //btBorrar.SetOnClickListener(new BorrarCListener(_context, _cursos, curso, this));

            return convertView;
        }
        private void DefinirBotones(View row, Cursos curso)
        {
            Button btnOpciones = row.FindViewById<Button>(Resource.Id.btnOpcionesC);
            btnOpciones.SetTag(Resource.Id.btnOpcionesC, btnOpciones);
            var draw = ContextCompat.GetDrawable(_context, Resource.Drawable.dots_vertical);
            btnOpciones.SetCompoundDrawablesWithIntrinsicBounds(draw, null, null, null);
            btnOpciones.SetOnClickListener(new CursosListener(_context, _cursos, curso, this, btnOpciones));


        }
        public void ActualizarDatos()
        {
            _tvCargando.Visibility = (_cursos.Count > 0)? ViewStates.Invisible : ViewStates.Visible;
            NotifyDataSetChanged();
        }
    }
}