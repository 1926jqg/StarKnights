﻿using Microsoft.Xna.Framework;
using StarKnightsLibrary.GameFlow;
using System;

namespace StarKnightsLibrary.SpaceObjects.Ships
{
    public interface IFollowDecider : IShipDecider
    {
        Vector2? UpdateTarget(Ship spaceObject);
        void SetAcceleration(Ship ship);
    }

    public abstract class BaseFollowDecider : BaseShipDecider, IFollowDecider
    {        
        protected BaseFollowDecider(Space space) : base(space) { }

        protected Vector2? Target { get; private set; }

        public abstract Vector2? UpdateTarget(Ship spaceObject);

        public abstract void SetAcceleration(Ship ship);

        protected override void GetAction(Ship spaceObject)
        {
            Target = UpdateTarget(spaceObject);
            if (!Target.HasValue)
            {
                spaceObject.Decelerate();
                return;
            }

            var heading = new Vector2((float)Math.Sin(spaceObject.Orientation.Angle), -(float)Math.Cos(spaceObject.Orientation.Angle));
            var target = spaceObject.Orientation.Position - Target;

            var cross = (target.Value.X * heading.Y) - (target.Value.Y * heading.X);

            if (cross < 0.0f)
                spaceObject.TurnLeft();
            else if (cross > 0.0f)
                spaceObject.TurnRight();

            if (spaceObject.Target is Ship shipTarget && Math.Abs(cross) < 22.5)
            {
                if (shipTarget?.ShieldGenerator.ShieldsOnline ?? true)
                {
                    if ((shipTarget.Orientation.Position - spaceObject.Orientation.Position).Length() <= 320)
                        spaceObject.FireLaser(Space);
                }
                else
                {
                    spaceObject.FireMissile(Space, true);
                }
            }

            SetAcceleration(spaceObject);
        }
    }
}
