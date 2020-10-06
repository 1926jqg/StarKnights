using Microsoft.Xna.Framework;
using StarKnightsLibrary.GameFlow;

namespace StarKnightsLibrary.GameObjects.Ships.Deciders
{
    public class CompositeDecider : BaseShipDecider, IFollowDecider, ITargetDecider
    {
        private readonly IFollowDecider _followDecider;
        private readonly ITargetDecider _targetDecider;

        public CompositeDecider(Space space, IFollowDecider followDecider, ITargetDecider targetDecider)
            : base(space)
        {
            _followDecider = followDecider;
            _targetDecider = targetDecider;
        }

        public override void Action(Ship spaceObject)
        {
            _targetDecider.Action(spaceObject);
            _followDecider.Action(spaceObject);
        }

        public void SetAcceleration(Ship ship)
        {
            _followDecider.SetAcceleration(ship);
        }

        public Vector2? UpdateTarget(Ship spaceObject)
        {
            return _followDecider.UpdateTarget(spaceObject);
        }
    }
}
