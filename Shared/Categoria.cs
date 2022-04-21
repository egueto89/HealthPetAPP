using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPetAPP.Shared
{
   public class Categoria
    {
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "La descripción  es requerida")]
        [StringLength(100, ErrorMessage = "La descripción supera la cantidad de caracteres permitidos")]
        public string Descripcion { get; set; }
    }
}
