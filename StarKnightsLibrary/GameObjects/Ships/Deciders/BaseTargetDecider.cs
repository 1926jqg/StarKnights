using StarKnightsLibrary.GameFlow;

namespace StarKnightsLibrary.GameObjects.Ships.Deciders
{
    public abstract class BaseTargetDecider : BaseShipDecider
    {
        protected BaseTargetDecider(Space space) : base(space) { }

        public override void Action(Ship spaceObject)
        {
            spaceObject.Target = ChooseTarget(spaceObject);
        }

        protected abstract SpaceObject ChooseTarget(Ship spaceObject);
    }
}
