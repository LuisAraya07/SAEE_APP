using Android.App;
using Android.Views;
using Android.Widget;
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
            DefinirBotones(row, position);
            row.FindViewById<TextView>(Resource.Id.textViewGrupo).Text = grupo.Grupo;
            row.FindViewById<TextView>(Resource.Id.textViewAnio).Text = grupo.Anio.ToString();
            return row;
        }
        public void DefinirBotones(View row, int position)
        {
            Button btBorrar = row.FindViewById<Button>(Resource.Id.btBorrar);
            btBorrar.SetTag(Resource.Id.btBorrar, position);
            btBorrar.Click -= OnClick_Borrar;
            btBorrar.Click += OnClick_Borrar;

            Button btEditar = row.FindViewById<Button>(Resource.Id.btEditar);
            btEditar.SetTag(Resource.Id.btEditar, position);
            btEditar.Click -= OnClick_Editar;
            btEditar.Click += OnClick_Editar;

            Button btEstudiantes = row.FindViewById<Button>(Resource.Id.btEstudiantes);
            btEstudiantes.SetTag(Resource.Id.btEstudiantes, position);
            btEstudiantes.Click -= OnClick_Estudiantes;
            btEstudiantes.Click += OnClick_Estudiantes;


        }
        public void OnClick_Borrar(object sender, EventArgs e)
        {
            int i = (int)((Button)sender).GetTag(Resource.Id.btBorrar);
            var grupo = _grupos.ElementAt(i);
            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(_context, Resource.Style.AlertDialogStyle);
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
            //Toast.MakeText(this,"Dialogo agregar",ToastLength.Long).Show();
            LayoutInflater layoutInflater = LayoutInflater.From(_context);
            View mView = layoutInflater.Inflate(Resource.Layout.Dialogo_Agregar_Grupos, null);
            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(_context,Resource.Style.AlertDialogStyle);
            alertDialogBuilder.SetView(mView);
            mView.FindViewById<EditText>(Resource.Id.etGrupo).Text = grupo.Grupo;
            alertDialogBuilder.SetTitle("Editando Grupo");
            alertDialogBuilder.SetCancelable(false)
            .SetPositiveButton("Guardar", delegate
            {
                // Toast.MakeText(this, "Grupo: "+txtGrupo.Text, ToastLength.Long).Show();
                EditarGrupoAsync(alertDialogBuilder, mView, grupo);
            })
            .SetNegativeButton("Cancelar", delegate
            {
                alertDialogBuilder.Dispose();

            });
            Android.Support.V7.App.AlertDialog alertDialogAndroid = alertDialogBuilder.Create();
            alertDialogAndroid.Show();


        }
        private async void EditarGrupoAsync(Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder, View view, Grupos grupo)
        {
            DateTime fechaActual = DateTime.Today;
            GruposServices gruposServices = new GruposServices();
            var etGrupo = view.FindViewById<EditText>(Resource.Id.etGrupo).Text;
            if (!etGrupo.Equals("") && !etGrupo.StartsWith(" "))
            {
                grupo.Grupo = etGrupo;
                grupo.Anio = fechaActual.Year;
                var actualizado = await gruposServices.PutAsync(grupo);
                if (actualizado)
                {
                    Toast.MakeText(_context, "Se ha editado con éxito.", ToastLength.Long).Show();
                    _grupos.Where(x => x.Id == grupo.Id).FirstOrDefault().Grupo = etGrupo;
                    NotifyDataSetChanged();
                    alertDialogBuilder.Dispose();
                }
                else
                {
                    Toast.MakeText(_context, "Error al editar grupo.", ToastLength.Long).Show();
                    alertDialogBuilder.Dispose();
                }

            }
            else
            {
                Toast.MakeText(_context, "Debe ingresar un grupo.", ToastLength.Long).Show();
            }

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
            listaEstudiantes = listaEG.Select(x => x.IdEstudianteNavigation).ToList();
            var estudiantesListView = _context.FindViewById<ListView>(Resource.Id.listViewEG);
            ListEGAdaptador adaptadorEG = new ListEGAdaptador(_context, listaEstudiantes, listaEG);
            //estudiantesListView.Adapter = adaptadorEG;
            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(_context, Resource.Style.AlertDialogStyle);
            alertDialogBuilder.SetCancelable(true)
            .SetTitle("Estudiantes del Grupo")
            .SetView(estudiantesListView)
            .SetAdapter(adaptadorEG, (s, e) =>
            {
                var index = e.Which;
            })
            .SetNegativeButton("Cerrar", delegate
            {
                alertDialogBuilder.Dispose();

            })
            .SetPositiveButton("Añadir", delegate
            {
                //Toast.MakeText(_context,"Aqui agregamos",ToastLength.Short).Show();
                MostrarLVAgregarAsync(grupo, listaEstudiantes, listaEG);


            });

            Android.Support.V7.App.AlertDialog alertDialogAndroid = alertDialogBuilder.Create();
            alertDialogAndroid.Show();

        }


        public async void MostrarLVAgregarAsync(Grupos grupo, List<Estudiantes> listaAgregados, List<EstudiantesXgrupos> listaEG)
        {
            EstudiantesServices estudiantesServicios = new EstudiantesServices();
            List<Estudiantes> listaTemporal = new List<Estudiantes>();
            //Se le envia el id del profesor para obtener todos los estudiantes
            listaTemporal = await estudiantesServicios.GetAsync(grupo.IdProfesor);
            var listaAgregar = listaTemporal.Where(st => !(listaAgregados.Select(x => x.Id).Contains(st.Id))).ToList();
            var estudiantesListView = _context.FindViewById<ListView>(Resource.Id.listViewEGA);
            ListAgregarEGAdaptador adaptadorEG = new ListAgregarEGAdaptador(_context, listaAgregar, listaEG, grupo.Id);
            //estudiantesListView.Adapter = adaptadorEG;
            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilderAgregar = new Android.Support.V7.App.AlertDialog.Builder(_context, Resource.Style.AlertDialogStyle);
            alertDialogBuilderAgregar.SetCancelable(true)
            .SetTitle("Estudiantes Para Agregar")
            //.SetView(estudiantesListView)
            .SetAdapter(adaptadorEG, (s, e) =>
            //.SetMultiChoiceItems(colors, checkedColors, (s, e) => 
            {
                var index = e.Which;
            })
            .SetNegativeButton("Cerrar", delegate
            {
                alertDialogBuilderAgregar.Dispose();
            });
            //.SetPositiveButton("Añadir", async delegate {
            //    Toast.MakeText(_context,"Aqui agregamos",ToastLength.Short).Show();
            //    await MostrarLVAgregarAsync(grupo, listaEstudiantes, listaEG);

            //});
            Android.Support.V7.App.AlertDialog alertDialogAndroidAgregar = alertDialogBuilderAgregar.Create();
            alertDialogAndroidAgregar.Show();


        }
    }



}

