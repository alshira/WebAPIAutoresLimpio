using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Models;

namespace WebAPIAutores
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        //así le decimos que cree una tabla a partir del modelo al que hacemos referencia (esquema/entidad/modelo)
        public DbSet<Autor> Autores { get; set; } //creamos una tabla a partir del modelo
        public DbSet<Libro> Libros { get; set; } //agregamos la segunda tabla a partir del nuevo modelo
        public DbSet<Comentario> Comentarios { get; set; } // agregamos la tabla comentarios a mano para poderla consultar directamente
    }
}
