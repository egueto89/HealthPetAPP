using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPetAPP.Shared
{
    public class FiltroBusquedaCita
    {

        public FiltroBusquedaCita()
        {
            Fecha =DateTime.Now;
        }
        [Required(ErrorMessage ="Debe ingresar una cédula")]
        public string Cedula { get; set; }

        [DataType(DataType.DateTime,ErrorMessage ="Fecha no válida")]
        [Required(ErrorMessage ="Debe ingresar una fecha")]
        public DateTime Fecha { get; set; }
    }
}
