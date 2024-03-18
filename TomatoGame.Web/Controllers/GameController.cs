using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TomatoGame.Service.Dto;
using TomatoGame.Service.Services;
using TomatoGame.Web.Models;

namespace TomatoGame.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }
        // GET: Game
        public async Task<ActionResult> Index()
        {
            var gameData = await _gameService.Begin(Service.Enum.GameMode.Easy);
            var vm = new GameDataViewModel()
            {
                Mode = gameData.Mode,
                Question = Base64StringToImage(gameData.Question),
                Solution = gameData.Solution,
            };

            ViewBag.GameData = vm;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CheckAnswer(UserAnswerViewModel model)
        {
            bool isCorrectAnswer = model.CorrectAnwser == model.UserAnswer;

            if (isCorrectAnswer)
            {
                ViewBag.CorrectAnswer = true;
                return RedirectToAction("AnswerCorrect", "Game");
            }
            else
            {
                ViewBag.CorrectAnswer = false;
                ViewBag.ErrorMessage = "Incorrect answer. Try again!";
                return RedirectToAction("WrongAnswer", "Game");
            }
        }

        public async Task<ActionResult> AnswerCorrect()
        {
            return View();
        }

        public async Task<ActionResult> WrongAnswer()
        {
            return View();
        }

        public Image Base64StringToImage(string base64String)
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