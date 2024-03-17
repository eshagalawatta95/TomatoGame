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
        Task<List<UserProfileDto>> GetUsers();

        Task<UserProfileDto> GetUser(int userId);
    }
}
