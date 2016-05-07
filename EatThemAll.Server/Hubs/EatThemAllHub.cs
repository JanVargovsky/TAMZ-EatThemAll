using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using EatThemAll.Server.Game.Models;

namespace EatThemAll.Server.Hubs
{
    public class EatThemAllHub : Hub
    {
        private readonly Game.Game game;

        public EatThemAllHub(Game.Game game)
        {
            this.game = game;
        }

        public EatThemAllHub()
            : this(Game.Game.Instance)
        {
        }

        public void UpdateDestination(Point point)
        {
            game.UpdateDestination(Context.ConnectionId, point);
        }

        public override Task OnConnected()
        {
            game.Players.Add(new Player(Context.ConnectionId, $"TAMZ({Context.ConnectionId.Substring(0, 3)})"));
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            game.Players.RemoveAll(p => p.Id == Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
    }
}