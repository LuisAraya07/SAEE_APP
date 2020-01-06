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
using SAEEAPP.Adaptadores;
using Xamarin.core.Models;

namespace SAEEAPP.Listeners
{
    class AgregarCGListener : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Activity _context;
        private readonly List<CursosGrupos> _cursosGrupos, agregar, borrar;
        private readonly Grupos _grupo;
        private readonly Cursos _curso;
        private readonly CheckBox chAgregar;

        public AgregarCGListener(Activity context, Cursos curso, Grupos grupo, List<CursosGrupos> cursosGrupos,
            List<CursosGrupos> agregar, List<CursosGrupos> borrar, CheckBox chAgregar)
        {
            _context = context;
            _cursosGrupos = cursosGrupos;
            this.agregar = agregar;
            this.borrar = borrar;
            _curso = curso;
            _grupo = grupo;
            this.chAgregar = chAgregar;
        }

        public void OnClick(View v)
        {
            if (chAgregar.Checked)
            {
                CursosGrupos cursoGrupo;
                int i = borrar.FindIndex(grupoTemp => grupoTemp.IdGrupo == _grupo.Id);
                if (i != -1)
                {
                    cursoGrupo = borrar[i];
                    borrar.RemoveAt(i);
                }
                else
                {
                    cursoGrupo = new CursosGrupos()
                    {
                        Id = -1,
                        IdCurso = _curso.Id,
                        IdGrupo = _grupo.Id,
                        IdGrupoNavigation = _grupo
                    };
                }
                if(cursoGrupo.Id == -1)// Si es igual, entonces es un grupo que no existe, hay que agregarlo
                {
                    agregar.Add(cursoGrupo);
                }
                _cursosGrupos.Add(cursoGrupo);
                Toast.MakeText(_context, "Agregado", ToastLength.Long).Show();
            }
            else
            {
                _cursosGrupos.RemoveAt(_cursosGrupos.FindIndex(grupoTemp => grupoTemp.IdGrupo == _grupo.Id));
                agregar.RemoveAt(agregar.FindIndex(grupoTemp => grupoTemp.IdGrupo == _grupo.Id));
                Toast.MakeText(_context, "Eliminado", ToastLength.Long).Show();
            }
        }
    }

    class BorrarCGListener : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Activity _context;
        private readonly List<CursosGrupos> _cursosGrupos, agregar, borrar;
        private readonly CursosGrupos _cursoGrupo;
        private readonly CursosGruposAgregadosAdapter _cursosGruposAgregadosAdapter;

        public BorrarCGListener(Activity context, CursosGrupos cursoGrupo, List<CursosGrupos> cursosGrupos,
            List<CursosGrupos> agregar, List<CursosGrupos> borrar,
            CursosGruposAgregadosAdapter cursosGruposAgregadosAdapter)
        {
            _context = context;
            _cursosGrupos = cursosGrupos;
            this.agregar = agregar;
            this.borrar = borrar;
            _cursoGrupo = cursoGrupo;
            _cursosGruposAgregadosAdapter = cursosGruposAgregadosAdapter;
        }

        public void OnClick(View v)
        {
            _cursosGrupos.Remove(_cursoGrupo);
            if (_cursoGrupo.Id != -1)// Si es distinto, es un grupo que ya existia, hay que borrarlo
            {
                borrar.Add(_cursoGrupo);
            }
            int i = agregar.FindIndex(grupoTemp => grupoTemp.IdGrupo == _cursoGrupo.IdGrupo);
            if (i != -1)
            {
                agregar.RemoveAt(i);
            }
            Toast.MakeText(_context, "Eliminado", ToastLength.Long).Show();
            _cursosGruposAgregadosAdapter.ActualizarDatos();
        }
    }
}