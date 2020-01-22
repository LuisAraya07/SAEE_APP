using Android.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.core;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP.Adaptadores
{
    class ListEGAdaptador : BaseAdapter<Estudiantes>
    {
        private readonly Activity _context;
        private readonly List<Estudiantes> _estudiantes;
        public List<EstudiantesXgrupos> _EG;
        public ListEGAdaptador(Activity context, List<Estudiantes> estudiantes, List<EstudiantesXgrupos> EG)
        {
            _context = context;
            _estudiantes = estudiantes;
            _EG = EG;

        }

        public override Estudiantes this[int position] => _estudiantes[position];

        public override int Count => _estudiantes.Count;

        public override long GetItemId(int position)
        {
            return this[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Estudiantes estudiante = this[position];

            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.EGListRow, null);

            }

            Button btBorrar = convertView.FindViewById<Button>(Resource.Id.btBorrarEG);
            btBorrar.SetTag(Resource.Id.btBorrarEG, position);
            btBorrar.Click -= OnClick_Borrar;
            btBorrar.Click += OnClick_Borrar;
            convertView.
                FindViewById<TextView>(Resource.Id.textViewNombreEG).
                Text = $"{estudiante.Nombre} {estudiante.PrimerApellido} {estudiante.SegundoApellido}";
            convertView.FindViewById<TextView>(Resource.Id.textViewCedEG).Text = estudiante.Cedula;
            return convertView;
        }

        public void OnClick_Borrar(object sender, EventArgs e)
        {
            int i = (int)((Button)sender).GetTag(Resource.Id.btBorrarEG);
            var estudiante = _estudiantes.ElementAt(i);
            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(_context, Resource.Style.AlertDialogStyle);
            alertDialogBuilder.SetIcon(Resource.Drawable.trash_can_outline)
              .SetCancelable(false)
              .SetTitle("¿Está seguro?")
              .SetMessage("Quiere eliminar al estudiante: " + $"{estudiante.Nombre} {estudiante.PrimerApellido} {estudiante.SegundoApellido}" + " de este grupo")
              .SetPositiveButton("Sí", delegate
              {
                      //ELIMINAR AQUI 
                      EliminarEG(estudiante,i);

              })
              .SetNegativeButton("No", delegate
              {
                  alertDialogBuilder.Dispose();

              })
              .Show();

        }

        public async void EliminarEG(Estudiantes estudiante,int i)
        {
            VerificarConexion vc = new VerificarConexion(_context);
            var conectado = vc.IsOnline();
            var _EGEliminar = _EG.Where(x => x.IdEstudiante == estudiante.Id).FirstOrDefault();
            if (conectado)
            {
                GruposServices gruposServices = new GruposServices();
                var eliminado = await gruposServices.DeleteEGAsync(_EGEliminar);
                if (eliminado)
                {
                    Toast.MakeText(_context, "Se ha eliminado con éxito.", ToastLength.Long).Show();
                    _estudiantes.RemoveAt(i);
                    _EG.Remove(_EGEliminar);
                    NotifyDataSetChanged();
                }
                else
                {
                    Toast.MakeText(_context, "No se ha podido eliminar.", ToastLength.Long).Show();
                }
            }
            else
            {
                //Toast.MakeText(_context, "Necesita conexión a internet.", ToastLength.Long).Show();
                ProfesoresServices ns = new ProfesoresServices(1);
                Profesores profesor = await ns.GetProfesorConectado();
                GruposServices gruposServicios = new GruposServices(profesor.Id);
                var eliminado = await gruposServicios.DeleteEGOffline(_EGEliminar);
                Toast.MakeText(_context, "Se ha eliminado con éxito.", ToastLength.Long).Show();
                _estudiantes.RemoveAt(i);
                _EG.Remove(_EGEliminar);
                NotifyDataSetChanged();
            }
        }


    }
}