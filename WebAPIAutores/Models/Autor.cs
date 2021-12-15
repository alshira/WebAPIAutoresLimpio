using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.Models
{
    public class Autor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
