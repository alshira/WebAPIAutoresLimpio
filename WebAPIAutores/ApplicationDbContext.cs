using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Models;

namespace WebAPIAutores
{
    //public class ApplicationDbContext : DbContext
    public class ApplicationDbContext : IdentityDbContext //ahora usamos el identity
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        //vamos a hacer una llave primaria compuesta LibroID y AutorID
        //para ello vamos a utilizar el API fluente que EntityFramework
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AutorLibro>().HasKey(al => new { al.AutorId, al.LibroId }); //configuración especial de la entidad AutorLibro
        }

        //así le decimos que cree una tabla a partir del modelo al que hacemos referencia (esquema/entidad/modelo)
        public DbSet<Autor> Autores { get; set; } //creamos una tabla a partir del modelo
        public DbSet<Libro> Libros { get; set; } //agregamos la segunda tabla a partir del nuevo modelo
        public DbSet<Comentario> Comentarios { get; set; } // agregamos la tabla comentarios a mano para poderla consultar directamente
        public DbSet<AutorLibro> AutoresLibros { get; set;} // agregamos la relación muchos a muchos
    }
}
