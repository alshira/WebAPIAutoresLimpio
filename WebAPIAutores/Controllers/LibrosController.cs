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
            //var libro = await context.Libros.Include(libroDB => libroDB.Comentarios).FirstOrDefaultAsync(x => x.Id == id);//uso de include así le pedimos que incluya los comentarios, eso genera un join 
            var libro = await context.Libros
                .Include(libroDB => libroDB.AutoresLibros) //el include del resistro en autores libros
                .ThenInclude(autorLibroDB=> autorLibroDB.Autor) // hacemos un include anidado e incluimos el autor
                .FirstOrDefaultAsync(x => x.Id == id);//uso de include así le pedimos que incluya los comentarios, eso genera un join 
                libro.AutoresLibros = libro.AutoresLibros.OrderBy(x=> x.Orden).ToList();

            return mapper.Map<LibroDTO>(libro);

            //ahora traemos los datos del autor también
            //return await context.Libros.Include(x=>x.Autor).FirstOrDefaultAsync(x => x.Id == id);
        }

       
        // POST api/<LibrosController>
        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO)
        {
            if (libroCreacionDTO.AutoresIds == null)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }
            
            //aquí hacemos un query en el cual traemos los autores y seleccionamos únicamente el ID de ellos
            var autoresIds = await context.Autores.Where(autorDB => libroCreacionDTO.AutoresIds.Contains(autorDB.Id)).Select(x=> x.Id).ToListAsync();  

            if (libroCreacionDTO.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest($"No existe uno de los autores enviados: {libroCreacionDTO.AutoresIds.Count}<>{autoresIds.Count}");
            }
           
            var libro = mapper.Map<Libro>(libroCreacionDTO);

            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }

            }

            context.Add(libro);//marcamos para agregar
            await context.SaveChangesAsync();//guardamos los cambios
            return Ok();

        }

        
     
    }
}
