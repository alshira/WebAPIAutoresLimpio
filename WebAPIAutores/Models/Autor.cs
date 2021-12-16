using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.Models
{
    public class Autor
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido.")]
        [PrimeraLetraMayuscula]//custom dataanotation
        [StringLength(maximumLength: 120, ErrorMessage = "El Campo {0} no debe tener más de {1} caracteres.")]
        public string Nombre { get; set; }
        public List<AutorLibro> AutoresLibros { get; set; }//relacion muchos a muchos
    }
}
