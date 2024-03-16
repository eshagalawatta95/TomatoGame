using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomatoGame.Service.Dto;

namespace TomatoGame.Service.Services
{
    public class ScoreService : IScoreService
    {
        Task<ScoreDto> IScoreService.GetHighScore()
        {
            throw new NotImplementedException();
        }

        Task<ScoreDto> IScoreService.GetScore(string email)
        {
            throw new NotImplementedException();
        }

        Task<List<ScoreDto>> IScoreService.GetScores()
        {
            throw new NotImplementedException();
        }

        Task<bool> IScoreService.UpdateScore(ScoreDto score)
        {
            throw new NotImplementedException();
        }
    }
}
