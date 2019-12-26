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
using Xamarin.core.Data;
using Xamarin.core.Models;

namespace Xamarin.core.Services
{
    public class ProfesoresServices
    {
        private readonly ProfesoresRepositorio _profesoresR;
        public ProfesoresServices()
        {
            _profesoresR = new ProfesoresRepositorio();
        }

        public List<Profesores> Get()
        {
            return _profesoresR.Get();
        }

        public Profesores GetGrupo(int id)
        {
            return _profesoresR.GetGrupo(id);
        }
    }
}