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
        }
    }
}
