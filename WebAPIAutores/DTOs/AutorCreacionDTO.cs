using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.DTOs
{
    public class AutorCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")] // este es un data anotation de validación, tiene un placeholder que da el nombre del campo
        [StringLength(maximumLength: 100, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres.")]
        [PrimeraLetraMayuscula]//custom dataanotation
        public string Nombre { get; set; }
    }
}
