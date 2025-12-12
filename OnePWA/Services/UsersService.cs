using System.Runtime.InteropServices;
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
                var claims = new List<Claim>
                     {
                         new Claim(ClaimTypes.Name, entidad.Name),
                         new Claim(ClaimTypes.NameIdentifier, entidad.Id.ToString()),
                         new Claim(ClaimTypes.Email, entidad.Email)
                      };

                // Generar token
                var token = jwtHelper.GenerateJwtToken(claims);
                return token;
            }
        }

        public void SignUp(ISignUpDTO dto)
        {
            var entidad = Mapper.Map<Users>(dto);
            entidad.Password = EncriptacionHelper.GetHash(entidad.Password);
            entidad.ProfilePictures = "1.png";
            Repository.Insert(entidad);
        }

        public IProfileDTO GetProfile(int id)
        {
            var perfil = Repository.Get(id);

            IProfileDTO p = new IProfileDTO()
            {
                Contraseña=perfil.Password,
                FotoPerfil=perfil.ProfilePictures,
                Nombre=perfil.Name,

            };
            return p;
        }

        public void UpdateProfilePic(IChangeProfilePicDTO dto)
        {
            var user = Repository.Get(dto.Id);

            user.ProfilePictures = dto.Picture;

            Repository.Update(user);
          
        }


        public void UpdateProfile(IProfileDTO dto)
        {
            var user = Repository.Get(dto.Id);
            user.Password = dto.Contraseña == "Contraseña" ? user.Password : EncriptacionHelper.GetHash(dto.Contraseña);
            user.Name = dto.Nombre;
            Repository.Update(user);

        }
    }
}
