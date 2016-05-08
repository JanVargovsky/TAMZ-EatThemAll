using EatThemAll.Server.Game.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EatThemAll.Server.Game.Models
{
    public class Player : EatableObject
    {
        [JsonProperty("name")]
        public string Name { get; }

        [JsonIgnore]
        public Vector Direction { get; set; }

        [JsonIgnore]
        public double MaxSpeed { get; }

        public Player(string id, string name)
            : base(id)
        {
            Name = name;
            Direction = new Vector();
            Alpha = 1;
            Score = 0;
            MaxSpeed = 2d;
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