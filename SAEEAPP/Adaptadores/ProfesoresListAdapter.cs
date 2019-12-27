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
    class ProfesoresListAdapter : BaseAdapter<Profesores>
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
            convertView.
                FindViewById<TextView>(Resource.Id.tvNombre).
                Text = $"{profesor.Nombre} {profesor.PrimerApellido} {profesor.SegundoApellido}";
            convertView.FindViewById<TextView>(Resource.Id.tvCedula).Text = profesor.Cedula;
            convertView.FindViewById<TextView>(Resource.Id.tvCorreo).Text = profesor.Correo;
            return convertView;
        }
    }
}