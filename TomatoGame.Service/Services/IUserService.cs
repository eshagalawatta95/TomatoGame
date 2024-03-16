using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomatoGame.Service.Dto;

namespace TomatoGame.Service.Services
{
    public interface IUserService
    {
        Task<bool> SignUp(UserSignUp signUp);

        Task<bool> Login(LoginDataDto login);
    }
}
