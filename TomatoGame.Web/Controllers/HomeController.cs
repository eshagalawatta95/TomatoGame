using System.Threading.Tasks;
using System.Web.Mvc;
using TomatoGame.Service.Dto;
using TomatoGame.Service.Services;
using TomatoGame.Web.Models;

namespace TomatoGame.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public HomeController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> LoginAsync(UserSignInSignUpViewModal model)
        {
            bool isAuthenticated = await _authService.Login(new LoginDataDto()
            {
                Email = model.LoginEmail,
                Password = model.LoginPassword,
            });

            if (isAuthenticated)
            {
                var user = await _userService.GetUser(model.LoginEmail);
                Session["UserID"] = user.Id.ToString();
                Session["UserName"] = user.Name.ToString();
                Session["UserScore"] = user.LatestScore.ToString();

                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> IsUserExists(string email)
        {
            var isExists = await _userService.IsUserExists(email);
            return Json(isExists, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LogOff()
        {
            //clear session variables
            Session["UserID"] = string.Empty;
            Session["UserName"] = string.Empty;
            Session["UserScore"] = string.Empty;
            return View("Index");
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

            return Json(isSignUpSucess);
        }
    }
}