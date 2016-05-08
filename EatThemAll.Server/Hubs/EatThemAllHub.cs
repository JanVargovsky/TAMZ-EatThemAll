using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using EatThemAll.Server.Game.Models;
using EatThemAll.Server.Game.Common;

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

        public void GetConnectionId()
        {
            Clients.Caller.setConnectionId(Context.ConnectionId);
        }

        public void UpdateDirection(Vector direction)
        {
            game.UpdateDirection(Context.ConnectionId, direction);
        }

        public override Task OnConnected()
        {
            game.AddNewPlayer(Context.ConnectionId);

            Clients.Caller.setConnectionId(Context.ConnectionId);

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