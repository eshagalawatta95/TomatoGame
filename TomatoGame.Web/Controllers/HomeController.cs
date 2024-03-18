using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TomatoGame.Service.Dto;
using TomatoGame.Service.Services;
using TomatoGame.Web.Models;
using Autofac;
using Autofac.Integration.Mvc;

namespace TomatoGame.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthService _authService;

        public HomeController(IAuthService authService)
        {
            _authService = authService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LoginAsync(UserSignInSignUpViewModal model)
        {
            bool isAuthenticated = await _authService.Login(new LoginDataDto()
            {
                Email = model.Email,
                Password = model.Password,
            });

            if (isAuthenticated)
            {
                // Redirect to the dashboard or home page upon successful login
                return RedirectToAction("Index", "Game");
            }
            else
            {
                // Display an error message if login fails
                ModelState.AddModelError("", "Invalid email or password");
                return View("Index", model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SignUpAsync(UserSignInSignUpViewModal model)
        {
            bool isSignUpSucess = await _authService.SignUp(new UserSignUp()
            {
                Email = model.Email,
                Password = model.Password,
                Name = model.Name,
            });

            if (isSignUpSucess)
            {
                // Redirect to the dashboard or home page upon successful login
                return RedirectToAction("Index", "Game");
            }
            else
            {
                // Display an error message if login fails
                ModelState.AddModelError("", "Invalid Data");
                return View("Index", model);
            }
        }
    }
}