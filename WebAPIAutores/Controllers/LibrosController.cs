using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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
        //[HttpGet("{id:int}")]
        [HttpGet("{id:int}", Name ="ObtenerLibro")] // le agregamos un nombre a la ruta
        //public async Task<ActionResult<LibroDTO>> Get(int id)// sin herencia
        public async Task<ActionResult<LibroDTOconAutores>> Get(int id) // con herencia
        {
            //solo traiamos los datos del libro
            //var libro = await context.Libros.Include(libroDB => libroDB.Comentarios).FirstOrDefaultAsync(x => x.Id == id);//uso de include así le pedimos que incluya los comentarios, eso genera un join 
            
            /*Include y then inclulde
             * var libro = await context.Libros
                .Include(libroDB => libroDB.AutoresLibros) //el include del resistro en autores libros
                .ThenInclude(autorLibroDB=> autorLibroDB.Autor) // hacemos un include anidado e incluimos el autor
                .FirstOrDefaultAsync(x => x.Id == id);//uso de include así le pedimos que incluya los comentarios, eso genera un join 
                libro.AutoresLibros = libro.AutoresLibros.OrderBy(x=> x.Orden).ToList();*/
            var libro = await context.Libros
               .Include(libroDB => libroDB.Comentarios) // el include para los comentarios
               .Include(libroDB => libroDB.AutoresLibros) //el include del resistro en autores libros
               .ThenInclude(autorLibroDB => autorLibroDB.Autor) // hacemos un include anidado e incluimos el autor
               .FirstOrDefaultAsync(x => x.Id == id);//uso de include así le pedimos que incluya los comentarios, eso genera un join 
            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();

            return mapper.Map<LibroDTOconAutores>(libro);

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

            /*if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }

            }*/
            AsignarOrdenAutores(libro);

            context.Add(libro);//marcamos para agregar
            await context.SaveChangesAsync();//guardamos los cambios

            var libroDTO = mapper.Map<LibroDTO>(libro);
            //return Ok();
            return CreatedAtRoute("ObtenerLibro", new {id=libro.Id }, libroDTO); // new {} esto es un objeto anónimo

        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id,LibroCreacionDTO libroCreacionDTO)
        {
            var libroDB = await context.Libros
                .Include(x => x.AutoresLibros)
                .FirstOrDefaultAsync(x => x.Id == id);// esto simplemente está trayendo el libro cuyo aid sea igual a id y al mismo tiempo se está trayendo todos los libros relacionados 

            if (libroDB == null)
            {
                return NotFound();
            }
            libroDB = mapper.Map(libroCreacionDTO, libroDB); //al ahacer la asignación al mismo libroDB hacemos las actualizaciones de los libros asociados fácilmente
            
            AsignarOrdenAutores(libroDB);

            await context.SaveChangesAsync();
            return NoContent();

                 
        }

        private void AsignarOrdenAutores(Libro libro)
        {

            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }

            }
        }


        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id,JsonPatchDocument<LibroPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();//error con el formato que nos enviaron
            }

            var libroDB = await context.Libros.FirstOrDefaultAsync(X => X.Id == id);
            if (libroDB == null)
            {
                return NotFound();
            }

            var libroDTO = mapper.Map<LibroPatchDTO>(libroDB); //cargamos los datos que tiene la base de datos
            patchDocument.ApplyTo(libroDTO,ModelState); //aplicamos los cambios que traiga el patchDocument

            var esValido = TryValidateModel(libroDTO); //le aplicamos las validaciones del modelo LibroDTO
            if (!esValido)
            {
                return BadRequest(ModelState);//devolvemos el error de la validación que no se cumplió
            }

            mapper.Map(libroDTO, libroDB);
            await context.SaveChangesAsync();
            return NoContent();

        }

    }
}
