using Newtonsoft.Json;
using System;

namespace EatThemAll.Server.Game.Common
{
    public class PointConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Point);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var point = value as Point;

            writer.WriteStartObject();

            writer.WritePropertyName("x");
            serializer.Serialize(writer, Math.Round(point.X, 1));

            writer.WritePropertyName("y");
            serializer.Serialize(writer, Math.Round(point.Y, 1));

            writer.WriteEndObject();
        }
    }

}