using EatThemAll.Server.Game.Common;
using Newtonsoft.Json;
using System;
using System.Drawing;

namespace EatThemAll.Server.Game.Models
{
    public class Player
    {
        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("location")]
        public Point Location { get; set; }

        [JsonProperty("alpha")]
        public double Alpha { get; set; }

        [JsonProperty("radius")]
        public double Radius { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonIgnore]
        public Point Destination { get; set; }

        [JsonIgnore]
        public double Speed { get; }

        [JsonIgnore]
        private readonly Color color;

        [JsonProperty("color")]
        public string Color => $"#{color.R:X2}{color.G:X2}{color.B:X2}";

        protected static readonly Random random = new Random();

        public Player(string id, string name)
        {
            Id = id;
            Name = name;
            Speed = 1.5d;

#if DEBUG
            Location = Destination = new Point { X = random.Next(500), Y = random.Next(500) };
#else
            Location = new Point();
            Destination = new Point();
#endif
            color = System.Drawing.Color.FromArgb(random.Next());
            Alpha = 1;
            Radius = 15;
        }

        public virtual bool MoveUpdate()
        {
            if (Location.Equals(Destination))
                return false;

            // TODO: Set and store it when Destination is set
            Vector vector = new Vector
            {
                X = Destination.X - Location.X,
                Y = Destination.Y - Location.Y,
            };

            vector.Normalize(vector.Length < Speed ? vector.Length : Speed);

            Location.X += vector.X;
            Location.Y += vector.Y;

            return true;
        }
    }
}