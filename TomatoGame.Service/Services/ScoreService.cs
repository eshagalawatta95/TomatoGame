using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
            var query = ReadScoreTable();
            var scores = await query.ToListAsync();
            return scores;
        }

        public async Task<ScoreDto> GetScore(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(s => s.Email == email);
            if (user == null)
            {
                var scores = ReadScoreTable();
                var userScore = scores.Where(x => x.UserId == user.Id).FirstOrDefault();
                return userScore;
            }
            throw new InvalidOperationException($"User not found for {email}");
        }

        public async Task<bool> UpdateScore(ScoreDto score)
        {
            var existingScore = await _context.Scores
                            .FirstOrDefaultAsync(s => s.UserId == score.UserId && s.Mode == (int)score.Mode);
            if (existingScore != null)
            {
                existingScore.LatestScore = score.LatestScore;
                existingScore.UpdatedTime = score.UpdatedDate;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                var newScore = new Score()
                {
                    Mode = (int)score.Mode,
                    UserId = score.UserId,
                    UpdatedTime = score.UpdatedDate,
                };
                _context.Scores.Add(newScore);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public ScoreDto GetHighScore()
        {
            var query = ReadScoreTable();
            var highScore = query.FirstOrDefault();
            return highScore;
        }

        private IQueryable<ScoreDto> ReadScoreTable()
        {
            var scores = _context.Scores
                .Join(_context.Users, score => score.UserId, user => user.Id,
                    (score, user) => new ScoreDto
                    {
                        UserEmail = user.Email,
                        LatestScore = score.LatestScore,
                        UserName = user.Name,
                        Id = score.Id,
                        Mode = (Enum.GameMode)score.Mode,
                        UpdatedDate = score.UpdatedTime
                    })
                .OrderByDescending(x => x.LatestScore)
                .AsNoTracking();

            return scores;
        }
    }
}
