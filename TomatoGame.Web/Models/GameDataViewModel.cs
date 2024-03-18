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

        public int Solution { get; set; }
    }
}