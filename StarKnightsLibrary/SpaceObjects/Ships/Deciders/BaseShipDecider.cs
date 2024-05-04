using StarKnightsLibrary.GameFlow;

namespace StarKnightsLibrary.SpaceObjects.Ships
{
    public abstract class BaseShipDecider : IShipDecider
    {
        protected Space Space { get; private set; }

        protected int DecisionDelay { get; set; } = 0;

        private int _decisionDelayCounter;

        protected BaseShipDecider(Space space)
        {
            Initialize(space);
            _decisionDelayCounter = 0;
        }

        public void TakeAction(Ship spaceObject)
        {
            if (_decisionDelayCounter++ >= DecisionDelay) 
            {
                _decisionDelayCounter = 0;
                GetAction(spaceObject);
            }
        }

        protected abstract void GetAction(Ship spaceObject);

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
