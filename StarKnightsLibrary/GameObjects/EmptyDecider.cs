namespace StarKnightsLibrary.GameObjects
{
    public class EmptyDecider<T> : IDecider<T>
        where T : SpaceObject
    {
        public void Action(T spaceObject)
        {
            //The Empty Decider does nothing
        }
    }
}
