using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace StarKnightsLibrary.UtilityObjects.Collision
{
    public class Line : ICollidable
    {
        public float Length { get; private set; }
        public Orientation Orientation { get; private set; }

        public Line(float length, float x = 0, float y = 0, float angle = 0)
            : this(length, new Orientation(x, y, angle)) { }

        public Line(float length, Orientation orientation)
        {
            Length = length;
            Orientation = orientation;
        }

        public Vector2 GetEndpoint()
        {
            var x = (float)(Orientation.Position.X + Length * Math.Cos(Orientation.Angle));
            var y = (float)(Orientation.Position.Y + Length * Math.Sin(Orientation.Angle));

            return new Vector2(x, y);
        }

        public bool CollidesWith(ICollidable other)
        {
            return CollisionManager.IsCollision(this, other);
        }

        private static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public IEnumerable<Vector2> GetBucketPoints(int xOffset, int yOffset)
        {
            var endpoint = GetEndpoint();
            return GetBucketPoints(
                (int)Orientation.Position.X + xOffset,
                (int)Orientation.Position.Y + yOffset,
                (int)endpoint.X + xOffset,
                (int)endpoint.Y + yOffset);
        }

        private IEnumerable<Vector2> GetBucketPoints(int x0, int y0, int x1, int y1)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);

            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }

            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }


            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2;
            int ystep = y0 < y1 ? 1 : -1;
            int y = y0;

            for (int x = x0; x <= x1; x++)
            {
                yield return new Vector2(steep ? y : x, steep ? x : y);
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
        }
    }
}
