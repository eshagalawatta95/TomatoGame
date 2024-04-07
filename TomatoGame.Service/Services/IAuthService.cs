using System.Threading.Tasks;
using TomatoGame.Service.Dto;

namespace TomatoGame.Service.Services
{
    public interface IAuthService
    {
        Task<bool> SignUp(UserSignUp signUp);

        Task<bool> Login(LoginDataDto login);
    }
}
