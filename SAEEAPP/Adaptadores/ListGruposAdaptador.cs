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
using Xamarin.core.Models;

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
            var item = this[position];
            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.GruposListRow, null);
            }
            convertView.FindViewById<TextView>(Resource.Id.textViewGrupo).Text = item.Grupo;
            return convertView;
        }
    }
}