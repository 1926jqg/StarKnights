namespace StarKnightsLibrary.GameObjects
{
    public abstract class Weapon : SpaceObject
    {
        public SpaceObject Source { get; private set; }

        protected abstract int DamageAmount { get; }
        protected abstract DamageType DamageType { get; }

        protected Weapon(float x, float y, float angle, float speed, SpaceObject source)
            : base(x, y, angle, (source.Velocity.Current + .1f) / 2, speed, false)
        {
            Source = source;
        }

        public override void Collide(SpaceObject other)
        {
            if (other is Weapon)
            {
                var weapon = other as Weapon;
                if (weapon.Source == Source)
                    return;
            }
            if (other != Source)
            {
                other.TakeDamage(DamageAmount, DamageType);
                HitEffects(other);
            }
        }

        protected abstract void HitEffects(SpaceObject other);
    }
}
