using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SAEEAPP.Adaptadores;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.core;
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace SAEEAPP
{
    public class NotasAgregarActivity
    {
        AlertDialog.Builder alertDialogBuilder;
        AlertDialog alertDialogAndroid;
        Activity _context;
        Button  btCancelar,btModificar;
        EstudianteEvaluacion _estudiante;
        Asignaciones _asignacion;
        View VistaAgregar;
        EditText etPuntos, etPorcentaje, etNota;
        EstudianteEvaluacion estudianteTemp;
        NotasAdaptador _adapter;
        List<EstudianteEvaluacion> _estudiantes;
        public NotasAgregarActivity(Activity context,List<EstudianteEvaluacion> estudiantes, EstudianteEvaluacion estudiante, Asignaciones asignacion,NotasAdaptador adapter)
        {
            _context = context;
            _estudiante = estudiante;
            _asignacion = asignacion;
            _adapter = adapter;
            _estudiantes = estudiantes;
             LayoutInflater layoutInflater = LayoutInflater.From(context);
            VistaAgregar = layoutInflater.Inflate(Resource.Layout.Dialogo_Notas_Modificar, null);
            etPuntos = VistaAgregar.FindViewById<EditText>(Resource.Id.etPuntos);
            etPorcentaje = VistaAgregar.FindViewById<EditText>(Resource.Id.etPorcentaje);
            etNota = VistaAgregar.FindViewById<EditText>(Resource.Id.etNota);
                                                                                                                                                            
            etPuntos.Text = _estudiante.Puntos.ToString();
            etPorcentaje.Text = _estudiante.Porcentaje.ToString();
            etNota.Text = _estudiante.Nota.ToString();
            alertDialogBuilder = new AlertDialog.Builder(context, Resource.Style.AlertDialogStyle)
            .SetView(VistaAgregar)
            .SetPositiveButton("Guardar", (EventHandler<DialogClickEventArgs>)null)
            .SetNegativeButton("Cerrar", (EventHandler<DialogClickEventArgs>)null)
            .SetTitle("Notas : "+estudiante.Cedula);
            alertDialogAndroid = alertDialogBuilder.Create();
        }
        private void Cancelar(object sender, EventArgs e)
        {
            alertDialogAndroid.Dismiss();
        }
        private async void Guardar(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(_context);
            var conectado = vc.IsOnline();
            if (conectado)
            {

                if (EntradaValida())
                {
                    // Se bloquean los botones
                    ActivarDesactivarBotones(false);
                    Toast.MakeText(_context, "Guardando, un momento...", ToastLength.Short).Show();

                    EvaluacionesServices serevac = new EvaluacionesServices();
                    estudianteTemp = new EstudianteEvaluacion();

                    Evaluaciones eva = _estudiante.evaluacion;
                    eva.Puntos = Decimal.Parse(etPuntos.Text);
                    eva.Porcentaje = Decimal.Parse(etPorcentaje.Text);
                    eva.Nota = int.Parse(etNota.Text);

                    estudianteTemp.Cedula = _estudiante.Cedula;
                    estudianteTemp.Nombre = _estudiante.Nombre;
                    estudianteTemp.evaluacion = eva;
                    estudianteTemp.Puntos = Decimal.Parse(etPuntos.Text);
                    estudianteTemp.Porcentaje = Decimal.Parse(etPorcentaje.Text);
                    estudianteTemp.Nota = int.Parse(etNota.Text);
                    estudianteTemp.Estado = _estudiante.Estado;
                    bool resultado = await serevac.UpdateEvaluacionAsync(estudianteTemp.evaluacion);
                    if (resultado)
                    {
                        int index = _estudiantes.IndexOf(_estudiante);
                        _estudiantes.Remove(_estudiante);
                        _estudiante = estudianteTemp;
                        _estudiantes.Insert(index,_estudiante);

                       
                        Toast.MakeText(_context, "Guardado correctamente", ToastLength.Long).Show();
                        alertDialogAndroid.SetCancelable(true);
                        alertDialogAndroid.Dismiss();
                        // Se actualiza la lista de cursos
                        _adapter.ActualizarDatos();
                    }
                    else
                    {
                        // Se restablecen los botones
                        ActivarDesactivarBotones(true);
                        Toast.MakeText(_context, "Error al guardar, intente nuevamente", ToastLength.Short).Show();
                    }
                }
            }
            else
            {
                ActivarDesactivarBotones(true);
                Toast.MakeText(_context, "Necesita conexión a internet", ToastLength.Short).Show();
            }

        }
        private bool EntradaValida()
        {
            if (Decimal.Parse(etPuntos.Text) <= 0)
            {
                Toast.MakeText(_context, "Los puntos deben ser mayor a cero", ToastLength.Long).Show();
                return false;
            }
            else if (Decimal.Parse(etPorcentaje.Text) <= 0)
            {
                Toast.MakeText(_context, "El porcentaje debe ser mayor a cero", ToastLength.Long).Show();
                return false;
            }
            else if (int.Parse(etNota.Text) <= 0)
            {
                Toast.MakeText(_context, "La nota debe ser mayor a cero", ToastLength.Long).Show();
                return false;
            }

            else if (Decimal.Parse(etPuntos.Text) > _asignacion.Puntos)
            {
                Toast.MakeText(_context, "La puntuación máxima es: "+ _asignacion.Puntos, ToastLength.Long).Show();
                return false;
            }
            else if (Decimal.Parse(etPorcentaje.Text) > _asignacion.Porcentaje)
            {
                Toast.MakeText(_context, "El porcentaje máximo es: "+ _asignacion.Porcentaje, ToastLength.Long).Show();
                return false;
            }
            else if (int.Parse(etNota.Text) > 100)
            {
                Toast.MakeText(_context, "La nota máxima es 100", ToastLength.Long).Show();
                return false;
            }
            else
            {
                return true;
            }
        }
        private void ActivarDesactivarBotones(bool estado)
        {
            btModificar.Enabled = estado;
            btCancelar.Enabled = estado;
            alertDialogAndroid.SetCancelable(estado);
        }
        public void Show()
        {
            alertDialogAndroid.Show();
            btCancelar = alertDialogAndroid.GetButton((int)DialogButtonType.Negative);
            btCancelar.Click += Cancelar;
            btModificar = alertDialogAndroid.GetButton((int)DialogButtonType.Positive);
            btModificar.Click += Guardar;
        }
    }
}