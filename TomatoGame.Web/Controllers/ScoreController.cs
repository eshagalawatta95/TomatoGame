using System.Web.Mvc;
using TomatoGame.Service.Services;
using TomatoGame.Web.Attributes;

namespace TomatoGame.Web.Controllers
{
    [SessionAuthorize("UserID")]
    public class ScoreController : Controller
    {
        private readonly IScoreService _scoreService;
        // GET: Score

        public ScoreController(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        public ActionResult Index()
        {
            var scores = _scoreService.GetScores();
            return View(scores);
        }
    }
}