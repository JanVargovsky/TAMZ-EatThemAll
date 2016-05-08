using EatThemAll.Server.Game.Models;
using System.Collections.Generic;

namespace EatThemAll.Server.Hubs
{
    public interface IEatThemAll
    {
        void SetConnectionId(string connectionId);

        void Update(List<Player> players, List<Food> foods);
    }
}
