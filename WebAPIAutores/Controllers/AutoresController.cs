using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Filtros;
using WebAPIAutores.Models;
//using WebAPIAutores.Servicios;

namespace WebAPIAutores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
         
        }
        [HttpGet]
       
        public async Task<List<AutorDTO>> Get()
        {
            
          var autores = await context.Autores.ToListAsync();
          return mapper.Map<List<AutorDTO>>(autores);
        }
        
        //usando variables como rutas de acceso
        //usando dos parametros y uno nulo
        //[HttpGet("{id:int}")]//api/[controller]/1   //atributo a nivel de método
        [HttpGet("{id:int}", Name ="obtenerAutor")]//Ponemos nombre a la ruta
        //public async Task<ActionResult<AutorDTO>> Get(int id) //attributo a nivel de prametros
        //public async Task<ActionResult<AutorDTO>> Get(int id) //sin herencia
        public async Task<ActionResult<AutorDTOconLibros>> Get(int id) //con herencia
        {
        
            var autor = await context.Autores
                .Include(autorDB => autorDB.AutoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Libro)
                .FirstOrDefaultAsync(autorDB => autorDB.Id == id);
            
            if (autor == null)
            {
                return NotFound();
            } else
            {
                return mapper.Map<AutorDTOconLibros>(autor);
            }
            
        }

        //usando variables como rutas de acceso
        //[HttpGet("{nombre}")] //api/[controller]/augusto
        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromRoute] string nombre)
        {
            var autores = await context.Autores.Where(autorDB => autorDB.Nombre.Contains(nombre)).ToListAsync();
            
            return mapper.Map<List<AutorDTO>>(autores);
        }
       
        [HttpPost]//attributo a nivel del metodo
        public async Task<ActionResult> Post([FromForm] AutorCreacionDTO autorCreacionDTO) //attributo a nivel de parametros
        {

            //validación a nivel de controller
            var existeAutor = await context.Autores.AnyAsync(autorDB => autorDB.Nombre == autorCreacionDTO.Nombre);
            if (existeAutor)
            {
                return BadRequest($"Ya existe un autor con el nombre {autorCreacionDTO.Nombre}");
            }

            //este es mapeo manual
            /*var autor = new Autor()
            {
                Nombre = autorCreacionDTO.Nombre
            };*/

            //mapeo usando automaper
            var autor = mapper.Map<Autor>(autorCreacionDTO);

            context.Add(autor);//estamos agregando algo que todavía no existe
            await context.SaveChangesAsync();//por eso le decimos que espere a que exista para agregarlo

            var autorDTO = mapper.Map<AutorDTO>(autor); // creamos esto porque no es correcto darle a CreatedATRoute el objeto autor, así no exponemos nuestro aobjeto autor al mundo externo.
            //return Ok();//por ahora reportamos OK aunque en el futuro revisaremos la respuesta real
            return CreatedAtRoute("obtenerAutor",new {id= autor.Id },autorDTO); // new {} esto es un objeto anónimo
        }

        [HttpPut("{id:int}")]//"api/[controller]/id"
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id)
        {
            /*if (autorCreacionDTO.Id != id)
            {
                return BadRequest("el id del autor no coincide con el id de la URL");
            }*/
            var existe = await context.Autores.AnyAsync(autorDB => autorDB.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            var autor = mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;
            context.Update(autor);//marcando el registro que va ser actualizado
            await context.SaveChangesAsync(); //solo aquí se guardan los cambios
            //return Ok();
            return NoContent(); //esto tambien es valido
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(autorDB => autorDB.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Autor() { Id = id });//marcando el registro que va ser borrado
            await context.SaveChangesAsync();//aquí si borro el autor
            return Ok();
        }
    }
}
