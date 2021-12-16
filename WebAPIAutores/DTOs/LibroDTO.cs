using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores.DTOs
{
    public class LibroDTO
    {
        public string Id { get; set; }
        public string Titulo { get; set; }

        public List<ComentarioDTO> Comentarios { get; set; }
    }
}
