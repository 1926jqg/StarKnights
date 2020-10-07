using Microsoft.Xna.Framework;
using StarKnightsLibrary.GameFlow;

namespace StarKnightsLibrary.SpaceObjects.Ships
{
    public class IntelligentFollowDecider : BaseShipDecider, IFollowDecider
    {
        private readonly TargetFollowDecider FollowDecider;
        private readonly BoundaryDecider BoundaryDecider;

        public IntelligentFollowDecider() : this(null) { }
        public IntelligentFollowDecider(Space space) : base(space)
        {
            FollowDecider = new TargetFollowDecider(space);
            BoundaryDecider = new BoundaryDecider(space);
        }

        private IFollowDecider GetActiveDecider(Ship ship)
        {
            return IsInBounds(ship) ? FollowDecider : (IFollowDecider)BoundaryDecider;
        }

        public override void Action(Ship spaceObject)
        {
            IFollowDecider activeDecider = GetActiveDecider(spaceObject);
            activeDecider.Action(spaceObject);
        }

        public void SetAcceleration(Ship ship)
        {
            IFollowDecider activeDecider = GetActiveDecider(ship);
            activeDecider.SetAcceleration(ship);
        }

        public Vector2? UpdateTarget(Ship spaceObject)
        {
            IFollowDecider activeDecider = GetActiveDecider(spaceObject);
            return activeDecider.UpdateTarget(spaceObject);
        }

        protected override void InitializeTemplate(Space space)
        {
            BoundaryDecider?.Initialize(space);
            FollowDecider?.Initialize(space);
        }
    }
}
