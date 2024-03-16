using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomatoGame.Service.Dto;

namespace TomatoGame.Service.Services
{
    public class UserService : IUserService
    {
        public Task<bool> Login(LoginDataDto login)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SignUp(UserSignUp signUp)
        {
            throw new NotImplementedException();
        }
    }
}
