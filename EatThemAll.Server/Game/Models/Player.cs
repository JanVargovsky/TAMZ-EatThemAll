using EatThemAll.Server.Game.Common;
using Newtonsoft.Json;
using System;

namespace EatThemAll.Server.Game.Models
{
    public class Player : LocationObject
    {
        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("alpha")]
        public double Alpha { get; set; }

        [JsonProperty("radius")]
        public double Radius => Score + 15;

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonIgnore]
        public Vector Direction { get; set; }

        [JsonIgnore]
        private readonly System.Drawing.Color color;

        [JsonProperty("color")]
        public string Color => $"#{color.R:X2}{color.G:X2}{color.B:X2}";

        protected static readonly Random random = new Random();

        [JsonIgnore]
        public double MaxSpeed { get; }

        public Player(string id, string name)
        {
            Id = id;
            Name = name;
            Direction = new Vector();
            color = System.Drawing.Color.FromArgb(random.Next());
            Alpha = 1;
            Score = 0;
            MaxSpeed = 1.5d;
        }

        public virtual bool MoveUpdate(int width, int height)
        {
            var speed = Direction.Length;
            if (speed == 0)
                return false;

            if (speed > MaxSpeed)
                speed = MaxSpeed;

            var update = Vector.Normalize(Direction, speed);

            var newLocation = new Point
            {
                X = Location.X + update.X,
                Y = Location.Y + update.Y,
            };

            if (newLocation.X < 0)
                newLocation.X = 0;
            else if (newLocation.X > width)
                newLocation.X = width;

            if (newLocation.Y < 0)
                newLocation.Y = 0;
            else if (newLocation.Y > height)
                newLocation.Y = height;

            // Check collisions

            Location = newLocation;

            return true;
        }
    }
}