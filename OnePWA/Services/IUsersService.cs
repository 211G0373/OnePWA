using AutoMapper;
using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;
using OnePWA.Repositories;

namespace OnePWA.Services
{
    public interface IUsersService
    {
        IMapper Mapper { get; }
        IRepository<Users> Repository { get; }

        string Login(ILoginDTO dto);
        void SignUpUsuario(ISignUpDTO dto);
    }
}
