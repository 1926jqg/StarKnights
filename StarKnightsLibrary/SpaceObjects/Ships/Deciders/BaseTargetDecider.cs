using StarKnightsLibrary.GameFlow;

namespace StarKnightsLibrary.SpaceObjects.Ships.Deciders
{
    public abstract class BaseTargetDecider : BaseShipDecider
    {
        protected BaseTargetDecider(Space space) : base(space) { }

        protected override void GetAction(Ship spaceObject)
        {
            spaceObject.Target = ChooseTarget(spaceObject);
        }

        protected abstract SpaceObject ChooseTarget(Ship spaceObject);
    }
}
