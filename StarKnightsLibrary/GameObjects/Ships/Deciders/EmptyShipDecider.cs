using StarKnightsLibrary.GameFlow;

namespace StarKnightsLibrary.GameObjects.Ships
{
    public class EmptyShipDecider : BaseShipDecider
    {
        public EmptyShipDecider() : this(null) { }
        public EmptyShipDecider(Space space) : base(space) { }

        public override void Action(Ship spaceObject)
        {
            //Do Nothing
        }
    }
}
