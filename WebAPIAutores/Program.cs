using Microsoft.OpenApi.Models;

namespace WebAPIAutores; // esta es la nueva forma de colocar namespaces without {}
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }
    //el operador => se refiere a una función lambda o función anómina, es decir sin nombre
    private static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
     {
         webBuilder.UseStartup<Startup>();
         //throw new NotImplementedException();
     });
}


//// esto es un TOP level statement
//// no existe declarada ninguna class, solo el código, esto es nuevo de c#9
////es para simplificar el código.
////falta la clase start up
//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
