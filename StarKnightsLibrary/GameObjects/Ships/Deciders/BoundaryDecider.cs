using Microsoft.Xna.Framework;
using StarKnightsLibrary.Scenes;

namespace StarKnightsLibrary.GameObjects.Ships
{
    public class BoundaryDecider : BaseFollowDecider
    {
        public BoundaryDecider() : this(null) { }

        public BoundaryDecider(Space space) : base(space) { }

        public override Vector2? UpdateTarget(Ship spaceObject)
        {
            var x = (Space.XMax + Space.XMin) / 2;
            var y = (Space.YMax + Space.YMin) / 2;

            return new Vector2(x, y);
        }

        public override void SetAcceleration(Ship ship)
        {
            if (IsInBounds(ship))
            {
                ship.ResetPower();
                ship.Decelerate();
            }
            else
            {
                //ship.PowerToEngines();
                if (ship.Velocity.Current < 2)
                    ship.Accelerate();
            }
        }
    }
}
