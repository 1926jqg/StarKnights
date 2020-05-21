namespace StarKnightsLibrary.GameObjects
{
    public interface IDecider<in T>
        where T : SpaceObject
    {
        void Action(T spaceObject);
    }
}
