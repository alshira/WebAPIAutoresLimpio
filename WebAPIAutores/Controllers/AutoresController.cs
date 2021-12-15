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
        /*limpiando código
         * private readonly iServicio servicio;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingleton servicioSingleton;
        private readonly ILogger<AutoresController> logger;*/

        //estamos ocupando inyección de dependencias, esto es, como en Startup.cs definimos 
        //el servicio de dbcontex, podemos desde el contructor de cualquier clase invocar dicha definición
        // es como varaibles globales asignadas a través de las clases.
        /*limpiando código
         * public AutoresController(ApplicationDbContext context,iServicio servicio,
            ServicioTransient servicioTransient, ServicioScoped servicioScoped,
            ServicioSingleton servicioSingleton, ILogger<AutoresController> logger)*/
        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
            /*limpiando código
             * this.servicio = servicio;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSingleton = servicioSingleton;
            this.logger = logger;*/
        }
        [HttpGet]
        /*Limpiando código
         * [HttpGet("listado")]// api/autores/listado
        [HttpGet("/listado")]// /listado
        
        [ResponseCache(Duration = 10)] //aplicando el filtro de cache 10s
        [ServiceFilter(typeof(MiFiltroDeAccion))] // agregando filtro personalizado
        */
        //[Authorize] //authorize implementado a nivel de metodo
        //modo fijo o convencional
        //public ActionResult<List<Autor>> Get()
        //usando asyc
        public async Task<ActionResult<List<Autor>>> Get()
        {
            //agregamos por código una excepción para que exista un error y se vaya al filtro grlobal personalizado que creamos
            //throw new NotImplementedException();

            /* limpiando código
             * logger.LogInformation("Estamos obteniendo listado de autores");
            logger.LogWarning("Este es un msg de warning");
            servicio.RealizarTarea();*/

            /* Esto era fijo
             * return new List<Autor>() { 
                 new Autor() { Id=1, nombre="Augusto"},
                 new Autor() { Id=2, nombre="Claudia"}
             };*/
            //traiamos unicamente los datos del autor
            return await context.Autores.ToListAsync();
            //tramos ahora los datos de los libros tambíen
            //return await context.Autores.Include(x=>x.Libros).ToListAsync();
        }

        /* limpiando código
        [HttpGet("primero")]//api/[controller]/primero
        public async Task<ActionResult<Autor>> PrimerAutor()
        {
            
            return await context.Autores.FirstOrDefaultAsync();
        }*/


        //usando variables como rutas de acceso
        //usando dos parametros y uno nulo
        [HttpGet("{id:int}")]//api/[controller]/1   //atributo a nivel de método
        public async Task<ActionResult<Autor>> Get([FromRoute] int id) //attributo a nivel de prametros
        {
        
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            
            if (autor == null)
            {
                return NotFound();
            } else
            {
                return Ok(autor);
            }
            
        }
        //usando variables como rutas de acceso
        [HttpGet("{nombre}")]//api/[controller]/augusto
        public async Task<ActionResult<Autor>> Get(string Nombre)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Nombre == Nombre);
            if (autor == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(autor);
            }
        }
        /* limpiando código
        [HttpGet("GUID")]
        [ResponseCache(Duration =10)] //aplicando el filtro de cache 10s
        [ServiceFilter(typeof(MiFiltroDeAccion))] // agregando filtro personalizado
        public ActionResult ObtenerGuids()
        {
            return Ok(new
            {
                AutoresController_Transient = servicioTransient.Guid,
                ServicioA_Transient = servicio.ObtenerTransient(),
                AutoresController_Scoped = servicioScoped.Guid,
                ServicioA_Scoped = servicio.ObtenerScoped(),
                AutoresController_Singleton = servicioSingleton.Guid,                             
                ServicioA_Singleton = servicio.ObtenerSingleton(),
            }) ;
        }
        */

            [HttpPost]//attributo a nivel del metodo
        public async Task<ActionResult> Post([FromForm] AutorCreacionDTO autorCreacionDTO) //attributo a nivel de parametros
        {

            //validación a nivel de controller
            var existeAutor = await context.Autores.AnyAsync(x => x.Nombre == autorCreacionDTO.Nombre);
            if (existeAutor)
            {
                return BadRequest($"Ya existe un autor con el nombre {autorCreacionDTO.Nombre}");
            }

            //este es mapeo manual
            var autor = new Autor()
            {
                Nombre = autorCreacionDTO.Nombre
            };

            context.Add(autor);//estamos agregando algo que todavía no existe
            await context.SaveChangesAsync();//por eso le decimos que espere a que exista para agregarlo
            return Ok();//por ahora reportamos OK aunque en el futuro revisaremos la respuesta real
        }

        [HttpPut("{id:int}")]//"api/[controller]/id"
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if (autor.Id != id)
            {
                return BadRequest("el id del autor no coincide con el id de la URL");
            }
            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Update(autor);//marcando el registro que va ser actualizado
            await context.SaveChangesAsync(); //solo aquí se guardan los cambios
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(context => context.Id == id);
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
