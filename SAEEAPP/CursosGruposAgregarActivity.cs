using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using SAEEAPP.Adaptadores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{
    public class CursosGruposAgregarActivity
    {
        AlertDialog.Builder alertDialogBuilder;
        AlertDialog alertDialogAndroid;
        Activity context;
        CursosGruposAgregadosAdapter cursosGruposAgregadosAdapter;
        CursosGruposAgregarAdapter cursosGruposAgregarAdapter;
        readonly Cursos curso;
        ListView lvCursosGruposAgregar;

        public CursosGruposAgregarActivity(Activity context, CursosGruposAgregadosAdapter cursosGruposAgregadosAdapter,
            Cursos curso, List<CursosGrupos> cursosGrupos, List<CursosGrupos> agregar, List<CursosGrupos> borrar, List<Grupos> grupos)
        {
            // Se inicializan los valores
            this.context = context;
            this.cursosGruposAgregadosAdapter = cursosGruposAgregadosAdapter;
            this.curso = curso;

            // Se obtiene la vista
            LayoutInflater layoutInflater = LayoutInflater.From(context);
            View VistaAgregar = layoutInflater.Inflate(Resource.Layout.Dialogo_Cursos_Grupos_Agregar, null);

            // Se configura el listview
            cursosGruposAgregarAdapter = new CursosGruposAgregarAdapter(context, curso, grupos, cursosGrupos, agregar, borrar);
            lvCursosGruposAgregar = VistaAgregar.FindViewById<ListView>(Resource.Id.lvCursosGruposAgregar);
            lvCursosGruposAgregar.Adapter = cursosGruposAgregarAdapter;

            // Se contruye el diálogo
            alertDialogBuilder = new AlertDialog.Builder(context, Resource.Style.AlertDialogStyle)
            .SetView(VistaAgregar)
            .SetPositiveButton("Cerrar", (EventHandler<DialogClickEventArgs>)null)
            //.SetNegativeButton("Cancelar", (EventHandler<DialogClickEventArgs>)null)
            //.SetNeutralButton("Agregar", (EventHandler<DialogClickEventArgs>)null)
            .SetTitle("Grupos disponibles para agregar");
            alertDialogAndroid = alertDialogBuilder.Create();
        }

        private void Cerrar(object sender, EventArgs e)
        {
            cursosGruposAgregadosAdapter.ActualizarDatos();
            alertDialogAndroid.Dismiss();
        }

        private async Task GuardarAsync()
        {
            CursosServices servicioCursos = new CursosServices();
            bool resultado = await servicioCursos.UpdateCursoAsync(curso);

            if (resultado)
            {
                // Se actualiza la lista de cursos
                cursosGruposAgregadosAdapter.ActualizarDatos();

                Toast.MakeText(context, "Guardado correctamente", ToastLength.Long).Show();
                alertDialogAndroid.Dismiss();
            }
            else
            {
                Toast.MakeText(context, "Error al guardar, intente nuevamente", ToastLength.Long).Show();
            }
        }

        public void Show()
        {
            alertDialogAndroid.Show();
            // Se obtienen los botones para asignarles los métodos nuevos (no cierran el diálogo).
            Button btGuardar = alertDialogAndroid.GetButton((int)DialogButtonType.Positive);

            // Se asignan las funciones
            btGuardar.Click += Cerrar;
        }
    }
}