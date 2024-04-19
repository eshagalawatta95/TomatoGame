using System;

namespace TomatoGame.Storage
{
    public class Score
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int Mode { get; set; }

        public int LatestScore { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
