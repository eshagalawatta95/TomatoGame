using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomatoGame.Service.Dto;

namespace TomatoGame.Service.Services
{
    public interface IScoreService
    {
        Task<List<ScoreDto>> GetScores();

        Task<ScoreDto> GetScore(string email);

        Task<bool> UpdateScore(ScoreDto score);

        Task<ScoreDto> GetHighScore();

    }
}
