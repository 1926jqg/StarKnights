using System;
using System.Runtime.Serialization;
using Xna = Microsoft.Xna.Framework;

namespace StarKnightsLibrary.UtilityObjects.Collision
{
    public static class CollisionManager
    {
        public static bool IsCollision(ICollidable a, ICollidable b)
        {
            var returnVal = false;

            if (a.GetType() == b.GetType())
                returnVal = IsCollisionSame(a, b);
            else
                returnVal = IsCollisionDifferent(a, b);
            return returnVal;
        }

        private static bool IsCollisionSame(ICollidable a, ICollidable b)
        {
            if (a is Rectangle)
                return RectangleCollision(a as Rectangle, b as Rectangle);
            else if (a is Line)
                return LineCollision(a as Line, b as Line);
            else
                throw new UnhandledCollisionTypeException(a, b);
        }

        private static bool IsCollisionDifferent(ICollidable a, ICollidable b)
        {
            if (a is Rectangle)
            {
                var rectangle = a as Rectangle;
                if (b is Line)
                    return LineRectangleCollision(b as Line, rectangle);
            }
            else if (a is Line)
            {
                var line = a as Line;
                if (b is Rectangle)
                    return LineRectangleCollision(line, b as Rectangle);
            }
            throw new UnhandledCollisionTypeException(a, b);
        }

        private static bool RectangleCollision(Rectangle a, Rectangle b)
        {
            return GetXnaRectangle(a).Intersects(GetXnaRectangle(b));
        }

        private static bool LineCollision(Line a, Line b)
        {
            var l1p1 = new Xna.Point((int)a.Orientation.Position.X, (int)a.Orientation.Position.Y);
            var l1p2 = new Xna.Point((int)a.GetEndpoint().X, (int)a.GetEndpoint().Y);
            var l2p1 = new Xna.Point((int)b.Orientation.Position.X, (int)b.Orientation.Position.Y);
            var l2p2 = new Xna.Point((int)b.GetEndpoint().X, (int)b.GetEndpoint().Y);

            return LineIntersectsLine(l1p1, l1p2, l2p1, l2p2);
        }

        private static bool LineIntersectsLine(Xna.Point l1p1, Xna.Point l1p2, Xna.Point l2p1, Xna.Point l2p2)
        {
            float q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
            float d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);

            if (d == 0)
            {
                return false;
            }

            float r = q / d;

            q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
            float s = q / d;

            if (r < 0 || r > 1 || s < 0 || s > 1)
            {
                return false;
            }

            return true;
        }

        private static bool LineRectangleCollision(Line line, Rectangle rectangle)
        {
            var p1 = new Xna.Point((int)line.Orientation.Position.X, (int)line.Orientation.Position.Y);
            var p2 = new Xna.Point((int)line.GetEndpoint().X, (int)line.GetEndpoint().Y);
            var r = GetXnaRectangle(rectangle);
            return LineIntersectsLine(p1, p2, new Xna.Point(r.X, r.Y), new Xna.Point(r.X + r.Width, r.Y)) ||
                   LineIntersectsLine(p1, p2, new Xna.Point(r.X + r.Width, r.Y), new Xna.Point(r.X + r.Width, r.Y + r.Height)) ||
                   LineIntersectsLine(p1, p2, new Xna.Point(r.X + r.Width, r.Y + r.Height), new Xna.Point(r.X, r.Y + r.Height)) ||
                   LineIntersectsLine(p1, p2, new Xna.Point(r.X, r.Y + r.Height), new Xna.Point(r.X, r.Y)) ||
                   (r.Contains(p1) && r.Contains(p2));
        }

        private static Xna.Rectangle GetXnaRectangle(Rectangle rectangle)
        {
            return new Xna.Rectangle(
                (int)rectangle.Orientation.Position.X,
                (int)rectangle.Orientation.Position.Y,
                (int)rectangle.Width,
                (int)rectangle.Height);
        }
    }

    [Serializable]
    public class UnhandledCollisionTypeException : Exception
    {
        public UnhandledCollisionTypeException(ICollidable a, ICollidable b)
            : base(string.Format("Collisions are not defined between {0} and {1}", a.GetType(), b.GetType())) { }

        protected UnhandledCollisionTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
