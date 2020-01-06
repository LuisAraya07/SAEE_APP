using Android.App;
using Android.Views;
using Android.Widget;
using SAEEAPP.Listeners;
using System.Collections.Generic;
using Xamarin.core.Models;

namespace SAEEAPP.Adaptadores
{
    public class CursosGruposAgregarAdapter : BaseAdapter<Grupos>
    {
        private readonly Activity _context;
        private Cursos _curso;
        private List<Grupos> _grupos;
        List<CursosGrupos> _cursosGrupos, agregar, borrar;

        public CursosGruposAgregarAdapter(Activity context, Cursos curso, List<Grupos> grupos,
            List<CursosGrupos> cursosGrupos, List<CursosGrupos> agregar, List<CursosGrupos> borrar)
        {
            this._context = context;
            this._grupos = grupos;
            this._cursosGrupos = cursosGrupos;
            this.agregar = agregar;
            this.borrar = borrar;
            this._curso = curso;
        }

        public override Grupos this[int position] => _grupos[position];

        public override int Count => _grupos.Count;

        public override long GetItemId(int position)
        {
            return this[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Grupos grupo = this[position];

            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.CursosGruposAgregarListRow, null);
            }
            TextView tvNombre = convertView.FindViewById<TextView>(Resource.Id.tvNombre);
            TextView tvAnio = convertView.FindViewById<TextView>(Resource.Id.tvAnio);
            CheckBox cbAgregar = convertView.FindViewById<CheckBox>(Resource.Id.cbAgregar);
            tvNombre.Text = grupo.Grupo;
            tvAnio.Text = grupo.Anio.ToString();
            cbAgregar.SetOnClickListener(
                new AgregarCGListener(_context, _curso, grupo, _cursosGrupos, agregar, borrar, cbAgregar));

            return convertView;
        }
    }
}