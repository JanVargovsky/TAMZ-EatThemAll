using Newtonsoft.Json;

namespace EatThemAll.Server.Game.Models
{
    public class Point
    {
        [JsonProperty("x")]
        public double X { get; set; }
        [JsonProperty("y")]
        public double Y { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (Point)obj;

            if (other == null)
                return false;

            return X == other.X && Y == other.Y;
        }

        public override string ToString()
        {
            return $"x={X}, y={Y}";
        }
    }
}