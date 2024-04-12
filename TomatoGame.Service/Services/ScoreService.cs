using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using TomatoGame.Service.Dto;
using TomatoGame.Service.Enum;
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
            var scores = await ReadScoreTable().ToListAsync();
            return scores;
        }

        public async Task<ScoreDto> GetScore(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(s => s.Email == email);
            if (user != null)
            {
                var scores = await ReadScoreTable().ToListAsync();
                var userScore = scores.FirstOrDefault(x => x.UserId == user.Id);
                if (userScore == null)
                {
                    return new ScoreDto()
                    {
                        LatestScore= 0,
                        UserId= user.Id,
                        UserName= user.Name,
                        UserEmail= email,
                    };
                }
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
                _context.Scores.AddOrUpdate(existingScore);
                await _context.SaveChangesAsync();
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

        public async Task<ScoreDto> GetHighScoreAsync()
        {
            var scores = await ReadScoreTable().ToListAsync();
            var highScore = scores.FirstOrDefault();
            return highScore;
        }

        private IQueryable<ScoreDto> ReadScoreTable()
        {
            var scores = from score in _context.Scores
                         join user in _context.Users on score.UserId equals user.Id
                         select new ScoreDto
                         {
                             UserEmail = user.Email,
                             LatestScore = score.LatestScore,
                             UserName = user.Name,
                             Id = score.Id,
                             Mode = (Enum.GameMode)score.Mode,
                             UpdatedDate = score.UpdatedTime
                         };

            return scores;
        }

    }
}
