using System.Threading.Tasks;
using TomatoGame.Service.Dto;

namespace TomatoGame.Service.Services
{
    public interface IAuthService
    {
        Task<bool> SignUp(UserSignUp signUp);

        Task<bool> Login(LoginDataDto login);


        Task<bool> SignUpWithGoogle(UserSignUp signUp);

        Task<bool> LoginWithGoogle(LoginDataDto login);


        Task<bool> SignUpWithFaceBook(UserSignUp signUp);

        Task<bool> LoginWithFaceBook(LoginDataDto login);
    }
}
