using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomatoGame.Service.Dto;
using TomatoGame.Service.Enum;

namespace TomatoGame.Service.Services
{
    public class GameService : IGameService
    {
        public Task<GameDataDto> Begin(GameMode mode)
        {
            throw new NotImplementedException();
        }

        public Task<GameDataDto> ReStart(GameMode mode)
        {
            throw new NotImplementedException();
        }
    }
}
