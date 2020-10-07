using StarKnightsLibrary.UtilityObjects.Collision;

namespace StarKnightsLibrary.SpaceObjects.Projectiles
{
    public abstract class Projectile : Weapon
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Range { get; private set; }
        public int Age { get; private set; }

        protected Projectile(float x, float y, float angle, int width, int height, float speed, SpaceObject source, int range)
            : base(x, y, angle, speed, source)
        {
            Range = range;
            Age = 0;
            Width = width;
            Height = height;
        }


        protected override void AdditionalUpdate()
        {
            if (++Age > Range)
                Destroyed = true;
        }

        protected override void HitEffects(SpaceObject other)
        {
            Destroyed = true;
        }

        protected override ICollidable GetCollidable()
        {
            return new Rectangle(Width, Height, Orientation);
        }
    }
}
