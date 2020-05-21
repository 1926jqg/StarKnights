namespace StarKnightsLibrary.GameObjects.Ships.Modules
{
    public interface IModule
    {
        int Power { get; set; }

        void Update();
    }
}
