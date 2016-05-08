using EatThemAll.Server.Game.Common;
using Newtonsoft.Json;
using System;

namespace EatThemAll.Server.Game.Common
{
    public class LocationObject
    {
        protected static readonly Random random = new Random();

        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("location")]
        [JsonConverter(typeof(PointConverter))]
        public Point Location { get; set; }

        [JsonProperty("alpha")]
        public double Alpha { get; set; }

        public LocationObject(string id)
        {
            Id = id;
            Location = new Point();
            Alpha = 1;
        }
    }
}