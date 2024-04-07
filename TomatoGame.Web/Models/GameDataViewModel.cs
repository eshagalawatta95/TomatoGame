using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using TomatoGame.Service.Enum;

namespace TomatoGame.Web.Models
{
    public class GameDataViewModel
    {
        public GameMode Mode { get; set; }

        public Image Question { get; set; }

        public List<SolutionViewModel> Solutions { get; set; }

        public int RetryTime { get; set; }

        public int AnswerTimeInMinits { get; set; }

        public int UserAnswer { get; set; }

        public int UserId { get; set; }

        public int UserScore { get; set; }
    }
}