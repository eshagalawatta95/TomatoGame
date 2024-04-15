using System;
using System.Data.Entity;
using System.Threading.Tasks;
using TomatoGame.Service.Dto;
using TomatoGame.Service.Utils;
using TomatoGame.Storage;

namespace TomatoGame.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly GameDbContext _context;

        public AuthService(GameDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SignUp(UserSignUp signUp)
        {
            // Check if the user name already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == signUp.Email);
            if (existingUser != null)
            {
                return false; // User already exists
            }

            // Create a new user
            var newUser = new User
            {
                Email = signUp.Email,
                Password = EncryptAndDecrypter.HashPassword(signUp.Password),
                Name = signUp.Name,
                SignUpDate = DateTime.Now,
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return true; // User signed up successfully
        }

        public async Task<bool> Login(LoginDataDto login)
        {
            // Check if the user name and password match
            var userObj = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
            if (userObj == null)
            {
                return false;
            }
            var isPasswordCorrect = EncryptAndDecrypter.VerifyPassword(login.Password, userObj.Password);
            return isPasswordCorrect;
        }
    }
}
