using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.Models
{
    //vamos a ver unn tipo de validación que mezcle varias validaciones para lo cual vamos a 
    //convertir la clase a heredar IValidatableObject
    //public class Libro
    public class Libro : IValidatableObject


    {
        public int Id { get; set; }
        [PrimeraLetraMayuscula]//custom dataanotation
        public string Titulo { get; set; }
        public int AutorId { get; set; }
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
    }
}
