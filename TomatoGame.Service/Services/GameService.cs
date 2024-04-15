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
        private const int _wrongAnswerCount = 3;

        public async Task<GameDataDto> Start(GameMode mode)
        {
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
                // Add correct answer to solution list
                new SolutionDataDto
                {
                    Answer = solution,
                    IsCorrectAnswer = true
                }
            };

            // Add wrong answers
            solutions.AddRange(ConstructWrongAnswers(solution));
            Shuffle(solutions); //shuffle solution list

            return solutions;
        }

        private void Shuffle(List<SolutionDataDto> solutionDataDto)
        {
            // Shuffle the solutionsVm list using Fisher-Yates shuffle algorithm
            var rng = new Random();
            int n = solutionDataDto.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = solutionDataDto[k];
                solutionDataDto[k] = solutionDataDto[n];
                solutionDataDto[n] = value;
            }
        }

        private List<SolutionDataDto> ConstructWrongAnswers(int solution)
        {
            var exclude = new HashSet<int> { solution };
            Random rand = new Random();

            // Generate a sequence of random numbers between 1 and 21 excluding solution value
            var randomNumbers = Enumerable.Range(1, 20)
                                        .Where(num => !exclude.Contains(num))
                                        .OrderBy(_ => rand.Next())
                                        .Take(_wrongAnswerCount);

            // Create SolutionDataDto objects from the random numbers
            var wrongAnswers = randomNumbers.Select(randomNum => new SolutionDataDto
            {
                Answer = randomNum,
                IsCorrectAnswer = false
            }).ToList();

            return wrongAnswers;
        }
    }

}
