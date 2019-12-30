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
            var item = this[position];
            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.GruposListRow, null);
            }
            //item.EstudiantesXgrupos.ElementAt(0).IdEstudianteNavigation.Nombre
            convertView.FindViewById<TextView>(Resource.Id.textViewGrupo).Text = item.Grupo;
            convertView.FindViewById<TextView>(Resource.Id.textViewAnio).Text = item.Anio.ToString();
            Button btBorrar = convertView.FindViewById<Button>(Resource.Id.btBorrar);
            btBorrar.Click += delegate
            {
                OnClick_Borrar(item);

            };
            Button btEditar = convertView.FindViewById<Button>(Resource.Id.btEditar);
            btEditar.Click += delegate
            {
                OnClick_Editar(item);

            };
            Button btEstudiantes = convertView.FindViewById<Button>(Resource.Id.btEstudiantes);
            btEstudiantes.Click += delegate
            {
                OnClick_Estudiantes(item);

            };
            return convertView;
        }

        public void OnClick_Borrar(Grupos grupo)
        {
            new AlertDialog.Builder(_context)
              .SetIcon(Resource.Drawable.trash_can_outline)
              .SetTitle("¿Está seguro?")
              .SetMessage("Quiere eliminar el grupo: " + grupo.Grupo)
              .SetPositiveButton("Sí", delegate
              {
                  GruposServices gruposServices = new GruposServices();
                  gruposServices.DeleteGruposAsync(grupo);
                  Toast.MakeText(_context, "Se ha eliminado con éxito.", ToastLength.Long).Show();
                  _grupos.Remove(grupo);
                  this.NotifyDataSetChanged();
              })
              .SetNegativeButton("No", delegate
              {
                  this.Dispose();

              })
              .Show();
        }
        public void OnClick_Editar(Grupos grupo)
        {


        }
        public void OnClick_Estudiantes(Grupos grupo)
        {


        }
    }

        
}