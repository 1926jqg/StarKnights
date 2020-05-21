using StarKnightsLibrary.GameObjects.Ships;
using StarKnightsLibrary.Transmissions;
using System.Collections.Generic;

namespace StarKnightsLibrary.Scenes
{
    public interface ISpaceScene : IScene
    {        
        IEnumerable<Ship> Ships { get; }

        void AddTransmission(ITransmission transmission, ulong duration);
    }
}
