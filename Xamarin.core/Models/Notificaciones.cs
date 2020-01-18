using Castle.Components.DictionaryAdapter;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
namespace Xamarin.core.Models
{
    public class Notificaciones
    {

        [Key("Id")]
        public int Id { get; set; }
        public int IdProfesor { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Note { get; set; }
        public int Estado { get; set; }
        public string DiasAntes { get; set; }
        public Notificaciones()
        {

        }
    }
}
