using AutoMapper;
using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;

namespace OnePWA.Mappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap(typeof(ISignUpDTO), typeof(Users));
            CreateMap<Users, ISignUpDTO>();
            //CreateMap<AgregarTareaDTO, Tareas>();
            //CreateMap<Tareas, TareaDTO>();
        }
    }
}
