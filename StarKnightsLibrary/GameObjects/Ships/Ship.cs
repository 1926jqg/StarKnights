using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarKnightsLibrary.GameFlow;
using StarKnightsLibrary.GameObjects.Ships.Modules;
using StarKnightsLibrary.UtilityObjects;
using System.Linq;
using Collision = StarKnightsLibrary.UtilityObjects.Collision;

namespace StarKnightsLibrary.GameObjects.Ships
{
    public class Ship : SpaceObject
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private IShipDecider Decider { get; }

        public SpaceObject Target { get; set; }

        public ShieldGenerator ShieldGenerator { get; }

        public WeaponSystem WeaponSystem { get; }

        public Engine Engine { get; }

        public PowerManagementSystem PowerManagementSystem { get; protected set; }

        public int Hull { get; private set; }

        public int Team { get; }

        private readonly string _textureName;
        public override string TextureName => _textureName;

        public Ship(
            float x,
            float y,
            float angle,
            int width,
            int height,
            float minSpeed,
            float maxSpeed,
            bool isPointOfView,
            IShipDecider decider,
            int team,
            string textureName)
            : base(x, y, angle, minSpeed, maxSpeed, isPointOfView)
        {
            Width = width;
            Height = height;

            Decider = decider;

            Target = null;

            ShieldGenerator = new ShieldGenerator(400, 1)
            {
                Power = 1
            };

            Engine = new Engine();
            WeaponSystem = new WeaponSystem();

            PowerManagementSystem = new PowerManagementSystem(4, 6);
            PowerManagementSystem.TryAddModule(ShieldGenerator);
            PowerManagementSystem.TryAddModule(Engine);
            PowerManagementSystem.TryAddModule(WeaponSystem);
            PowerManagementSystem.ResetPower();

            Hull = 400;

            Team = team;

            _textureName = textureName;
        }

        public void Accelerate()
        {
            Engine.Accelerate(Velocity);
        }

        public void Decelerate()
        {
            Engine.Decelerate(Velocity);
        }

        public override void TurnLeft()
        {
            Engine.TurnLeft(Orientation, Velocity);
        }

        public override void TurnRight()
        {
            Engine.TurnRight(Orientation, Velocity);
        }

        public void FireMissile(Space space, bool seeking)
        {
            WeaponSystem.FireMissile(space, this, seeking);
        }

        public void FireLaser(Space space)
        {
            WeaponSystem.FireLaser(space, this);
        }

        public void ResetPower()
        {
            PowerManagementSystem.ResetPower();
        }

        public bool PowerToShields()
        {
            return PowerManagementSystem.TryAddPowerToModule<ShieldGenerator>();
        }

        public bool PowerToWeapons()
        {
            return PowerManagementSystem.TryAddPowerToModule<WeaponSystem>();
        }

        public bool PowerToEngines()
        {
            return PowerManagementSystem.TryAddPowerToModule<Engine>();
        }

        public void TargetNearestShip(Space space)
        {
            var nearestShip = space.Objects
                .Where(o => o != this)
                .Where(o => o is Ship)
                .OrderBy(o => Vector2.Distance(Orientation.Position, o.Orientation.Position))
                .FirstOrDefault();
            Target = nearestShip;
        }

        protected override void AdditionalUpdate()
        {
            Decider.Action(this);
            if (Target?.Destroyed ?? false)
                Target = null;
            PowerManagementSystem.Update();
        }

        public override void Collide(SpaceObject other)
        {

        }

        protected override Collision.ICollidable GetCollidable()
        {
            var orientation = new Orientation(
                Orientation.Position.X - (float)Width / 2,
                Orientation.Position.Y - (float)Height / 2,
                Orientation.Angle);
            return new Collision.Rectangle(Width, Height, orientation);
        }

        public bool IsInBounds()
        {
            return Decider.IsInBounds(this);
        }

        public override void TakeDamage(int ammount, DamageType type)
        {
            if (ShieldGenerator.ShieldsOnline)
                ShieldGenerator.TakeDamage(ammount, type);
            else
            {
                int damage;
                switch (type)
                {
                    case DamageType.Thermal:
                        damage = ammount / 6;
                        break;
                    case DamageType.Concussive:
                        damage = ammount;
                        break;
                    default:
                        throw new DamageTypeUnexpectedException(type);
                }
                Hull -= damage;
                if (Hull <= 0)
                    Destroyed = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, IContentContainer contentContainer)
        {
            if (IsPointOfView)
            {
                DrawPointOfViewShip(spriteBatch, contentContainer);
            }
            else
            {
                base.Draw(spriteBatch, contentContainer);
            }

        }

        private void DrawPointOfViewShip(SpriteBatch spriteBatch, IContentContainer contentContainer)
        {
            var viewport = spriteBatch.GraphicsDevice.Viewport;
            var texture = contentContainer.LoadTexture(TextureName);
            var screenposShip = new Vector2(
                viewport.Width / 2 - texture.Width / 2,
                viewport.Height - 250 - texture.Height / 2);

            spriteBatch.Draw(texture, screenposShip, Color.White);
        }
    }
}
