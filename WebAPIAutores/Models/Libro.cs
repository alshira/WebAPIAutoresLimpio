using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.Models
{
    
    public class Libro 


    {
        public int Id { get; set; }
        [Required]
        [PrimeraLetraMayuscula]//custom dataanotation
        [StringLength(maximumLength:250)]
        public string Titulo { get; set; }
        //propiedad de navegación
        // nos va permitir traer la lista de comentarios de un libro en espcífico
        public List<Comentario> Comentarios { get; set; }
        /* limpieza de codigo
         * public int AutorId { get; set; }
        [NotMapped]//esto es para que no se hagan las migraciones y no afecte la bd
        public int Menor { get; set; }
        [NotMapped]//esto es para que no se hagan las migraciones y no afecte la bd
        public int Mayor { get; set; }
        public Autor Autor { get; set; }// esta es una propiedad de navegación(una relación con otro objeto)

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Titulo))
            {
                var primeraLetra = Titulo[0].ToString();
                if(primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayúscula",new string[] { nameof(Titulo)});
                }
            }

            if(Menor > Mayor)
            {
                yield return new ValidationResult("El valor de " + nameof(Menor).ToString() + " no puede ser más grande que el campo " + nameof(Mayor).ToString(), new string[] { nameof(Menor) });
                //yield return new ValidationResult("El valor de no puede ser más grande que el campo Mayor ",new string[] { nameof(Menor) });
            }
            //throw new NotImplementedException();
        }
        */
    }
}
