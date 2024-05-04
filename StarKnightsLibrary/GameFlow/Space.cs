using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarKnightsLibrary.SpaceObjects;
using StarKnightsLibrary.SpaceObjects.Projectiles.Missiles;
using StarKnightsLibrary.SpaceObjects.Ships;
using StarKnightsLibrary.SpaceObjects.Ships.Deciders;
using StarKnightsLibrary.UtilityObjects;
using StarKnightsLibrary.UtilityObjects.Collision;
using StarKnightsLibrary.UtilityObjects.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using XNARectangle = Microsoft.Xna.Framework.Rectangle;

namespace StarKnightsLibrary.GameFlow
{
    public class Space
    {
        protected const float CIRCLE = MathHelper.Pi * 2;

        private readonly Dictionary<SpaceObject, int> OutOfBoundsObjects;
        private readonly IList<Weapon> _addedWeapons;

        private readonly List<SpaceObject> ObjectList;
        public IEnumerable<SpaceObject> Objects => ObjectList.AsEnumerable();

        public IEnumerable<Ship> Ships => ObjectList
                    .OfType<Ship>();

        private Ship _pointOfViewObject;
        public Ship PointOfViewObject
        {
            get
            {
                if (_pointOfViewObject == null || _pointOfViewObject.Destroyed)
                {
                    _pointOfViewObject = Ships.SingleOrDefault(o => o.IsPointOfView);
                }
                return _pointOfViewObject;
            }
        }

        public int YMin { get; }
        public int YMax { get; }
        public int XMin { get; }
        public int XMax { get; }
        public int Width { get; }
        public int Height { get; }
        public int OutOfBoundsLimit { get; }
        public ulong Ticks { get; private set; }

        private readonly SpatialHashMap<SpaceObject> _spatialHashMap;

        public Space(int width, int height, int outOfBoundsLimit = 1000)
        {
            ObjectList = new List<SpaceObject>();
            OutOfBoundsObjects = new Dictionary<SpaceObject, int>();
            Width = width;
            Height = height;
            YMax = height / 2;
            XMax = width / 2;
            YMin = YMax - height;
            XMin = XMax - width;
            OutOfBoundsLimit = outOfBoundsLimit;
            Ticks = 0;

            _addedWeapons = new List<Weapon>();
            _spatialHashMap = new SpatialHashMap<SpaceObject>(width, height, 43);
        }

        public bool IsInBounds(SpaceObject obj)
        {
            return
                obj.Orientation.Position.X < XMax &&
                obj.Orientation.Position.X > XMin &&
                obj.Orientation.Position.Y < YMax &&
                obj.Orientation.Position.Y > YMin;
        }

        public IEnumerable<SpaceObject> GetNonPointOfViewObjects()
        {
            return ObjectList.Where(o => !o.IsPointOfView);
        }

        public Ship AddPlayerShip(int x, int y, float angle, int width, int height, int team)
        {
            Ship ship = null;
            if (PointOfViewObject == null)
            {
                ship = new Ship(x, y, angle, width, height, 0, 5, true, new PlayerDecider(this), team, "ShipOne");
                ObjectList.Add(ship);
            }
            return ship;
        }

        public Ship AddNonPlayerShip(int x, int y, float angle, int width, int height, int team)
        {
            return AddNonPlayerShip<TargetFollowDecider>(x, y, angle, width, height, team);
        }

        public Ship AddNonPlayerShip<T>(int x, int y, float angle, int width, int height, int team)
            where T : IShipDecider, new()
        {
            IShipDecider decider = new T();
            decider.Initialize(this);
            return AddNonPlayerShip(x, y, angle, width, height, team, decider);
        }

        private Ship AddNonPlayerShip(int x, int y, float angle, int width, int height, int team, IShipDecider decider)
        {
            Ship ship = new Ship(x, y, angle, width, height, 0, 5, false, decider, team, "ShipTwo");
            ObjectList.Add(ship);
            return ship;
        }

        public Ship AddNonPlayerShip<TFollow, TTarget>(int x, int y, float angle, int width, int height, int team)
            where TFollow : IFollowDecider, new()
            where TTarget : ITargetDecider, new()
        {
            IFollowDecider followDecider = new TFollow();
            followDecider.Initialize(this);
            TTarget targetDecider = new TTarget();
            targetDecider.Initialize(this);

            CompositeDecider decider = new CompositeDecider(this, followDecider, targetDecider);

            return AddNonPlayerShip(x, y, angle, width, height, team, decider);
        }

        public void AddMissile(SpaceObject source, int speed, int range)
        {
            var angle = source.Orientation.Angle;
            AddWeapon(new Missile(source.Orientation.Position.X, source.Orientation.Position.Y, angle, 14, 22, speed, source, range, new EmptyDecider<Missile>()));
        }

        public void AddSeekingMissile(Ship ship, int speed, int range)
        {
            var angle = ship.Orientation.Angle;

            AddWeapon(new Missile(ship.Orientation.Position.X, ship.Orientation.Position.Y, angle, 14, 22, speed, ship, range, new SeekingMissileDecider(ship.Target)));
        }

        public void AddLaser(Ship ship, int damage, int length)
        {
            var angle = ship.Orientation.Angle;
            AddWeapon(new Laser(ship.Orientation.Position.X, ship.Orientation.Position.Y, angle - CIRCLE / 4, length, damage, ship));
        }

        private void AddWeapon(Weapon weapon)
        {
            _addedWeapons.Add(weapon);
        }

        private void DetermineCollisions()
        {
            foreach (var spaceObject in ObjectList)
            {
                foreach (var other in _spatialHashMap.GetNearby(spaceObject))
                {
                    if (spaceObject.CollidesWith(other))
                    {
                        spaceObject.Collide(other);
                    }
                }
            }
        }

        private void DetermineOutOfBounds(SpaceObject spaceObject)
        {
            if (spaceObject == null)
                return;
            bool outOfBoundsRecordExists = OutOfBoundsObjects.TryGetValue(spaceObject, out int outOfBoundsEpochs);
            if (!IsInBounds(spaceObject))
            {
                if (!outOfBoundsRecordExists)
                {
                    OutOfBoundsObjects.Add(spaceObject, 0);
                }
                OutOfBoundsObjects[spaceObject] = ++outOfBoundsEpochs;

                if (OutOfBoundsObjects[spaceObject] >= OutOfBoundsLimit)
                {
                    spaceObject.Destroyed = true;
                }
            }
            else if (outOfBoundsRecordExists) //In Bounds and record exists
            {
                OutOfBoundsObjects.Remove(spaceObject);
            }
        }

        public void Update()
        {
            Parallel.ForEach(ObjectList, o => o?.Update());
            ObjectList.AddRange(_addedWeapons);
            _addedWeapons.Clear();
            _spatialHashMap.Clear();
            for (int i = 0; i < ObjectList.Count; i++)
            {
                var spaceObject = ObjectList.ElementAt(i);
                DetermineOutOfBounds(spaceObject);
                _spatialHashMap.RegisterCollidable(spaceObject);
            }
            DetermineCollisions();

            ObjectList.RemoveAll(o => o.Destroyed);
            Ticks++;
        }

        public void Draw(SpriteBatch spriteBatch, IContentContainer contentContainer)
        {
            var texture = contentContainer.LoadTexture("SpaceBg");

            int xMid = (int)PointOfViewObject.Orientation.Position.X / texture.Width;
            int yMid = (int)PointOfViewObject.Orientation.Position.Y / texture.Height;
            for (int x = xMid - 2; x <= xMid + 2; x++)
            {
                for (int y = yMid - 2; y <= yMid + 2; y++)
                {
                    spriteBatch.Draw(texture, new Vector2(x * texture.Width, y * texture.Height), Color.White);
                }
            }

            var bounds = new XNARectangle(XMin, YMin, Width, Height);

            spriteBatch.DrawBorder(contentContainer, bounds, 1, Color.Green);

            var playerShip = PointOfViewObject;
            var target = playerShip.Target as Ship;
            foreach (var spaceObject in GetNonPointOfViewObjects())
            {
                spaceObject.Draw(spriteBatch, contentContainer);
                if (spaceObject == target)
                {
                    var objectTexture = contentContainer.LoadTexture(spaceObject.TextureName);
                    var targetRect = new Rectangle(
                        (int)spaceObject.Orientation.Position.X - objectTexture.Width / 2,
                        (int)spaceObject.Orientation.Position.Y - objectTexture.Height / 2,
                        objectTexture.Width,
                        objectTexture.Height);
                    spriteBatch.DrawBorder(contentContainer, targetRect, 1, Color.Red);
                }
            }
        }
    }
}
