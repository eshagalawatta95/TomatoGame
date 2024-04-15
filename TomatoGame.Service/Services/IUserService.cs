using System.Collections.Generic;
using System.Threading.Tasks;
using TomatoGame.Service.Dto;

namespace TomatoGame.Service.Services
{
    public interface IUserService
    {
        Task<List<UserProfileDto>> GetUsers();

        Task<UserProfileDto> GetUser(string email);

        Task<UserProfileDto> GetUser(int userId);

        Task<bool> IsUserExists(string email);
    }
}
