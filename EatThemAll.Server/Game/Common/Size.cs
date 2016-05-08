using Newtonsoft.Json;

namespace EatThemAll.Server.Game.Common
{
    public class Size
    {
        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
}