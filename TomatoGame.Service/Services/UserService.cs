using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomatoGame.Service.Dto;
using TomatoGame.Storage;

namespace TomatoGame.Service.Services
{
    public class UserService: IUserService
    {
        private readonly GameDbContext _context;

        public UserService(GameDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserProfileDto>> GetUsers()
        {
            // Get all users and map them to UserProfileDto
            var users = await _context.Users
                .Select(u => new UserProfileDto
                {
                    Email = u.Email,
                })
                .ToListAsync();

            return users;
        }

        public async Task<UserProfileDto> GetUser(int userId)
        {
            // Get a specific user by userId and map to UserProfileDto
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserProfileDto
                {
                    Id = u.Id,
                    Email = u.Email,
                })
                .FirstOrDefaultAsync();

            return user;
        }
    }
}
