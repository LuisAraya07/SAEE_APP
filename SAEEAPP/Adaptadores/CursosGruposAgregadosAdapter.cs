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
    public class CursosGruposAgregadosAdapter : BaseAdapter<CursosGrupos>
    {
        private readonly Activity _context;
        private List<CursosGrupos> _cursosGrupos, agregar, borrar;
        private Cursos _curso;

        public CursosGruposAgregadosAdapter(Activity context, Cursos curso, List<CursosGrupos> cursosGrupos,
            List<CursosGrupos> agregar, List<CursosGrupos> borrar)
        {
            this._context = context;
            this._curso = curso;
            this._cursosGrupos = cursosGrupos;
            this.agregar = agregar;
            this.borrar = borrar;
        }

        public override CursosGrupos this[int position] => _cursosGrupos[position];

        public override int Count => _cursosGrupos.Count;

        public override long GetItemId(int position)
        {
            return this[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            CursosGrupos cursoGrupo = this[position];

            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.CursosGruposAgregadosListRow, null);
            }
            TextView tvNombre = convertView.FindViewById<TextView>(Resource.Id.tvNombre);
            TextView tvAnio = convertView.FindViewById<TextView>(Resource.Id.tvAnio);
            Button btBorrar = convertView.FindViewById<Button>(Resource.Id.btBorrar);

            tvNombre.Text = cursoGrupo.IdGrupoNavigation.Grupo;
            tvAnio.Text = cursoGrupo.IdGrupoNavigation.Anio.ToString();

            btBorrar.SetOnClickListener(
                new BorrarCGListener(_context, cursoGrupo, _cursosGrupos, agregar, borrar, this));

            return convertView;
        }
        public void ActualizarDatos()
        {
            NotifyDataSetChanged();
        }
    }
}