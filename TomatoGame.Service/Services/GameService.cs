using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TomatoGame.Service.Dto;
using TomatoGame.Service.Enum;
using TomatoGame.Service.Utils;

namespace TomatoGame.Service.Services
{
    public class GameService : IGameService
    {
        private readonly int _wrongAnswerCount = 3;

        public async Task<GameDataDto> Start(GameMode mode)
        {
            //initially we set try count as 1
            return await StartgameAsync(mode, 1);
        }

        public async Task<GameDataDto> ReStart(GameMode mode, int retryTime)
        {
            return await StartgameAsync(mode, retryTime);
        }

        private async Task<GameDataDto> StartgameAsync(GameMode mode, int retryTime)
        {
            var baseUri = ConfigurationManager.AppSettings["GameBaseUri"];
            var httpClientInstance = new WebApiClient();

            // Call GetClientAsync to get an instance of HttpClient
            var client = httpClientInstance.CreateHttpClient();

            // Now you can use the HttpClient instance to make requests
            HttpResponseMessage response = await client.GetAsync(baseUri + "?out=json&base64=yes");

            if (response.IsSuccessStatusCode)
            {
                // Process the successful response
                string content = await response.Content.ReadAsStringAsync();
                var gameData = JsonConvert.DeserializeObject<GameDataApiResponse>(content);

                var gameDto = CreateGameDataDto(gameData, mode, retryTime);
                return gameDto;
            }
            else
            {
                // Handle the error response
                Console.WriteLine($"Failed to fetch data. Status code: {response.StatusCode}");
                throw new HttpRequestException("Please try Again");
            }
        }

        private GameDataDto CreateGameDataDto(GameDataApiResponse gameData, GameMode mode, int retryTime)
        {
            var gameDto = new GameDataDto
            {
                Question = gameData.Question,
                Mode = mode,
                RetryTime = retryTime,
                Solutions = CreateSolutions(gameData.Solution)
            };
            return gameDto;
        }

        private List<SolutionDataDto> CreateSolutions(int solution)
        {
            var solutions = new List<SolutionDataDto>
            {
                // Add correct answer
                new SolutionDataDto
                {
                    Answer = solution,
                    IsCorrectAnswer = true
                }
            };

            // Add wrong answers
            solutions.AddRange(ConstructWrongAnswers(solution));
            return solutions;
        }

        private List<SolutionDataDto> ConstructWrongAnswers(int solution)
        {
            var wrongAnswers = new List<SolutionDataDto>();
            var exclude = new HashSet<int> { solution };

            for (int j = 0; j < _wrongAnswerCount; j++)
            {
                wrongAnswers.Add(new SolutionDataDto
                {
                    Answer = GiveMeANumber(exclude),
                    IsCorrectAnswer = false
                });
            }
            return wrongAnswers;
        }

        private int GiveMeANumber(HashSet<int> exclude)
        {
            var range = Enumerable.Range(1, 20).Where(i => !exclude.Contains(i));

            var rand = new Random();
            int index = rand.Next(0, 20 - exclude.Count);
            return range.ElementAt(index);
        }
    }
}
