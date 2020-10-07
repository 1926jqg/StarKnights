namespace StarKnightsLibrary.SpaceObjects.Ships.Modules
{
    public interface IModule
    {
        int Power { get; set; }

        void Update();
    }
}
