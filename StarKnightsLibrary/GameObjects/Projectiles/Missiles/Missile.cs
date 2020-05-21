namespace StarKnightsLibrary.GameObjects.Projectiles.Missiles
{
    public class Missile : Projectile
    {
        public IDecider<Missile> Decider { get; set; }

        protected override int DamageAmount => 200;

        protected override DamageType DamageType => DamageType.Concussive;

        public override string TextureName => "missile";

        public Missile(float x, float y, float angle, int width, int height, float speed, SpaceObject source, int range, IDecider<Missile> decider)
            : base(x, y, angle, width, height, speed, source, range)
        {
            Decider = decider;
        }

        //private int jerk = 0;
        protected override void AdditionalUpdate()
        {
            Velocity.Current += Velocity.Current / 15;
            Decider.Action(this);
        }

        public override void TakeDamage(int ammount, DamageType type)
        {

        }
    }
}
