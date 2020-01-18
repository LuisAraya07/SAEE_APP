using Android.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using SAEEAPP.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP.Adaptadores
{
    class ListGruposAdaptador : BaseAdapter<Grupos>
    {

        private readonly Activity _context;
        private readonly List<Grupos> _grupos;

        public ListGruposAdaptador(Activity context, List<Grupos> grupos)
        {
            _context = context;
            _grupos = grupos;

        }


        public override Grupos this[int position] => _grupos[position];

        public override int Count => _grupos.Count;

        public override long GetItemId(int position)
        {
            return this[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            var grupo = this[position];
            if (row == null)
            {
                row = _context.LayoutInflater.Inflate(Resource.Layout.GruposListRow, null);

            }
            DefinirBotones(row,grupo);
            row.FindViewById<TextView>(Resource.Id.textViewGrupo).Text = grupo.Grupo;
            row.FindViewById<TextView>(Resource.Id.textViewAnio).Text = grupo.Anio.ToString();
            return row;
        }
        public void DefinirBotones(View row,Grupos grupo)
        {
            

            Button btnOpciones = row.FindViewById<Button>(Resource.Id.btnOpcionesG);
            btnOpciones.SetTag(Resource.Id.btnOpcionesG, btnOpciones);
            var draw = ContextCompat.GetDrawable(_context, Resource.Drawable.dots_vertical);
            btnOpciones.SetCompoundDrawablesWithIntrinsicBounds(draw, null, null, null);
            btnOpciones.SetOnClickListener(new GruposListener(_context, _grupos, grupo, this, btnOpciones));



        }
        public void ActualizarDatos()
        {
            NotifyDataSetChanged();
        }
        
    }



}

