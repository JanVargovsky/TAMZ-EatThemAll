using EatThemAll.Server.Game.Common;
using System;

namespace EatThemAll.Server.Game.Models
{
    public class FoodFactory
    {
        private int counter = 0;
        private readonly int maxWidth, maxHeight;

        private Random random;

        public FoodFactory(int w, int h)
        {
            maxWidth = w;
            maxHeight = h;
            random = new Random();
        }

        public Food Create()
        {
            return new Food(counter.ToString())
            {
                Location = new Point
                {
                    X = random.Next(0, maxWidth),
                    Y = random.Next(0, maxHeight)
                },
                Score = random.Next(1, 4),
                Alpha = 0.5,
            };
        }
    }

    public class Food : EatableObject
    {
        public override double Radius => Score * 3 + 10;

        public Food(string id)
            : base(id)
        {
        }
    }
}