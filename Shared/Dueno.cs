using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPetAPP.Shared
{
    public class Dueno
    {
        public int IdDueno { get; set; }

        [Required(ErrorMessage = "El nombre es requerida")]
        [StringLength(100, ErrorMessage = "El nombre supera la cantidad de caracteres permitidos")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Los apellidos son requerida")]
        [StringLength(200, ErrorMessage = "Los apellidos superan la cantidad de caracteres permitidos")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "La cédula es requerida")]
        [StringLength(50, ErrorMessage = "La cédula supera la cantidad de caracteres permitidos")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "La cédula es requerida")]
        [StringLength(25, ErrorMessage = "La cédula supera la cantidad de caracteres permitidos")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El correo es requerida")]
        [StringLength(50, ErrorMessage = "El correo supera la cantidad de caracteres permitidos")]
        public string Correo { get; set; }

    }
}
