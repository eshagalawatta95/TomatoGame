using System.Collections.Generic;
using TomatoGame.Service.Enum;

namespace TomatoGame.Service.Dto
{
    public class GameDataDto
    {
        public GameMode Mode { get; set; }

        public string Question { get; set; }

        public List<SolutionDataDto> Solutions { get; set; }

        public int RetryTime { get; set; }

    }

    public class GameDataApiResponse
    {
        public string Question { get; set; }
        public int Solution { get; set; }
    }

}
