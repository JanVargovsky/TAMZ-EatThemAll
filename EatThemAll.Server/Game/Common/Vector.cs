using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EatThemAll.Server.Game.Common
{
    public class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Length => Math.Sqrt(X * X + Y * Y);

        public void Normalize(double length = 1)
        {
            double ratio = Length / length;

            X /= ratio;
            Y /= ratio;
        }
    }
}