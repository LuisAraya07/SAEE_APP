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
    class ListAgregarEGAdaptador : BaseAdapter<Estudiantes>
    {

        private readonly Activity _context;
        private readonly List<Estudiantes> _listaAgregar;
        public List<EstudiantesXgrupos> _EG;
        private readonly int _idGrupo;
        public ListAgregarEGAdaptador(Activity context, List<Estudiantes> listaAgregar, List<EstudiantesXgrupos> EG, int idGrupo)
        {
            _context = context;
            _listaAgregar = listaAgregar;
            _EG = EG;
            _idGrupo = idGrupo;

        }


        public override Estudiantes this[int position] => _listaAgregar[position];

        public override int Count => _listaAgregar.Count;

        public override long GetItemId(int position)
        {
            return this[position].Id;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Estudiantes estudiante = this[position];

            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.AgregarEGListRow, null);

            }
            CheckBox CBAgregar = convertView.FindViewById<CheckBox>(Resource.Id.checkBoxEGA);
            CBAgregar.SetTag(Resource.Id.checkBoxEGA, position);
            CBAgregar.Click -= OnClick_AgregarAsync;
            CBAgregar.Click += OnClick_AgregarAsync;
            convertView.
                FindViewById<TextView>(Resource.Id.textViewNombreEGA).
                Text = $"{estudiante.Nombre} {estudiante.PrimerApellido} {estudiante.SegundoApellido}";
            convertView.FindViewById<TextView>(Resource.Id.textViewCedulaEGA).Text = estudiante.Cedula;
            return convertView;
        }

        public void OnClick_AgregarAsync(object sender, EventArgs e)
        {
            int i = (int)((Button)sender).GetTag(Resource.Id.checkBoxEGA);
            var estudiante = _listaAgregar.ElementAt(i);
            var isSelect = _EG.Where(x => x.IdEstudiante == estudiante.Id).FirstOrDefault();
            if (isSelect == null)
            {
                AgregarEstudiante(estudiante);
            }
            else
            {
                EliminarEstudiantes(isSelect);

            }
            //Toast.MakeText(_context,estudiante.Nombre,ToastLength.Long).Show();
        }
        public async void EliminarEstudiantes(EstudiantesXgrupos eg)
        {
            VerificarConexion vc = new VerificarConexion(_context);
            var conectado = vc.IsOnline();
            if (conectado) { 
                GruposServices gruposServices = new GruposServices();
                var EGBorrado = await gruposServices.DeleteEGAsync(eg);
                if (EGBorrado)
                {
                    Toast.MakeText(_context, "Eliminado.", ToastLength.Short).Show();
                    _EG.Remove(eg);

                }
                else
                {
                    Toast.MakeText(_context, "Error.", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(_context, "Necesita conexión a internet.", ToastLength.Long).Show();
            }
        }
        public async void AgregarEstudiante(Estudiantes estudiante)
        {
            EstudiantesXgrupos EGAgregar = new EstudiantesXgrupos()
            {
                IdEstudiante = estudiante.Id,
                IdGrupo = _idGrupo
                //El id del profesor es irrelevante
                //IdProfesor = estudiante.IdProfesor
            };
            VerificarConexion vc = new VerificarConexion(_context);
            var conectado = vc.IsOnline();
            if (conectado)
            {
                GruposServices gruposServices = new GruposServices();
                var EGNuevo = await gruposServices.PostEGAsync(EGAgregar);
                if (EGNuevo == null)
                {
                    Toast.MakeText(_context, "Error.", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(_context, "Agregado.", ToastLength.Short).Show();
                    _EG.Add(EGNuevo);
                }
            }
            else
            {
                Toast.MakeText(_context, "Necesita conexión a internet.", ToastLength.Short).Show();

            }

        }


    }


}