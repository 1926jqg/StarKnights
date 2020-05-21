using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarKnightsLibrary.UtilityObjects;
using StarKnightsLibrary.UtilityObjects.Collision;
using System.Collections.Generic;
using Collision = StarKnightsLibrary.UtilityObjects.Collision;

namespace StarKnightsLibrary.GameObjects
{
    public abstract class SpaceObject : ICollidable
    {
        public Orientation Orientation { get; private set; }

        public Velocity Velocity { get; private set; }

        public bool IsPointOfView { get; private set; }

        public bool Destroyed { get; set; }

        public abstract string TextureName { get; }

        protected SpaceObject(float x, float y, float angle, float minSpeed, float maxSpeed, bool isPointOfView)
        {
            Orientation = new Orientation(x, y, angle);
            Velocity = new Velocity(minSpeed, maxSpeed);

            IsPointOfView = isPointOfView;

            Destroyed = false;
        }

        public virtual void TurnLeft()
        {
            Orientation.Angle -= .01f + Velocity.Current * .005f;
        }

        public virtual void TurnRight()
        {
            Orientation.Angle += .01f + Velocity.Current * .005f;
        }

        public void Update()
        {
            Orientation.Move(Velocity.Current);
            AdditionalUpdate();
        }

        protected abstract Collision.ICollidable GetCollidable();

        /// <summary>
        /// This space object renders an effect on the other space object
        /// </summary>
        /// <param name="other">The other object to be effected</param>
        public abstract void Collide(SpaceObject other);

        public abstract void TakeDamage(int ammount, DamageType type);

        protected abstract void AdditionalUpdate();

        public bool CollidesWith(ICollidable other)
        {
            return GetCollidable().CollidesWith(other);
        }

        public bool CollidesWith(SpaceObject other)
        {
            return GetCollidable().CollidesWith(other.GetCollidable());
        }

        public IEnumerable<Vector2> GetBucketPoints(int xOffset, int yOffset)
        {
            return GetCollidable().GetBucketPoints(xOffset, yOffset);
        }

        public virtual void Draw(SpriteBatch spriteBatch, IContentContainer contentContainer)
        {
            var texture = contentContainer.LoadTexture(TextureName);
            spriteBatch.Draw(texture, Orientation.Position, null, Color.White, Orientation.Angle,
                            new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, SpriteEffects.None, 0f);
        }
    }
}
