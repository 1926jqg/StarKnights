using StarKnightsLibrary.GameFlow;

namespace StarKnightsLibrary.SpaceObjects.Ships.Modules
{
    public class WeaponSystem : IModule
    {
        public int Power { get; set; }

        private int _missile_counter;
        private int MissileCounter
        {
            get => _missile_counter;
            set => _missile_counter = value > 0 ? value : 0;
        }

        public WeaponSystem()
        {
            Power = 1;
            MissileCounter = 0;
        }

        public void FireMissile(Space space, Ship ship, bool seeking)
        {
            if (MissileCounter == 0)
            {
                if (ship.Target != null && seeking)
                    space.AddSeekingMissile(ship, 25, 300);
                else
                    space.AddMissile(ship, 25, 300);
                MissileCounter = 60;
            }
        }

        public void FireLaser(Space space, Ship ship)
        {
            space.AddLaser(ship, 2 * Power, 300);
        }

        public void Update()
        {
            if (MissileCounter > 0)
            {
                MissileCounter -= Power;
            }
        }
    }
}
