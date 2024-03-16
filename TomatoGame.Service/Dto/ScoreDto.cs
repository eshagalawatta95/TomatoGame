using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomatoGame.Service.Enum;

namespace TomatoGame.Service.Dto
{
    internal class ScoreDto
    {
        public int Id { get; set; }

        public int UserName { get; set; }

        public int UserEmail { get; set; }

        public GameMode Mode { get; set; }

        public float LatestScore { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
