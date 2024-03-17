using System;
using System.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TomatoGame.Service.Dto;
using TomatoGame.Service.Enum;
using TomatoGame.Service.Utils;

namespace TomatoGame.Service.Services
{
    public class GameService : IGameService
    {
        public int RetryTime { get; private set; }

        public async Task<GameDataDto> Begin(GameMode mode)
        {
            return await StartgameAsync(mode);
        }

        public async Task<GameDataDto> ReStart(GameMode mode)
        {
            RetryTime++;
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
                var gameData = JsonSerializer.Deserialize<GameDataDto>(content);
                gameData.Mode = mode;
                return gameData;
            }
            else
            {
                // Handle the error response
                Console.WriteLine($"Failed to fetch data. Status code: {response.StatusCode}");
                throw new HttpRequestException("Please try Again");
            }
        }
    }
}
