using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.DTOs
{
    public class LibroCreacionDTO
    {
        //[Required(ErrorMessage = "El campo {0} es requerido.")] // este es un data anotation de validación, tiene un placeholder que da el nombre del campo
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres.")]
        [PrimeraLetraMayuscula]//custom dataanotation
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public List<int> AutoresIds { get; set; }//attributo de navegación para una lista de autores
    }
}
