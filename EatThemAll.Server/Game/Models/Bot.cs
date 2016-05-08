using EatThemAll.Server.Game.Common;
using System;

namespace EatThemAll.Server.Game.Models
{
    public class Bot : Player
    {
        private DateTime newUpdate;

        public Bot(string id, string name)
            : base(id, name)
        {
            GenerateNextMove();
            Alpha = 0.2;
        }

        private void GenerateNextMove() => newUpdate = DateTime.Now.AddMilliseconds(random.Next(3000));

        public override bool MoveUpdate(int width, int height)
        {
            if (newUpdate <= DateTime.Now)
            {
                Direction = new Vector { X = random.Next(-1, 2) * random.NextDouble(), Y = random.Next(-1, 2) * random.NextDouble() };
                GenerateNextMove();
            }

            return base.MoveUpdate(width, height);
        }
    }
}