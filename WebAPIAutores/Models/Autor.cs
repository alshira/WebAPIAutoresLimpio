using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.Models
{
    public class Autor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")] // este es un data anotation de validación, tiene un placeholder que da el nombre del campo
        [StringLength(maximumLength: 100, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres.")]
        [PrimeraLetraMayuscula]//custom dataanotation
        /*limpieza de código
         * [PrimeraLetraMayuscula]//custom dataanotation
         * 
        public string nombre { get; set; }
        [Range(18, 99, ErrorMessage = "El campo {0} debe estar en rango de {1} y {2}")]
        [NotMapped]//esto es para que no se hagan las migraciones y no afecte la bd
        public int Edad { get; set; }
        [CreditCard]
        [NotMapped]//esto es para que no se hagan las migraciones y no afecte la bd
        public string CreditCard { get; set;}
        [Url]
        [NotMapped]//esto es para que no se hagan las migraciones y no afecte la bd
         public string Url { get; set; }
        
        public List<Libro> Libros { get; set; }// esta es una propiedad de navegación relación muchos a uno
        */
        public string nombre { get; set; }
    }
}
