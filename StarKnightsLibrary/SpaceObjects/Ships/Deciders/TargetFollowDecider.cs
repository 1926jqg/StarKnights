using Microsoft.Xna.Framework;
using StarKnightsLibrary.GameFlow;

namespace StarKnightsLibrary.SpaceObjects.Ships
{
    public class TargetFollowDecider : BaseFollowDecider
    {
        public TargetFollowDecider() : this(null) { }

        public TargetFollowDecider(Space space) : base(space) { 
            DecisionDelay = 2;
        }

        public override Vector2? UpdateTarget(Ship spaceObject)
        {
            return spaceObject.Target?.Orientation.Position;
        }

        public override void SetAcceleration(Ship ship)
        {
            var target = ship.Orientation.Position - Target;
            var distance = target.Value.Length();

            if (distance > 150)
            {
                //ship.PowerToEngines();
                ship.Accelerate();
            }
            else
            {
                ship.ResetPower();
            }
        }
    }
}
