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

        (string, string, DateTime?) Login(ILoginDTO dto);
        void SignUp(ISignUpDTO dto);

        IProfileDTO GetProfile(int id);

        void UpdateProfilePic(IChangeProfilePicDTO dto);
        void UpdateProfile(IProfileDTO dto);
        (string, string, DateTime?) RenovarToken(string refreshToken);
    }
}
