using EatThemAll.Server.Game.Models;
using EatThemAll.Server.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Threading;

namespace EatThemAll.Server.Game
{
    public sealed class Game
    {
        #region Singleton
        private readonly static Lazy<Game> instance = new Lazy<Game>(() => new Game());
        public static Game Instance => instance.Value;
        #endregion

        private readonly IHubContext hubContext;
        private readonly TimeSpan interval = TimeSpan.FromMilliseconds(1000d / 30d);
        private readonly Timer timer;

        public int Width { get; }
        public int Height { get; }
        public List<Player> Players { get; }

        private Random random = new Random();

        public Game()
        {
            hubContext = GlobalHost.ConnectionManager.GetHubContext<EatThemAllHub>();
            timer = new Timer(GameUpdate, null, TimeSpan.Zero, interval);
            Players = new List<Player>();

            for (int i = 0; i < 10; i++)
                Players.Add(new Bot(Guid.NewGuid().ToString(), $"BOT#{i}"));

            Width = Height = 5000;
        }

        private void GameUpdate(object state)
        {
            foreach (var player in Players)
                player.MoveUpdate();

            hubContext.Clients.All.updateDestination(Players);
        }

        public void UpdateDestination(string playerId, Point point)
        {
            var player = Players.Find(p => p.Id == playerId);

            if (player == null)
                return;

            player.Destination = point;
        }
    }
}