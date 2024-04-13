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
                Email = model.Email,
                Password = model.Password,
            });

            if (isAuthenticated)
            {
                // Redirect to the dashboard or home page upon successful login
                var user = await _userService.GetUser(model.Email);
                Session["UserID"] = user.Id.ToString();
                Session["UserName"] = user.Name.ToString();
                Session["UserScore"] = user.LatestScore.ToString();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> LogOffAsync()
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

            if (isSignUpSucess)
            {
                // Redirect to the dashboard or home page upon successful login
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Display an error message if login fails
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}