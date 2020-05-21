using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace StarKnightsLibrary.UtilityObjects.Collision
{
    public class Rectangle : ICollidable
    {
        public float Width { get; private set; }
        public float Height { get; private set; }
        public Orientation Orientation { get; private set; }

        public Rectangle(int width, int height, float x = 0, float y = 0, float angle = 0)
            : this(width, height, new Orientation(x, y, angle)) { }

        public Rectangle(int width, int height, Orientation orientation)
        {
            Width = width;
            Height = height;
            Orientation = orientation;
        }

        public bool CollidesWith(ICollidable other)
        {
            return CollisionManager.IsCollision(this, other);
        }

        public IEnumerable<Vector2> GetBucketPoints(int xOffset, int yOffset)
        {
            float minX = Orientation.Position.X + xOffset;
            float minY = Orientation.Position.Y + yOffset;
            float maxX = Orientation.Position.X + Width + xOffset;
            float maxY = Orientation.Position.Y + Height + yOffset;

            yield return new Vector2(minX, minY);
            yield return new Vector2(minX, maxY);
            yield return new Vector2(maxX, minY);
            yield return new Vector2(maxX, maxY);
        }
    }
}
