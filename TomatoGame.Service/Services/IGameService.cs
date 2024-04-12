using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomatoGame.Service.Dto;
using TomatoGame.Service.Enum;

namespace TomatoGame.Service.Services
{
    public interface IGameService
    {
        Task<GameDataDto> Start(GameMode mode);
    }
}
