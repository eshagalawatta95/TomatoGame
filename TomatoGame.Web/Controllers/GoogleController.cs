using ASPSnippets.GoogleAPI;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TomatoGame.Service.Dto;
using TomatoGame.Service.Services;
using TomatoGame.Web.Models;

namespace TomatoGame.Web
{
    public class GoogleController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public GoogleController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;

        }

        // GET: /Google/SignIn
        public GoogleProfileModelViewModel GetGoogleAuthData()
        {
            GoogleConnect.ClientId = "223825968887-2ut3vn0ph4juak6k4brcjts848ho3uor.apps.googleusercontent.com";
            GoogleConnect.ClientSecret = "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";
            GoogleConnect.RedirectUri = Request.Url.AbsoluteUri.Split('?')[0];

            GoogleProfileModelViewModel profile = new GoogleProfileModelViewModel();
            string code = Request.QueryString["code"];
            if (!string.IsNullOrEmpty(code))
            {
                GoogleConnect connect = new GoogleConnect();
                string json = connect.Fetch("me", code);
                profile = new JavaScriptSerializer().Deserialize<GoogleProfileModelViewModel>(json);
                profile.IsDisable = true;
            }

            return profile;
        }

        [HttpPost]
        public async Task<ActionResult> Login()
        {
            GoogleConnect.Authorize("profile", "email");
            var data = GetGoogleAuthData();
            if (!string.IsNullOrEmpty(data?.Email))
            {
                await AddUserToSystemIfNotExistAsync(data);
                var user = await _userService.GetUser(data.Email);
                Session["UserID"] = user.Email;
                Session["UserName"] = user.Name;
                Session["UserScore"] = user.LatestScore;

                // Redirect to the dashboard or home page upon successful login
                return RedirectToAction("Index", "Game");
            }

            // Move the return View() outside of the if block
            return View();
        }

        private async Task AddUserToSystemIfNotExistAsync(GoogleProfileModelViewModel data)
        {
            var user = await _userService.GetUser(data.Email);
            if (user == null)
            {
                await _authService.SignUp(new UserSignUp()
                {
                    Email = data.Email,
                    Name = data.Name,
                });
            }
        }
    }
}