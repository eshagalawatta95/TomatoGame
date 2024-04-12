using System;
using TomatoGame.Service.Enum;

namespace TomatoGame.Web.Models
{
    public class ScoreViewModel
    {
        public string UserName { get; set; }

        public GameMode Mode { get; set; }

        public double LatestScore { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}