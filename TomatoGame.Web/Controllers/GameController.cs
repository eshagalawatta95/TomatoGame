using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using TomatoGame.Service.Dto;
using TomatoGame.Service.Enum;
using TomatoGame.Service.Services;
using TomatoGame.Web.Attributes;
using TomatoGame.Web.Models;

namespace TomatoGame.Web.Controllers
{
    [SessionAuthorize("UserID")]
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IScoreService _scoreService;

        public GameController(IGameService gameService, IScoreService scoreService)
        {
            _gameService = gameService;
            _scoreService = scoreService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Start(GameMode mode)
        {
            var gameData = await GameAction(mode);
            return Json(gameData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void SaveHighScore(GameMode mode, int scoreValue)
        {
            int userId = 0;
            int.TryParse(Session["UserID"].ToString(), out userId);
            var score = new ScoreDto()
            {
                LatestScore = scoreValue,
                Mode = mode,
                UserId = userId,
                UpdatedDate = DateTime.UtcNow,
            };
            _scoreService.UpdateScore(score);
        }

        private async Task<GameDataViewModel> GameAction(GameMode gameMode)
        {
            var gameData = await _gameService.Start(gameMode);
            var vm = new GameDataViewModel()
            {
                Mode = gameData.Mode,
                Question = ToBase64String(gameData.Question),
                RetryTime = gameData.RetryTime,
                Solutions = CreateSolutionsVm(gameData.Solutions),
                AnswerTimeInMinits = GetAnswerTimeInMinutes(gameMode)
            };
            return vm;
        }

        private int GetAnswerTimeInMinutes(GameMode mode)
        {
            switch (mode)
            {
                case GameMode.Easy:
                    return 3;
                case GameMode.Medium:
                    return 2;
                case GameMode.Hard:
                    return 1;
                default:
                    throw new ArgumentException("Invalid game mode specified.");
            }
        }

        private List<SolutionViewModel> CreateSolutionsVm(List<SolutionDataDto> solutions)
        {
            if (solutions == null)
            {
                throw new ArgumentNullException(nameof(solutions));
            }

            var solutionsVm = new List<SolutionViewModel>();

            foreach (var solutionDto in solutions)
            {
                var solutionVm = new SolutionViewModel
                {
                    Answer = solutionDto.Answer,
                    IsCorrectAnswer = solutionDto.IsCorrectAnswer
                };

                solutionsVm.Add(solutionVm);
            }
            Shuffle(solutionsVm);
            return solutionsVm;
        }

        private void Shuffle(List<SolutionViewModel> solutionsVm)
        {
            // Shuffle the solutionsVm list using Fisher-Yates shuffle algorithm
            Random rng = new Random();
            int n = solutionsVm.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                SolutionViewModel value = solutionsVm[k];
                solutionsVm[k] = solutionsVm[n];
                solutionsVm[n] = value;
            }
        }

        private string ToBase64String(string base64String)
        {
            string imageUrl = "data:image/jpeg;base64," + base64String;
            return imageUrl;
        }
    }
}