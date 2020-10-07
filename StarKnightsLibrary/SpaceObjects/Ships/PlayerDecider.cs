using Microsoft.Xna.Framework.Input;
using StarKnightsLibrary.GameFlow;

namespace StarKnightsLibrary.SpaceObjects.Ships
{
    public class PlayerDecider : BaseShipDecider
    {
        public PlayerDecider(Space space) : base(space) { }

        public override void Action(Ship spaceObject)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || GamePad.GetState(1).IsButtonDown(Buttons.LeftTrigger))
                spaceObject.Accelerate();
            else
                spaceObject.Decelerate();

            if (Keyboard.GetState().IsKeyDown(Keys.A) || GamePad.GetState(1).ThumbSticks.Right.X < 0)
                spaceObject.TurnLeft();
            if (Keyboard.GetState().IsKeyDown(Keys.D) || GamePad.GetState(1).ThumbSticks.Right.X > 0)
                spaceObject.TurnRight();

            if (Keyboard.GetState().IsKeyDown(Keys.W) || GamePad.GetState(1).IsButtonDown(Buttons.A))
                spaceObject.TargetNearestShip(Space);

            if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(1).IsButtonDown(Buttons.RightTrigger))
                spaceObject.FireLaser(Space);
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || GamePad.GetState(1).IsButtonDown(Buttons.RightShoulder))
                spaceObject.FireMissile(Space, true);

            if (Keyboard.GetState().IsKeyDown(Keys.D1) || GamePad.GetState(1).IsButtonDown(Buttons.X))
                spaceObject.PowerToEngines();
            if (Keyboard.GetState().IsKeyDown(Keys.D2) || GamePad.GetState(1).IsButtonDown(Buttons.Y))
                spaceObject.PowerToShields();
            if (Keyboard.GetState().IsKeyDown(Keys.D3) || GamePad.GetState(1).IsButtonDown(Buttons.B))
                spaceObject.PowerToWeapons();
            if (Keyboard.GetState().IsKeyDown(Keys.OemTilde))
                spaceObject.ResetPower();
        }
    }
}
