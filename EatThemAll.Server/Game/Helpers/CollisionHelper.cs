using EatThemAll.Server.Game.Common;

namespace EatThemAll.Server.Game.Helpers
{
    public class CollisionHelper
    {
        public static bool Intersects(EatableObject a, EatableObject b)
        {
            double distanceX = a.Location.X - b.Location.X;
            double distanceY = a.Location.Y - b.Location.Y;
            double radius = a.Radius + b.Radius;

            return distanceX * distanceX + distanceY * distanceY <= radius * radius;
        }
    }
}