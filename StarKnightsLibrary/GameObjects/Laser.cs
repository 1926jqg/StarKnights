using Microsoft.Xna.Framework.Graphics;
using StarKnightsLibrary.UtilityObjects;
using StarKnightsLibrary.UtilityObjects.Collision;
using StarKnightsLibrary.UtilityObjects.Extensions;

namespace StarKnightsLibrary.GameObjects
{
    public class Laser : Weapon
    {
        public int Length { get; private set; }

        protected override int DamageAmount { get; }

        protected override DamageType DamageType => DamageType.Thermal;

        public override string TextureName => "Pixel";

        public Laser(float x, float y, float angle, int length, int damageAmmount, SpaceObject source) :
            base(x, y, angle, 0, source)
        {
            Length = length;
            DamageAmount = damageAmmount;
        }


        protected override void AdditionalUpdate()
        {
            Destroyed = true;
        }

        protected override ICollidable GetCollidable()
        {
            return new Line(Length, Orientation);
        }

        public override void TakeDamage(int ammount, DamageType type)
        {

        }

        protected override void HitEffects(SpaceObject other)
        {
            //No Effects
        }

        public override void Draw(SpriteBatch spriteBatch, IContentContainer contentContainer)
        {
            spriteBatch.DrawLine(contentContainer, Orientation.Position, Orientation.Angle, Length);
        }
    }
}
