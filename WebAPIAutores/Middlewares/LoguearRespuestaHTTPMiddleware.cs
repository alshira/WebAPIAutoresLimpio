namespace WebAPIAutores.Middlewares
{

    //para no exponer la clase a usar debemos hacer una clase estatica que mande llamar a Invoke
    //esto es un metodo de extensión y para crealo hacemos una nueva clase estatica "Extensions"
    public static class LoguearRespuestaHTTPMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoguearRespuestaHTTP(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
        }  
    }


    public class LoguearRespuestaHTTPMiddleware
    {
        private readonly RequestDelegate siguiente;
        private readonly ILogger<LoguearRespuestaHTTPMiddleware> logger;

       
        public LoguearRespuestaHTTPMiddleware(RequestDelegate siguiente, ILogger<LoguearRespuestaHTTPMiddleware> logger)
        {
            this.siguiente = siguiente;
            this.logger = logger;
        }

        //una regla para poder utilizar esta clase como middleware es que necesita tener
        // es que debe tener un método publico llamado invoke o invokeAsync
        //debe retornar una tarea(TASK) y aceptar como primer parámetro un HTTPcontext
        public async Task Invoke(HttpContext contexto)
        {
            using (var ms = new MemoryStream())
            {
                var cuerpoOriginalRespuesta = contexto.Response.Body;
                contexto.Response.Body = ms;
                await siguiente(contexto);//este es un delegado

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
        }
        
    }
}
