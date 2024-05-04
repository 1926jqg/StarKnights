using StarKnightsLibrary.GameFlow;

namespace StarKnightsLibrary.SpaceObjects.Ships
{
    public class EmptyShipDecider : BaseShipDecider
    {
        public EmptyShipDecider() : this(null) { }
        public EmptyShipDecider(Space space) : base(space) { }

        protected override void GetAction(Ship spaceObject)
        {
            //Do Nothing
        }
    }
}
