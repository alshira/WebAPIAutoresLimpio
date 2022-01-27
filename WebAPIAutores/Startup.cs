using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Filtros;
using WebAPIAutores.Middlewares;
//using WebAPIAutores.Servicios;

namespace WebAPIAutores
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        //agregando controllers
        public void ConfigureServices(IServiceCollection services)
        {
            //// Add services to the container.

            /* ANTES del filtro personalizado de logger
             * services.AddControllers().AddJsonOptions(x=>
            x.JsonSerializerOptions.ReferenceHandler= 
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles); // está línea te ayuda a ignorar las inclusiones ciclicas de los modelos
            */
            //agregando filtro personalizado de logger
            services.AddControllers(opciones =>
            {
                opciones.Filters.Add(typeof(FiltroDeExcepcion)); // esta es la aplicación de un filtro GOLBAL!
            }).AddJsonOptions(x =>
           x.JsonSerializerOptions.ReferenceHandler =
           //System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles); // está línea te ayuda a ignorar las inclusiones ciclicas de los modelos
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles).AddNewtonsoftJson(); // Configuramos el NewtonsoftJson

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            //agregando filtro de cache
            services.AddResponseCaching();

            //aplicando el servicio para configurar connectionstring manualmente
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"))
            );

            //agregando filtro de autnenticación
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //configurarmos Automaper.Extensions.Microsoft.DependencyInjection 
            services.AddAutoMapper(typeof(Startup));

        }

        //agregando los middlewares
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //aquí se listan los middlewares
            //se ejecutan de arruba a abajo y las respuestas se regresan de apajo hacia arriba.


            /*
            //uso de un middle ware que permite la ejecución de los subsecuentes
            //harcodeado
            app.Use(async (contexto, siguiente) =>
            {
                using (var ms= new MemoryStream())
                {
                    var cuerpoOriginalRespuesta = contexto.Response.Body;
                    contexto.Response.Body = ms;
                    await siguiente.Invoke();//permitimos la ejecución de otros middleware

                    //cuando me esten devolviendo una respuesta
                    ms.Seek(0, SeekOrigin.Begin);
                    //string respuesta = new StringReader(ms).ReadToEnd(); //esto da error unable conver stream to string
                    StreamReader reader = new StreamReader(ms);
                    string respuesta = reader.ReadToEnd();
                    logger.LogInformation(respuesta);
                    
                    ms.Seek(0, SeekOrigin.Begin);

                    await ms.CopyToAsync(cuerpoOriginalRespuesta);
                    contexto.Response.Body = cuerpoOriginalRespuesta;

                    
                }
            });
            */

            //usamos el middleware desde una clase forma 1 exponiendo la clase a usar
            //app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();

            //usamos el middleware desde una clase forma 2 sin exponer la calse a usar como lo hace  app.UseSwagger();
            app.UseLoguearRespuestaHTTP();

            /* limpieza de código
            //por medio de run creamos un middle ware que detiene la ejecución de todos
            //con el uso de map hacemos una bifurcación de nuestra tubería de procesos
            //esta rama se llama "ruta1"
            app.Map("/ruta1", app =>
            {
                app.Run(async contexto => //=> es una expresión lamda o función anónima.
                {
                    await contexto.Response.WriteAsync("Estoy inteceptando la tubería"); //esto evita la ejecución de los subsecuentes middlewares
                });
            });
            */

            // configure the http request pipeline.
            //le cambiamos el CASING a todas las funciones OJO
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //aplicando filtro de cache
            app.UseResponseCaching();

           

            //despues de agregar services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            //verificamos que este UseAuthorization
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
