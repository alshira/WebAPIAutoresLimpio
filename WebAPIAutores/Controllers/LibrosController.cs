using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPIAutores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/<LibrosController>/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            //solo traiamos los datos del libro
            return await context.Libros.FirstOrDefaultAsync(x => x.Id == id);
            //ahora traemos los datos del autor también
            //return await context.Libros.Include(x=>x.Autor).FirstOrDefaultAsync(x => x.Id == id);
        }

      /* 
        // POST api/<LibrosController>
        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId); //consultamos si el autor que nos dieron existe
            if (!existeAutor)
            {
                return BadRequest($"El autor que proporcionaste no existe:{libro.AutorId}");
            }
            context.Add(libro);//marcamos para agregar
            await context.SaveChangesAsync();//guardamos los cambios
            return Ok();

        }

        */
     
    }
}
