namespace WebAPIAutores.Models
{
    public class AutorLibro
    {
        public int LibroId { get; set; }
        public int AutorId { get; set; }
        public int Orden { get; set; }
        //propiedades de navegación
        public Libro Libro { get; set; }// se deben agregar una contraparte tb en la clase incluida
        public Autor Autor { get; set; }// se deben agregar una contraparte tb en la clase incluida
    }
}
