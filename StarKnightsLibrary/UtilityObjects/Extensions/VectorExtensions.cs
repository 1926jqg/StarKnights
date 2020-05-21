using Microsoft.Xna.Framework;
using System;

namespace StarKnightsLibrary.UtilityObjects.Extensions
{
    public static class VectorExtensions
    {
        public static double Distance(this Vector2 vector1, Vector2 vector2)
        {
            return Math.Sqrt(
                (vector1.X - vector2.X) * (vector1.X - vector2.X) +
                (vector1.Y - vector2.Y) * (vector1.Y - vector2.Y));
        }
    }
}
