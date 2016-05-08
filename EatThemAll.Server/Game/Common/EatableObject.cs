using Newtonsoft.Json;
using System.Drawing;

namespace EatThemAll.Server.Game.Common
{
    public class EatableObject : LocationObject
    {
        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("radius")]
        public virtual double Radius => Score + 15;

        [JsonIgnore]
        private readonly Color color;

        [JsonProperty("color")]
        public string Color => $"#{color.R:X2}{color.G:X2}{color.B:X2}";

        public EatableObject(string id, Color color)
            : base(id)
        {
            this.color = color;
            Score = 0;
        }

        public EatableObject(string id)
            : this(id, System.Drawing.Color.FromArgb(random.Next()))
        {

        }
    }
}