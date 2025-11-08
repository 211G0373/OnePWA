using System.Security.Claims;
using AutoMapper;
using OnePWA.Helpers;
using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;
using OnePWA.Repositories;

namespace OnePWA.Services
{
    public class UsersService : IUsersService
    {
        private readonly JwtHelper jwtHelper;
        public IRepository<Users> Repository { get; }
        public IMapper Mapper { get; }



        public UsersService(IRepository<Users> repository, IMapper mapper, JwtHelper jwtHelper)
        {
            Repository = repository; 
            Mapper = mapper;
            this.jwtHelper = jwtHelper;
        }




        public string Login(ILoginDTO dto)
        {
            //Regresa el JWT si le permite iniciar sesion
            var hash = EncriptacionHelper.GetHash(dto.Password);
            var entidad = Repository.GetAll()
                .FirstOrDefault(x => x.Email == dto.Email
                && x.Password == hash);
            if (entidad == null)
            { return string.Empty; }
            else
            {
                //Crear las claims, elegir entre ClaimType o nombre personalizado
                //No se usan las dos formas, aqui estan con proposito de ejemplo
                List<Claim> claims = [
                    new Claim(ClaimTypes.Name, entidad.Name),
                    new Claim("Nombre", entidad.Name),

                    new Claim(ClaimTypes.NameIdentifier,entidad.Id.ToString()),
                    new Claim("Id", entidad.Id.ToString()),

                    new Claim("Email", entidad.Email)];
                //Generar el token
                var token = jwtHelper.GenerateJwtToken(claims);
                //Regresa el token
                return token;
            }
        }

        public void SignUp(ISignUpDTO dto)
        {
            var entidad = Mapper.Map<Users>(dto);
            entidad.Password = EncriptacionHelper.GetHash(entidad.Password);
            Repository.Insert(entidad);
        }
    }
}
