using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomatoGame.Service.Dto;
using TomatoGame.Storage;

namespace TomatoGame.Service.Services
{
    public class ScoreService : IScoreService
    {
        private readonly GameDbContext _context;

        public ScoreService(GameDbContext context)
        {
            _context = context;
        }

        public async Task<List<ScoreDto>> GetScores()
        {
            var scores = await _context.Scores
              .Select(s => new ScoreDto { UserEmail = "", LatestScore = s.LatestScore })
              .ToListAsync();
            return scores;
        }

        public async Task<ScoreDto> GetScore(string email)
        {
            var id =0;
            var score = await _context.Scores
                .Where(s => s.Id == id)
                .Select(s => new ScoreDto { UserEmail = "", LatestScore = s.LatestScore })
                .FirstOrDefaultAsync();
            return score;
        }

        public async Task<bool> UpdateScore(ScoreDto score)
        {
            var existingScore = await _context.Scores.FirstOrDefaultAsync(s => s.UserId == score.UserId);
            if (existingScore != null)
            {
                existingScore.LatestScore = score.LatestScore;
                existingScore.UpdatedTime = score.UpdatedDate;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<ScoreDto> GetHighScore()
        {
            var highScore = await _context.Scores
              .OrderByDescending(s => s.LatestScore)
              .Select(s => new ScoreDto { UserEmail = "", LatestScore = s.LatestScore })
              .FirstOrDefaultAsync();
            return highScore;
        }
    }
}
