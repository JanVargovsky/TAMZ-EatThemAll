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
            Radius = 10;
        }

        private void GenerateNextMove() => newUpdate = DateTime.Now.AddMilliseconds(random.Next(3000));

        public override bool MoveUpdate()
        {
            if (newUpdate <= DateTime.Now)
            {
                Destination = new Point { X = random.Next(500), Y = random.Next(500) };
                GenerateNextMove();
            }

            return base.MoveUpdate();
        }
    }
}