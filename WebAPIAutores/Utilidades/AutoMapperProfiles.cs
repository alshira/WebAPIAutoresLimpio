using AutoMapper;
using WebAPIAutores.DTOs;
using WebAPIAutores.Models;

namespace WebAPIAutores.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<Autor, AutorDTOconLibros>().ForMember(autorDTO => autorDTO.Libros, opciones=> opciones.MapFrom(MapAutorDTOLibros)); //asignamos metodo manual para hacer la asignación

            //creando una regla especial y especifica para el aptributo de autoreslibros de la entidad  libros
            CreateMap<LibroCreacionDTO, Libro>().ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));//MapAutoresLibros es un metodo creado por nosotros
            CreateMap<Libro, LibroDTO>();
            CreateMap<Libro, LibroDTOconAutores>().ForMember(libroDTO => libroDTO.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores)); //asignamos la funcion personalizada para hacer el mapeo
            CreateMap<LibroPatchDTO, Libro>().ReverseMap();// reverse map es para que se configure el mapeo desde Libro a LibroPatchDTO y no lo tengamos que configurar
            CreateMap<ComentarioCreacionDTO,Comentario>();
            CreateMap<Comentario, ComentarioDTO>();
            
        }

        private List<AutorLibro> MapAutoresLibros(LibroCreacionDTO libroCreacionDTO, Libro libro)
        {
            var resultado = new List<AutorLibro>();
            if (libroCreacionDTO.AutoresIds == null)
            {
                return resultado;
            }
            foreach (var autorId in libroCreacionDTO.AutoresIds)
            {
                resultado.Add(new AutorLibro() { AutorId = autorId });
            }
            return resultado;
        }

        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTO libroDTO)
        {
            var resultado = new List<AutorDTO>();

            if (libro.AutoresLibros == null) { return resultado; }

            foreach (var autorlibro in libro.AutoresLibros)
            {
                resultado.Add(new AutorDTO()
                {
                    Id = autorlibro.AutorId,
                    Nombre = autorlibro.Autor.Nombre
                });
            }
            return resultado;
        }

        private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDTO)
        {
            var resultado = new List<LibroDTO>();
            foreach(var autorLibro in autor.AutoresLibros)
            {
                resultado.Add(new LibroDTO
                {
                    Id = autorLibro.LibroId,
                    Titulo = autorLibro.Libro.Titulo
                });
            }
            return resultado;
        }
    }
}
