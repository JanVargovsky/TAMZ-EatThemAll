using EatThemAll.Server.Game.Common;
using Newtonsoft.Json;

namespace EatThemAll.Server.Game.Models
{
    public class LocationObject
    {
        [JsonProperty("location")]
        [JsonConverter(typeof(PointConverter))]
        public Point Location { get; set; }
    }
}