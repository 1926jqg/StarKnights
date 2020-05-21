using StarKnightsLibrary.Scenes;

namespace StarKnightsLibrary.GameObjects.Ships
{
    public abstract class BaseShipDecider : IShipDecider
    {
        protected Space Space { get; private set; }

        protected BaseShipDecider(Space space)
        {
            Initialize(space);
        }

        public abstract void Action(Ship spaceObject);

        public void Initialize(Space space)
        {
            if (Space == null)
            {
                Space = space;
                InitializeTemplate(space);
            }

        }

        protected virtual void InitializeTemplate(Space space)
        {
            // Do Nothing
        }

        public bool IsInBounds(Ship ship)
        {
            return Space.IsInBounds(ship);
        }
    }
}
