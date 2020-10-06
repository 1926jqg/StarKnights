using StarKnightsLibrary.GameFlow;
using StarKnightsLibrary.UtilityObjects.Extensions;
using System.Linq;

namespace StarKnightsLibrary.GameObjects.Ships.Deciders
{
    public interface ITargetDecider : IShipDecider { }

    public class TeamTargetDecider : BaseTargetDecider, ITargetDecider
    {
        public TeamTargetDecider() : this(null) { }

        protected TeamTargetDecider(Space space) : base(space) { }

        protected override SpaceObject ChooseTarget(Ship spaceObject)
        {
            return Space.Ships
                .Where(s => s.Team != spaceObject.Team)
                .OrderBy(s => spaceObject.Orientation.Position.Distance(s.Orientation.Position))
                .FirstOrDefault();
        }
    }
}
