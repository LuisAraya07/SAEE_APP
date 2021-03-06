﻿using Android.App;
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
        Spinner spEstado;
        TextView tvEstado;
        string estadoAsis = "";
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
            tvEstado = VistaAgregar.FindViewById<TextView>(Resource.Id.tvEstado);
            tvEstado.Visibility = ViewStates.Invisible;
            spEstado = VistaAgregar.FindViewById<Spinner>(Resource.Id.spEstado);
            spEstado.Visibility = ViewStates.Invisible;
            if (_asignacion.Tipo.Equals("Asistencia"))
            {
                List<string> listaEstados = new List<string>();
                listaEstados.Add("Presente");
                listaEstados.Add("Escape");
                listaEstados.Add("Justificada");
                listaEstados.Add("Injustificada");
                listaEstados.Add("Tardía");
                spEstado.Visibility = ViewStates.Visible;
                tvEstado.Visibility = ViewStates.Visible;
                spEstado.Prompt = "Elija Estado";
                spEstado.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spEstado_ItemSelected);
                var dataAdapter = new ArrayAdapter(context, Resource.Layout.SpinnerItem, listaEstados);
                dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spEstado.Adapter = dataAdapter;
                if (listaEstados.Contains(_asignacion.Estado))
                {
                    int index = listaEstados.IndexOf(_asignacion.Estado);
                    spEstado.SetSelection(index);
                }


            }
            etPuntos.Text = _estudiante.Puntos.ToString();
            etPorcentaje.Text = _estudiante.Porcentaje.ToString();
            etNota.Text = _estudiante.Nota.ToString();
            alertDialogBuilder = new AlertDialog.Builder(context, Resource.Style.AlertDialogStyle)
            .SetView(VistaAgregar)
            .SetPositiveButton("Guardar", (EventHandler<DialogClickEventArgs>)null)
            .SetNegativeButton("Cerrar", (EventHandler<DialogClickEventArgs>)null)
            .SetTitle("Notas : " + estudiante.Cedula);
            alertDialogAndroid = alertDialogBuilder.Create();
        }
        private void spEstado_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            estadoAsis = spinner.GetItemAtPosition(e.Position).ToString();
            //  Toast.MakeText(context, "You choose:" + spinner.GetItemAtPosition(e.Position), ToastLength.Short).Show();
        }

        private void Cancelar(object sender, EventArgs e)
        {
            alertDialogAndroid.Dismiss();
        }
        private async void Guardar(object sender, EventArgs e)
        {
            VerificarConexion vc = new VerificarConexion(_context);
            var conectado = vc.IsOnline();
            if (EntradaValida())
            {
                // Se bloquean los botones
                ActivarDesactivarBotones(false);
                Toast.MakeText(_context, "Guardando, un momento...", ToastLength.Short).Show();

                EvaluacionesServices serevac;
                estudianteTemp = new EstudianteEvaluacion();

                Evaluaciones eva = _estudiante.evaluacion;
                eva.Puntos = Decimal.Parse(etPuntos.Text);
                eva.Porcentaje = Decimal.Parse(etPorcentaje.Text);
                eva.Nota = int.Parse(etNota.Text);
                if (_asignacion.Tipo.Equals("Asistencia"))
                {
                    eva.Estado = estadoAsis;
                }
                estudianteTemp.Cedula = _estudiante.Cedula;
                estudianteTemp.Nombre = _estudiante.Nombre;
                estudianteTemp.evaluacion = eva;
                estudianteTemp.Puntos = Decimal.Parse(etPuntos.Text);
                estudianteTemp.Porcentaje = Decimal.Parse(etPorcentaje.Text);
                estudianteTemp.Nota = int.Parse(etNota.Text);
                estudianteTemp.Estado = _estudiante.Estado;
                bool resultado;
                if (conectado)
                {
                    serevac = new EvaluacionesServices();
                    resultado = await serevac.UpdateEvaluacionAsync(estudianteTemp.evaluacion);

                }
                else
                {
                    //Toast.MakeText(this, "Necesita conexión a internet.", ToastLength.Long).Show();
                    ProfesoresServices ns = new ProfesoresServices(1);
                    Profesores profesor = await ns.GetProfesorConectado();
                    if (!(profesor == null))
                    {
                        serevac = new EvaluacionesServices(profesor.Id);
                        resultado = await serevac.UpdateEvaluacionOffline(estudianteTemp.evaluacion);
                    }
                    else
                    {
                        resultado = false;
                    }
                }

                if (resultado)
                {
                    int index = _estudiantes.IndexOf(_estudiante);
                    _estudiantes.Remove(_estudiante);
                    _estudiante = estudianteTemp;
                    _estudiantes.Insert(index, _estudiante);


                    Toast.MakeText(_context, "Guardado correctamente", ToastLength.Long).Show();
                    
                    // Se actualiza la lista de cursos
                    _adapter.ActualizarDatos();
                    alertDialogAndroid.Dismiss();
                }
                else
                {
                    // Se restablecen los botones
                    ActivarDesactivarBotones(true);
                    Toast.MakeText(_context, "Error al guardar, intente nuevamente", ToastLength.Short).Show();
                    alertDialogAndroid.Dismiss();
                }
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