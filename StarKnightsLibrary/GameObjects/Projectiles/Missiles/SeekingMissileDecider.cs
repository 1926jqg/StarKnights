using Microsoft.Xna.Framework;
using System;

namespace StarKnightsLibrary.GameObjects.Projectiles.Missiles
{
    public class SeekingMissileDecider : IDecider<Missile>
    {
        private readonly SpaceObject Target;

        public SeekingMissileDecider(SpaceObject target)
        {
            Target = target;
        }

        public void Action(Missile spaceObject)
        {
            var heading = new Vector2((float)Math.Sin(spaceObject.Orientation.Angle), -(float)Math.Cos(spaceObject.Orientation.Angle));
            var target = spaceObject.Orientation.Position - Target.Orientation.Position;

            var cross = (target.X * heading.Y) - (target.Y * heading.X);

            if (cross < 0.0f)
                spaceObject.TurnLeft();
            else if (cross > 0.0f)
                spaceObject.TurnRight();

            if (Target.Destroyed)
                spaceObject.Decider = new EmptyDecider<Missile>();
        }
    }
}
