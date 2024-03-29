﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
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

        public Task<bool> SignUpWithGoogle(UserSignUp signUp)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LoginWithGoogle(LoginDataDto login)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SignUpWithFaceBook(UserSignUp signUp)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LoginWithFaceBook(LoginDataDto login)
        {
            throw new NotImplementedException();
        }
    }
}
