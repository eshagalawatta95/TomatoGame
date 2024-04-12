using System.Threading.Tasks;
using System.Web.Mvc;
using TomatoGame.Service.Services;
using TomatoGame.Web.Attributes;
using TomatoGame.Web.Models;

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

        public async Task<ActionResult> Index()
        {
            var scores = await _scoreService.GetScores();
            var vm = scores.ConvertAll(score => new ScoreViewModel
            {
                UserName = score.UserName,
                UpdatedDate = score.UpdatedDate,
                LatestScore = score.LatestScore,
                Mode = score.Mode,
            });
            return View(vm);
        }
    }
}