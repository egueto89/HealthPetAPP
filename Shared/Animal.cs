using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPetAPP.Shared
{
   public class Animal
    {
        public int IdAnimal { get; set; }
       
        public int IdDueno { get; set; }

        public string cedulaDueno { get; set; }
        
        [Required(ErrorMessage = "El nombre del animal es requerida")]
        [StringLength(20, ErrorMessage = "El nombre del animal supera la cantidad de caracteres permitidos")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El tipo de animal es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una tipo de animal")]
        public int IdTipoAnimal { get; set; }
        public string DesTipoAnimal { get; set; }

        [Required(ErrorMessage = "La edad  es requerida")]
        [Range(1, 255, ErrorMessage = "Debe agregar una edad válida")]
        public Int16 Edad { get; set; }

        [Required(ErrorMessage = "La raza es requerida")]
        [StringLength(150, ErrorMessage = "La raza supera la cantidad de caracteres permitidos")]
        public string Raza { get; set; }
    }
}
