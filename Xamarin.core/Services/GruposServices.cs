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
    public class GruposServices
    {

        private GruposRepositorio _gruposR;
        public GruposServices()
        {
            _gruposR = new GruposRepositorio();

        }

        public List<Grupos> Get(int id) {
        
            return _gruposR.Get(id);

        
        }

        public Grupos GetGrupo(int id) {
            return _gruposR.GetGrupo(id);
        
        }


    }
}