namespace WebAPIAutores.Models
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Contenido { get; set; }
        public int LibroId { get; set; }
        //propiedad de navegación
        //cargar el libro al que le corresponde el comentario
        public Libro Libro { get; set; }
    }
}
