using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPetAPP.Shared
{
    public class TipoAnimal
    {
        public int IdTipoAnimal { get; set; }

        [Required(ErrorMessage = "El tipo de animal es requerida")]
        [StringLength(200, ErrorMessage = "El tipo de animal supera la cantidad de caracteres permitidos")]
        public string Descripcion { get; set; }
    }
}
