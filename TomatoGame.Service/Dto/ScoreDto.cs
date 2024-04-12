using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomatoGame.Service.Enum;

namespace TomatoGame.Service.Dto
{
    public class ScoreDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public GameMode Mode { get; set; }

        public double LatestScore { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
