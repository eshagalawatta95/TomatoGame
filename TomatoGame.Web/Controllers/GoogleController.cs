using ASPSnippets.GoogleAPI;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TomatoGame.Web.Models;

namespace TomatoGame.Web
{
    public class GoogleController : Controller
    {
        private readonly string clientId = "223825968887-2ut3vn0ph4juak6k4brcjts848ho3uor.apps.googleusercontent.com";
        private readonly string clientSecret = "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";

        // GET: /Google/SignIn
        public ActionResult Index()
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

            return View(profile);
        }

        [HttpGet]
        public ActionResult Login()
        {
            GoogleConnect.Authorize("profile", "email");
            return RedirectToAction("Index");
        }
    }
}