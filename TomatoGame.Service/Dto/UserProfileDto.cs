using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomatoGame.Service.Enum;

namespace TomatoGame.Service.Dto
{
    public class UserProfileDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public GameMode Mode { get; set; }

        public float LatestScore { get; set; }
    }
}
