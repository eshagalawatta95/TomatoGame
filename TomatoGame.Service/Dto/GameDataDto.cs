using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomatoGame.Service.Enum;

namespace TomatoGame.Service.Dto
{
    public class GameDataDto
    {
        public int Id { get; set; }

        public GameMode Mode { get; set; }

        public string Question { get; set; }

        public int Solution { get; set; }

    }
}
