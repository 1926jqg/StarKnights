using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace StarKnightsLibrary.UtilityObjects.Collision
{
    public interface ICollidable
    {
        Orientation Orientation { get; }
        bool CollidesWith(ICollidable other);
        IEnumerable<Vector2> GetBucketPoints(int xOffset, int yOffset);
    }
}
