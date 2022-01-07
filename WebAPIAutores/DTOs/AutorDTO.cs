namespace WebAPIAutores.DTOs
{
    public class AutorDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }  
        //los manejamops usando herencia, creamos la clase AutorDTOcon Libros
        //public List<LibroDTO> Libros { get; set; }// agregando soporte para listar los libros asociados al autor
    }
}
