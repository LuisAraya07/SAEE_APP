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
            View row = convertView;
            var grupo = this[position];
            if (row == null)
            {
                row = _context.LayoutInflater.Inflate(Resource.Layout.GruposListRow, null);

                Button btBorrar = row.FindViewById<Button>(Resource.Id.btBorrar);
                btBorrar.SetTag(Resource.Id.btBorrar, position);
                //btBorrar.Click -= OnClick_Borrar;
                btBorrar.Click += OnClick_Borrar;

                Button btEditar = row.FindViewById<Button>(Resource.Id.btEditar);
                btEditar.SetTag(Resource.Id.btEditar, position);
                btEditar.Click += OnClick_Editar;

                Button btEstudiantes = row.FindViewById<Button>(Resource.Id.btEstudiantes);
                btEstudiantes.SetTag(Resource.Id.btEstudiantes, position);
                btEstudiantes.Click += OnClick_Estudiantes;
            }
            //item.EstudiantesXgrupos.ElementAt(0).IdEstudianteNavigation.Nombre
            row.FindViewById<TextView>(Resource.Id.textViewGrupo).Text = grupo.Grupo;
            row.FindViewById<TextView>(Resource.Id.textViewAnio).Text = grupo.Anio.ToString();
            return row;
        }
        
        public void OnClick_Borrar(object sender, EventArgs e)
        {
            int i = (int)((Button)sender).GetTag(Resource.Id.btBorrar);
            var grupo = _grupos.ElementAt(i);
            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(_context);
            alertDialogBuilder.SetCancelable(false)
            .SetIcon(Resource.Drawable.trash_can_outline)
               .SetTitle("¿Está seguro?")
               .SetMessage("Quiere eliminar el grupo: " + grupo.Grupo)
               .SetPositiveButton("Sí", async delegate
               {
                   GruposServices gruposServices = new GruposServices();
                   await gruposServices.DeleteGruposAsync(grupo);
                   Toast.MakeText(_context, "Se ha eliminado con éxito.", ToastLength.Long).Show();
                   _grupos.RemoveAt(i);
                   NotifyDataSetChanged();
               })
               .SetNegativeButton("No", delegate
               {
                   alertDialogBuilder.Dispose();
               })
               .Show();
        }
        public void OnClick_Editar(object sender, EventArgs e)
        {
            int i = (int)((Button)sender).GetTag(Resource.Id.btEditar);
            var grupo = _grupos.ElementAt(i);
            

        }


        public async void OnClick_Estudiantes(object sender, EventArgs e)
        {
            int i = (int)((Button)sender).GetTag(Resource.Id.btEstudiantes);
            var grupo = _grupos.ElementAt(i);
            List<Estudiantes> listaEstudiantes = new List<Estudiantes>();
            List<EstudiantesXgrupos> listaEG = new List<EstudiantesXgrupos>();
            GruposServices gruposServicios = new GruposServices();
            //Cada vez vamos a obtener los estudiantes de ese grupo
            listaEG = await gruposServicios.GetEGAsync(grupo.Id);//grupo.EstudiantesXgrupos.Select(x => x.IdEstudianteNavigation).ToList();
            listaEstudiantes = listaEG.Select(x=>x.IdEstudianteNavigation).ToList();
            var estudiantesListView = _context.FindViewById<ListView>(Resource.Id.listViewEG);
            ListEGAdaptador adaptadorEG = new ListEGAdaptador(_context, listaEstudiantes,listaEG);
            //estudiantesListView.Adapter = adaptadorEG;
            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(_context);
            alertDialogBuilder.SetCancelable(true)
            .SetTitle("Estudiantes del Grupo")
            .SetView(estudiantesListView)
            .SetAdapter(adaptadorEG,(s,e)=>{
                var index = e.Which;
                
            })
            .SetNegativeButton("Cerrar", delegate {
                 alertDialogBuilder.Dispose();

             })
            .SetPositiveButton("Añadir", delegate {
                Toast.MakeText(_context,"Aqui agregamos",ToastLength.Short).Show();

             });
            Android.Support.V7.App.AlertDialog alertDialogAndroid = alertDialogBuilder.Create();
            alertDialogAndroid.Show();
            
        }
    }
    


}

