using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPIAutores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET: api/<LibrosController>/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTO>> Get(int id)
        {
            //solo traiamos los datos del libro
            var libro = await context.Libros.Include(libroDB => libroDB.Comentarios).FirstOrDefaultAsync(x => x.Id == id);//uso de include así le pedimos que incluya los comentarios, eso genera un join 
            return mapper.Map<LibroDTO>(libro);

            //ahora traemos los datos del autor también
            //return await context.Libros.Include(x=>x.Autor).FirstOrDefaultAsync(x => x.Id == id);
        }

       
        // POST api/<LibrosController>
        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO)
        {
            /*var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId); //consultamos si el autor que nos dieron existe
            if (!existeAutor)
            {
                return BadRequest($"El autor que proporcionaste no existe:{libro.AutorId}");
            }*/
            var libro = mapper.Map<Libro>(libroCreacionDTO);
            context.Add(libro);//marcamos para agregar
            await context.SaveChangesAsync();//guardamos los cambios
            return Ok();

        }

        
     
    }
}
