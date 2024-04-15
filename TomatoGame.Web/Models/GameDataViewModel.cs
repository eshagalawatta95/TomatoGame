using System.Collections.Generic;
using TomatoGame.Service.Enum;

namespace TomatoGame.Web.Models
{
    public class GameDataViewModel
    {
        public GameMode Mode { get; set; }

        public string Question { get; set; }

        public List<SolutionViewModel> Solutions { get; set; }

        public int RetryTime { get; set; }

        public int AnswerTimeInMinits { get; set; }
    }
}