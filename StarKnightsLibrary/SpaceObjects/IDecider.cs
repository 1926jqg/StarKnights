namespace StarKnightsLibrary.SpaceObjects
{
    public interface IDecider<in T>
        where T : SpaceObject
    {
        void TakeAction(T spaceObject);
    }
}
