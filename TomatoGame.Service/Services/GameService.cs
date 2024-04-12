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
            return await StartgameAsync(mode);
        }

        private async Task<GameDataDto> StartgameAsync(GameMode mode)
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

                var gameDto = CreateGameDataDto(gameData, mode);
                return gameDto;
            }
            else
            {
                // Handle the error response
                Console.WriteLine($"Failed to fetch data. Status code: {response.StatusCode}");
                throw new HttpRequestException("Please try Again");
            }
        }

        private GameDataDto CreateGameDataDto(GameDataApiResponse gameData, GameMode mode)
        {
            var gameDto = new GameDataDto
            {
                Question = gameData.Question,
                Mode = mode,
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

            Random rand = new Random();

            while (wrongAnswers.Count < _wrongAnswerCount)
            {
                int randomAnswer = rand.Next(1, 21); // Generate a random number between 1 and 20

                if (!exclude.Contains(randomAnswer))
                {
                    wrongAnswers.Add(new SolutionDataDto
                    {
                        Answer = randomAnswer,
                        IsCorrectAnswer = false
                    });
                    exclude.Add(randomAnswer);
                }
            }

            return wrongAnswers;
        }
    }

    }
