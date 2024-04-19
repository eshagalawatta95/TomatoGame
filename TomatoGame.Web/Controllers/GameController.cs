using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TomatoGame.Service.Dto;
using TomatoGame.Service.Enum;
using TomatoGame.Service.Services;
using TomatoGame.Web.Attributes;
using TomatoGame.Web.Models;

namespace TomatoGame.Web.Controllers
{
    [SessionAuthorize("UserID")] //authentication
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
            var gameData = await StartGame(mode);
            return Json(gameData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<bool> SaveHighScoreAsync(GameMode mode, int scoreValue)
        {
            int userId = 0;
            int.TryParse(Session["UserID"].ToString(), out userId);
            var score = new ScoreDto()
            {
                LatestScore = scoreValue,
                Mode = mode,
                UserId = userId,
                UpdatedDate = DateTime.Now,
            };
             var result = await _scoreService.UpdateScore(score);
            return result;
        }

        private async Task<GameDataViewModel> StartGame(GameMode gameMode)
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

            return solutions.Select(solutionDto => new SolutionViewModel
            {
                Answer = solutionDto.Answer,
                IsCorrectAnswer = solutionDto.IsCorrectAnswer
            }).ToList();
        }

        private string ToBase64String(string base64String)
        {
            string imageUrl = "data:image/jpeg;base64," + base64String;
            return imageUrl;
        }
    }
}