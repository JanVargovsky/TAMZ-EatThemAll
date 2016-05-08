using EatThemAll.Server.Game.Common;
using EatThemAll.Server.Game.Helpers;
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

        private readonly IHubContext hub;
        private readonly TimeSpan interval = TimeSpan.FromMilliseconds(1000d / 30d);
        private readonly Timer timer;
        private readonly Random random = new Random();
        private readonly FoodFactory foodFactory;
        private readonly int MAX_FOOD = 50;

        public int Width { get; }
        public int Height { get; }
        public List<Player> Players { get; }
        public List<Food> Foods { get; }

        public Game()
        {
            hub = GlobalHost.ConnectionManager.GetHubContext<EatThemAllHub>();
            timer = new Timer(GameUpdate, null, TimeSpan.Zero, interval);
            Players = new List<Player>();
            Foods = new List<Food>();

            Width = Height = 1000;
            foodFactory = new FoodFactory(Width, Height);
                
            for (int i = 0; i < MAX_FOOD; i++)
                Foods.Add(foodFactory.Create());
        }

        private int botId = 0;
        public void AddNewBot()
        {
            Players.Add(new Bot(Guid.NewGuid().ToString(), $"BOT#{botId++}")
            {
                Location = new Point
                {
                    X = random.Next(0, Width),
                    Y = random.Next(0, Height)
                }
            });
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
            Stack<Player> playersToRemove = new Stack<Player>();

            lock (instance)
            {
                foreach (var player in Players)
                {
                    // player moved
                    if (player.MoveUpdate(Width, Height))
                    {
                        // Player collision check
                        foreach (var otherPlayer in Players)
                        {
                            if (player.Id == otherPlayer.Id)
                                continue;

                            if (CollisionHelper.Intersects(player, otherPlayer))
                            {
                                // Player with higher score stays alive
                                // if their score is equal then the player who was "bumped" is dead
                                if (player.Score < otherPlayer.Score)
                                {
                                    playersToRemove.Push(player);
                                    otherPlayer.Score += player.Score + 5;
                                }
                                else
                                {
                                    playersToRemove.Push(otherPlayer);
                                    player.Score += otherPlayer.Score + 5;
                                }
                            }
                        }

                        // Food collision check
                        Stack<Food> foodToRemove = new Stack<Food>();
                        // check if he collide with food
                        foreach (var food in Foods)
                        {
                            if (CollisionHelper.Intersects(player, food))
                            {
                                player.Score += food.Score;
                                foodToRemove.Push(food);
                            }
                        }
                        foreach (var food in foodToRemove)
                        {
                            Foods.Remove(food);
                            Foods.Add(foodFactory.Create());
                        }
                    }
                }

                foreach (var player in playersToRemove)
                    Kill(player);

                hub.Clients.All.Update(Players, Foods);
            }
        }

        public void UpdateDirection(string playerId, Vector direction)
        {
            var player = Players.Find(p => p.Id == playerId);

            if (player == null)
                return;

            player.Direction = direction;
        }

        public void Kill(Player player)
        {
            Players.Remove(player);
            hub.Clients.Client(player.Id).NotifyDead(player.Score);
        }
    }
}