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
using Xamarin.core.Models;
using Xamarin.core.Services;

namespace Xamarin.core.Data
{
    public class GruposRepositorio
    {
        private List<Grupos> _grupos;
        private WebServices ws;
        public GruposRepositorio() {
            ws = new WebServices();
        }

        public List<Grupos> Get(int id) {
            var response = ws.Get(ValuesServices.url+"Grupos/GetGrupos?id="+id);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Grupos>>(response.Content);
            
        }

        public Grupos GetGrupo(int id)
        {
            var response = ws.Get(ValuesServices.url + "Grupos/"+id);
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK) {
                throw new ApplicationException("Grupo no encontrado");
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Grupos>(response.Content);

        }


    }
}