using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using SAEEAPP.Adaptadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.core;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{
    public class CursosGruposAgregadosActivity
    {
        AlertDialog.Builder alertDialogBuilder;
        AlertDialog alertDialogAndroid;
        Activity context;
        CursosListAdapter cursosAdapter;
        CursosGruposAgregadosAdapter cursosGruposAgregadosAdapter;
        List<CursosGrupos> cursosGrupos;
        List<CursosGrupos> agregar;
        List<CursosGrupos> borrar;
        Cursos curso;
        Button btGuardar, btCancelar, btAgregar;
        ListView lvCursosGruposAgregados;

        public CursosGruposAgregadosActivity(Activity context, CursosListAdapter cursosAdapter,
            Cursos curso)
        {
            // Se inicializan los valores
            this.context = context;
            this.cursosAdapter = cursosAdapter;
            this.cursosGrupos = curso.CursosGrupos.ToList();// Para restaurar si presiona cancelar
            this.curso = curso;
            agregar = new List<CursosGrupos>();
            borrar = new List<CursosGrupos>();

            // Se obtiene la vista
            LayoutInflater layoutInflater = LayoutInflater.From(context);
            View VistaAgregar = layoutInflater.Inflate(Resource.Layout.Dialogo_Cursos_Grupos_Agregados, null);

            // Se configura el listview
            cursosGruposAgregadosAdapter = new CursosGruposAgregadosAdapter(context, cursosGrupos, agregar, borrar);
            lvCursosGruposAgregados = VistaAgregar.FindViewById<ListView>(Resource.Id.lvCursosGruposAgregados);
            lvCursosGruposAgregados.Adapter = cursosGruposAgregadosAdapter;

            // Se contruye el diálogo
            alertDialogBuilder = new AlertDialog.Builder(context, Resource.Style.AlertDialogStyle)
            .SetView(VistaAgregar)
            .SetPositiveButton("Guardar", (EventHandler<DialogClickEventArgs>)null)
            .SetNegativeButton("Cancelar", (EventHandler<DialogClickEventArgs>)null)
            .SetNeutralButton("Agregar", (EventHandler<DialogClickEventArgs>)null)
            .SetTitle("Grupos agregados");
            alertDialogAndroid = alertDialogBuilder.Create();
        }

        private async void Guardar(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(context);
            // Se desactivan los botones
            ActivarDesactivarBotones(false);
            Toast.MakeText(context, "Guardando, espere", ToastLength.Short).Show();

            CursosServices servicioCursos = new CursosServices();
            List<CursosGrupos> agregarPreparados = new List<CursosGrupos>();
            // Se llena la lista con los cursosGrupos preparados para ser insertados
            foreach (CursosGrupos cursosGruposTemp in agregar)
            {
                agregarPreparados.Add(new CursosGrupos()
                {
                    IdCurso = cursosGruposTemp.IdCurso,
                    IdGrupo = cursosGruposTemp.IdGrupo
                });// Importante que solo tengan esto dos atributos para evitar errores
            }
            bool agregados = true, borrados = true, cambio = false;

            if (agregar.Count > 0)
            {
                
                var conectado = vc.IsOnline();
                if (conectado)
                {
                    agregados = await servicioCursos.AgregarCursosGruposAsync(agregarPreparados);
                }
                else
                {
                    //AQUI OFFLINE
                    ProfesoresServices ns = new ProfesoresServices(1);
                    Profesores profesor = await ns.GetProfesorConectado();
                    CursosServices servicioOffline = new CursosServices(profesor.Id);
                    agregados = await servicioOffline.AgregarCursosGruposOffline(agregarPreparados);

                }
                cambio = true;
            }
            if (borrar.Count > 0)
            {
                var conectado = vc.IsOnline();
                if (conectado)
                {
                    borrados = await servicioCursos.BorrarCursosGruposAsync(borrar);
                }
                else
                {
                    //AQUI OFFLINE
                    ProfesoresServices ns = new ProfesoresServices(1);
                    Profesores profesor = await ns.GetProfesorConectado();
                    CursosServices servicioOffline = new CursosServices(profesor.Id);
                    borrados = await servicioOffline.BorrarCursosGruposOffline(borrar);

                }
                cambio = true;
            }

            if (agregados && borrados)
            {
                // Se actualiza la lista de cursos
                if (cambio)
                {
                    var conectado = vc.IsOnline();
                    if (conectado)
                    {
                        cursosGrupos = await servicioCursos.GetCursosGruposAsync(curso.Id);
                    }
                    else
                    {
                        //AQUI OFFLINE
                        ProfesoresServices ns = new ProfesoresServices(1);
                        Profesores profesor = await ns.GetProfesorConectado();
                        CursosServices servicioOffline = new CursosServices(profesor.Id);
                        cursosGrupos = await servicioOffline.GetCursosGruposOffline(curso.Id);
                    }

                }
                curso.CursosGrupos = cursosGrupos;
                cursosAdapter.ActualizarDatos();

                Toast.MakeText(context, "Guardado correctamente", ToastLength.Long).Show();
                alertDialogAndroid.Dismiss();
            }
            else
            {
                // Se restablecen los botones
                ActivarDesactivarBotones(true);

                Toast.MakeText(context, "Error al guardar, intente nuevamente", ToastLength.Short).Show();
            }
        }

        private void Cancelar(object sender, EventArgs e)
        {
            alertDialogAndroid.Dismiss();
        }

        private async void Agregar(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(context);
            // Se desactivan los botones
            ActivarDesactivarBotones(false);
            Toast.MakeText(context, "Cargando, espere", ToastLength.Short).Show();
            List<Grupos> listaGrupos = new List<Grupos>();
            GruposServices servicioGrupos;
            var conectado = vc.IsOnline();
            if (conectado)
            {
                servicioGrupos = new GruposServices();
                listaGrupos = await servicioGrupos.GetAsync();
            }
            else
            {
                ProfesoresServices ns = new ProfesoresServices(1);
                Profesores profesor = await ns.GetProfesorConectado();
                servicioGrupos = new GruposServices(profesor.Id);
                listaGrupos = await servicioGrupos.GetOffline();
            }
                var grupos = (listaGrupos).
                Where(g => !cursosGrupos.Exists(cg => cg.IdGrupo == g.Id)).ToList();
            CursosGruposAgregarActivity cursosGruposAgregarActivity =
                new CursosGruposAgregarActivity(context, cursosGruposAgregadosAdapter, curso, cursosGrupos, agregar, borrar, grupos);
            cursosGruposAgregarActivity.Show();

            // Se restablecen los botones
            ActivarDesactivarBotones(true);
        }

        private void ActivarDesactivarBotones(bool estado)
        {
            btAgregar.Enabled = estado;
            btGuardar.Enabled = estado;
            btCancelar.Enabled = estado;
            alertDialogAndroid.SetCancelable(estado);
        }

        public void Show()
        {
            alertDialogAndroid.Show();
            // Se obtienen los botones para asignarles los métodos nuevos (no cierran el diálogo).
            btGuardar = alertDialogAndroid.GetButton((int)DialogButtonType.Positive);
            btCancelar = alertDialogAndroid.GetButton((int)DialogButtonType.Negative);
            btAgregar = alertDialogAndroid.GetButton((int)DialogButtonType.Neutral);

            // Se asignan las funciones
            btGuardar.Click += Guardar;
            btCancelar.Click += Cancelar;
            btAgregar.Click += Agregar;
        }
    }
}