using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPetAPP.Shared
{
   public class Cita
    {


        public Cita()
        {
            Fecha =DateTime.Now;
            FechaAnterior = Fecha;
            HoraAnterior = Hora;

        }
        public int IdCita { get; set; }

        //[Required(ErrorMessage = "El dueño es requerida")]
        //[Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un dueño")]
        public int IdDueno { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría")]
        public int IdCategoria { get; set; }


        [Required(ErrorMessage = "Mascota es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Mascota")]
        public int IdAnimal { get; set; }

        [Required(ErrorMessage = "La hora es requerida")]
        [StringLength(8, ErrorMessage = "La hora supera la cantidad de caracteres permitidos")]
        public string Hora { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        [DataType(DataType.DateTime, ErrorMessage ="No es una fech válida")]
        public DateTime Fecha { get; set; }

        public string Estado { get; set; }

        

        public string NombreDueno { get; set; }
        public string CedulaDueno { get; set; }

        public string DesCategoria { get; set; }

        public DateTime FechaAnterior { get; set; }

        public string HoraAnterior { get; set; }

        public Dictionary<string,string> CargarHoras()
        {
            Dictionary<string,string> dic = new Dictionary<string,string>();
            dic.Add("08:00 am", "08:00 am");
            dic.Add("08:30 am", "08:30 am");
            dic.Add("09:00 am", "08:00 am");
            dic.Add("09:30 am", "09:30 am");
            dic.Add("10:00 am", "10:00 am");
            dic.Add("10:30 am", "10:30 am");
            dic.Add("11:00 am", "11:00 am");
            dic.Add("11:30 am", "11:30 am");
            dic.Add("13:00 am", "13:00 am");
            dic.Add("13:30 am", "13:30 am");
            dic.Add("14:00 am", "14:00 am");
            dic.Add("14:30 am", "14:30 am");
            dic.Add("15:00 am", "15:00 am");
            dic.Add("15:30 am", "15:30 am");
            dic.Add("16:00 am", "16:00 am");
            dic.Add("16:30 am", "16:30 am");

            return dic;
        }
    }
}