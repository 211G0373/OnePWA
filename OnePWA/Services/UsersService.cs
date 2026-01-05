using System.Runtime.InteropServices;
using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        public IRepository<RefreshTokens> RefreshTokenRepository { get; }

        public UsersService(IRepository<Users> repository, IMapper mapper, JwtHelper jwtHelper, IRepository<RefreshTokens> refreshTokenRepository)
        {
            Repository = repository;
            Mapper = mapper;
            this.jwtHelper = jwtHelper;
            RefreshTokenRepository=refreshTokenRepository;
        }




        public (string, string) Login(ILoginDTO dto)
        {
            //Regresa el JWT si le permite iniciar sesion
            var hash = EncriptacionHelper.GetHash(dto.Password);
            var entidad = Repository.GetAll()
                .FirstOrDefault(x => x.Email == dto.Email
                && x.Password == hash);
            if (entidad == null)
            { return (string.Empty, string.Empty); }
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
                var refreshToken = Guid.NewGuid().ToString();
                var entidadRefreshToken = new RefreshTokens
                {
                    IdUsuario = entidad.Id,
                    Creado = DateTime.Now,
                    Expiracion = DateTime.Now.AddMonths(3),
                    Token = refreshToken,
                    Usado = 0
                };
                RefreshTokenRepository.Insert(entidadRefreshToken);
                
                return (token, refreshToken);
            }
        }
        public (string, string) RenovarToken(string refreshToken)
        {
            //Verificar que exista el refresh token

            var entidad = RefreshTokenRepository.GetAll().
                AsQueryable().Include(x=>x.IdUsuarioNavigation).
                Where(x => x.Token == refreshToken).FirstOrDefault();
            //Validarlo
            if (entidad!=null && entidad.Usado==0 && entidad.Expiracion > DateTime.Now)
            {
                entidad.Usado = 1;
                RefreshTokenRepository.Update(entidad);

                //Crear las claims, elegir entre ClaimType o nombre personalizado
                //No se usan las dos formas, aqui estan con proposito de ejemplo
                List<Claim> claims = [
                    new Claim(ClaimTypes.Name, entidad.IdUsuarioNavigation.Name),
                    new Claim("Nombre", entidad.IdUsuarioNavigation.Name),

                    new Claim(ClaimTypes.NameIdentifier,entidad.IdUsuarioNavigation.Id.ToString()),
                    new Claim("Id", entidad.IdUsuarioNavigation.Id.ToString()),

                    new Claim("Correo", entidad.IdUsuarioNavigation.Email)];
                //Generar el token
                var token = jwtHelper.GenerateJwtToken(claims);


                var refreshtoken = Guid.NewGuid().ToString();

                var entidadRefreshToken = new RefreshTokens
                {
                    IdUsuario = entidad.Id,
                    Creado = DateTime.Now,
                    Expiracion = DateTime.Now.AddMonths(3),
                    Token = refreshtoken,
                    Usado = 0
                };
                //RefreshTokenRepository.Insert(entidadRefreshToken);



                //Regresa el token
                return (token, refreshToken);

            }
            else
            {
                return ("", "");
            }
        }



        public void SignUp(ISignUpDTO dto)
        {
            var entidad = Mapper.Map<Users>(dto);
            entidad.Password = EncriptacionHelper.GetHash(entidad.Password);
            entidad.ProfilePictures = "1.jpg";
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
            user.Name = dto.Nombre;
            user.ProfilePictures=dto.FotoPerfil;
            Repository.Update(user);

        }
    }
}
