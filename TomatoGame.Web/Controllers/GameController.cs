using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    [SessionAuthorize("UserID")]
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IScoreService _scoreService;
        private readonly int _gameOverRetryCount = 3;
        private readonly string _gameOverMessage = "Game Over !!!. Please try again";

        public GameController(IGameService gameService, IScoreService scoreService)
        {
            _gameService = gameService;
            _scoreService = scoreService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(GameMode gameMode)
        {
            return await GameAction(gameMode);
        }

        [HttpGet]
        public async Task<ActionResult> Retry(GameMode gameMode, int retryTime, int scoreValue, int userId)
        {
            if (retryTime == _gameOverRetryCount) //game over
            {
                SaveHighScore(userId, gameMode, scoreValue);
                return View(new GameOverViewModel()
                {
                    Message = _gameOverMessage
                });
            }
            return await GameAction(gameMode, retryTime);  //retry
        }

        [HttpPost]
        public async Task<ActionResult> TimeIsOver(GameMode gameMode, int scoreValue, int userId)
        {
            SaveHighScore(userId, gameMode, scoreValue);
            return View(new GameOverViewModel()
            {
                Message = _gameOverMessage
            });
        }

        [HttpPost]
        public async Task<ActionResult> CheckAnswer(GameDataViewModel model)
        {
            var correctAnswer = model.Solutions.Where(x=>x.IsCorrectAnswer).First().Answer;
            bool isCorrectAnswer = correctAnswer == model.UserAnswer;

            if (isCorrectAnswer)
            {
                return View(new RightAnswerViewModel());
            }
            else {
                return View(new WrongAnswerViewModel());
            }
        }

        private void SaveHighScore(int userId, GameMode gameMode, int scoreValue)
        {
            var score = new ScoreDto()
            {
                LatestScore = scoreValue,
                Mode = gameMode,
                UserId = userId,
                UpdatedDate = DateTime.UtcNow,
            };
            _scoreService.UpdateScore(score);
        }

        private async Task<ActionResult> GameAction(GameMode gameMode, int? retryTime = null)
        {
            GameDataDto gameData;
            if (retryTime.HasValue)
            {
                gameData = await _gameService.ReStart(gameMode, retryTime.Value);
            }
            else
            {
                gameData = await _gameService.Start(gameMode);
            }

            var vm = new GameDataViewModel()
            {
                Mode = gameData.Mode,
                Question = Base64StringToImage(gameData.Question),
                RetryTime = gameData.RetryTime,
                Solutions = CreateSolutionsVm(gameData.Solutions),
                AnswerTimeInMinits = GetAnswerTimeInMinutes(gameMode)
            };
            return View(vm);
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
            return solutionsVm;
        }

        private Image Base64StringToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                Image image = Image.FromStream(ms);
                return image;
            }
        }
    }
}