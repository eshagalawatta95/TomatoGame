﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomatoGame.Storage
{
    public class Score
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int Mode { get; set; }

        public float LatestScore { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
