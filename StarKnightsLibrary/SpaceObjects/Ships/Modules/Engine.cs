using StarKnightsLibrary.UtilityObjects;

namespace StarKnightsLibrary.SpaceObjects.Ships.Modules
{
    public class Engine : IModule
    {
        public int Power { get; set; }

        public Engine()
        {
            Power = 1;
        }

        public void Accelerate(Velocity velocity)
        {
            velocity.Current += .05f * Power;
        }

        public void Decelerate(Velocity velocity)
        {
            velocity.Current -= .025f * Power;
        }

        public void TurnRight(Orientation orientation, Velocity velocity)
        {
            orientation.Angle += .01f + velocity.Current * .0025f * Power;
        }

        public void TurnLeft(Orientation orientation, Velocity velocity)
        {
            orientation.Angle -= .01f + velocity.Current * .0025f * Power;
        }

        public void Update()
        {
            //Do Nothing
        }
    }
}
