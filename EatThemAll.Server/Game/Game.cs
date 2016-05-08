using EatThemAll.Server.Game.Common;
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
        private readonly Random random = new Random();

        public int Width { get; }
        public int Height { get; }
        public List<Player> Players { get; }

        public Game()
        {
            hubContext = GlobalHost.ConnectionManager.GetHubContext<EatThemAllHub>();
            timer = new Timer(GameUpdate, null, TimeSpan.Zero, interval);
            Players = new List<Player>();

            //for (int i = 0; i < 10; i++)
            //    Players.Add(new Bot(Guid.NewGuid().ToString(), $"BOT#{i}"));

            Players.Add(new Player("STATIC1", "STATIC1") { Location = new Point { X = 0, Y = 0 } });
            Players.Add(new Player("STATIC2", "STATIC2") { Location = new Point { X = 0, Y = 100 } });
            Players.Add(new Player("STATIC3", "STATIC3") { Location = new Point { X = 100, Y = 0 } });
            Players.Add(new Player("STATIC4", "STATIC4") { Location = new Point { X = 100, Y = 100 } });
            Width = Height = 1000;
        }

        public void AddNewPlayer(string connectionId)
        {
            Players.Add(new Player(connectionId, $"TAMZ({connectionId.Substring(0, 3)})")
            {
                Location = new Point
                {
                    X = random.Next(0, Width + 1),
                    Y = random.Next(0, Height + 1)
                }
            });
        }

        private void GameUpdate(object state)
        {
            foreach (var player in Players)
                player.MoveUpdate(Width, Height);

            hubContext.Clients.All.update(Players);
        }

        public void UpdateDirection(string playerId, Vector direction)
        {
            var player = Players.Find(p => p.Id == playerId);

            if (player == null)
                return;

            player.Direction = direction;
        }
    }
}